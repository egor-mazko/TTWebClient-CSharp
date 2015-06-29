using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TTWebClient;
using TTWebClient.Domain;
using TTWebClientRobot.Statistic;

namespace TTWebRobot
{
    public enum Action
    {
        OPEN_TRADE_MARKET,
        OPEN_TRADE_LIMIT,
        OPEN_TRADE_STOP,
        CANCEL_TRADE,
        CLOSE_TRADE
    }

    public class WebRobot
    {
        #region Properties

        public TickTraderWebClient WebClient { get; private set; }

        #endregion

        #region Constructor

        public WebRobot(TickTraderWebClient client)
        {
            WebClient = client;
        }

        #endregion

        #region Public methods

        public void Start()
        {
            _executionCancellationTokenSource = new CancellationTokenSource();
            _executionTask = Task.Factory.StartNew(ExecutionLoop, _executionCancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        public void Stop()
        {
            _executionCancellationTokenSource.Cancel();
            try
            {
                _executionTask.Wait();
            }
            catch (AggregateException e)
            {
                foreach (var v in e.InnerExceptions)
                    Console.WriteLine(e.Message + " " + v.Message);
            }
            finally
            {
                _executionCancellationTokenSource.Dispose();
            }
        }

        public void PrintStatistic()
        {
            _statistic.PrintStatistic();
        }

        public void RunInConsole()
        {
            Start();

            // Loop until Esc is pressed or cancellation is requested...
            Console.WriteLine("Press Esc to cancel the exection...");
            do
            {
                while (!Console.KeyAvailable)
                {
                    // Sleep for a while...
                    Thread.Sleep(1000);

                    if (_executionTask.IsCompleted || _executionCancellationTokenSource.IsCancellationRequested)
                        break;                    
                }
                if (_executionTask.IsCompleted || _executionCancellationTokenSource.IsCancellationRequested)
                    break;
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            Console.WriteLine();

            Stop();

            PrintStatistic();

            Console.ReadKey();
        }

        #endregion

        #region Execution loop

        private readonly Random _random = new Random();
        private readonly Aggregator _statistic = new Aggregator();

        private Task _executionTask;
        private CancellationTokenSource _executionCancellationTokenSource;

        private TTAccount _account;
        private Dictionary<string, TTSymbol> _symbols = new Dictionary<string, TTSymbol>();
        private Dictionary<string, TTFeedTick> _ticks = new Dictionary<string, TTFeedTick>();
        private Dictionary<long, TTTrade> _tradesAll = new Dictionary<long, TTTrade>();
        private Dictionary<long, TTTrade> _tradesMarket = new Dictionary<long, TTTrade>();
        private Dictionary<long, TTTrade> _tradesPending = new Dictionary<long, TTTrade>();

        private void ExecutionLoop()
        {
            while (!_executionCancellationTokenSource.IsCancellationRequested)
            {
                RefreshTables();

                Action action = ChooseAction();
                string result = PerfromAction(action);
                Console.WriteLine("Operation: " + action + ". Result: " + result);

                // Sleep for a while...
                Thread.Sleep(1000);
            }

            CloseAllMarketTrades();
            CancelAllPendingTrades();
        }

        private void RefreshTables()
        {
            // Refresh account
            _account = WebClient.GetAccount();
            // Refresh symbols table
            _symbols = WebClient.GetAllSymbols().ToDictionary(s => s.Symbol);
            // Refresh feed ticks table
            _ticks = WebClient.GetAllTicks().ToDictionary(t => t.Symbol);            
        }

        private Action ChooseAction()
        {
            Array values = Enum.GetValues(typeof (Action));
            return (Action)values.GetValue(_random.Next(values.Length));
        }

        private string PerfromAction(Action action)
        {
            try
            {
                switch (action)
                {
                    case Action.OPEN_TRADE_MARKET:
                        return OpenTradeMarket();
                    case Action.OPEN_TRADE_LIMIT:
                        return OpenTradeLimit();
                    case Action.OPEN_TRADE_STOP:
                        return OpenTradeStop();
                    case Action.CANCEL_TRADE:
                        return CancelTrade();
                    case Action.CLOSE_TRADE:
                        return CloseTrade();
                }
                return "Unknown";
            }
            catch (HttpRequestException ex)
            {
                return ex.Message;
            }
        }

        private void CloseAllMarketTrades()
        {
            try
            {
                if (_account == null)
                    return;
                if (_account.AccountingType != TTAccountingTypes.Gross)
                    return;
                if (_tradesMarket.Count == 0)
                    return;

                Console.WriteLine("Final close all market trades... Count = " + _tradesMarket.Count);
                foreach (var trade in _tradesMarket.Values)
                {
                    WebClient.CloseTrade(trade.Id, null);
                    _tradesAll.Remove(trade.Id);
                    Console.WriteLine(trade.Type + " " + trade.Side + " trade with Id = " + trade.Id + " was closed");
                }
                _tradesMarket.Clear();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }                        
        }

        private void CancelAllPendingTrades()
        {
            try
            {
                if (_tradesPending.Count == 0)
                    return;

                Console.WriteLine("Final cancel all pending trades... Count = " + _tradesPending.Count);
                foreach (var trade in _tradesPending.Values)
                {
                    WebClient.CancelTrade(trade.Id);
                    _tradesAll.Remove(trade.Id);
                    Console.WriteLine(trade.Type + " " + trade.Side + " trade with Id = " + trade.Id + " was canceled");
                }
                _tradesPending.Clear();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }

        private string OpenTradeMarket()
        {
            if (_symbols.Count == 0)
                return "There is no symbols avaliable!";

            TTSymbol symbol = _symbols.ElementAt(_random.Next(_symbols.Count)).Value;
            TTOrderSides side = (_random.Next()%2 == 0) ? TTOrderSides.Buy : TTOrderSides.Sell;
            decimal amount = symbol.MinTradeAmount;

            if (!symbol.IsTradeAllowed)
                return "Trade is not allowed for symbol " + symbol.Symbol;

            TTTrade trade;
            using (_statistic.Benchmark("Open Market Trade"))
            {
                trade = WebClient.CreateTrade(new TTTradeCreate
                {
                    Type = TTOrderTypes.Market,
                    Side = side,
                    Symbol = symbol.Symbol,
                    Amount = amount,
                    Comment = "TTWebRobot market"
                });
            }

            _tradesAll.Add(trade.Id, trade);
            _tradesMarket.Add(trade.Id, trade);
            return trade.Type + " " + trade.Side + " trade is opened with Id = " + trade.Id;
        }

        private string OpenTradeLimit()
        {
            if (_symbols.Count == 0)
                return "There is no symbols avaliable!";

            TTSymbol symbol = _symbols.ElementAt(_random.Next(_symbols.Count)).Value;
            TTOrderSides side = (_random.Next() % 2 == 0) ? TTOrderSides.Buy : TTOrderSides.Sell;
            decimal amount = symbol.MinTradeAmount;
            decimal price = (side == TTOrderSides.Buy) ? _ticks[symbol.Symbol].BestAsk.Price - 20.0m : _ticks[symbol.Symbol].BestBid.Price + 20.0m;

            if (!symbol.IsTradeAllowed)
                return "Trade is not allowed for symbol " + symbol.Symbol;

            TTTrade trade;
            using (_statistic.Benchmark("Open Limit Trade"))
            {
                trade = WebClient.CreateTrade(new TTTradeCreate
                {
                    Type = TTOrderTypes.Limit,
                    Side = side,
                    Symbol = symbol.Symbol,
                    Amount = amount,
                    Price = price,
                    Comment = "TTWebRobot limit"
                });
            }

            _tradesAll.Add(trade.Id, trade);
            _tradesPending.Add(trade.Id, trade);
            return trade.Type + " " + trade.Side + " trade is opened with Id = " + trade.Id;
        }

        private string OpenTradeStop()
        {
            if (_symbols.Count == 0)
                return "There is no symbols avaliable!";

            TTSymbol symbol = _symbols.ElementAt(_random.Next(_symbols.Count)).Value;
            TTOrderSides side = (_random.Next() % 2 == 0) ? TTOrderSides.Buy : TTOrderSides.Sell;
            decimal amount = symbol.MinTradeAmount;
            decimal price = (side == TTOrderSides.Buy) ? _ticks[symbol.Symbol].BestAsk.Price + 20.0m : _ticks[symbol.Symbol].BestBid.Price - 20.0m;

            if (!symbol.IsTradeAllowed)
                return "Trade is not allowed for symbol " + symbol.Symbol;

            TTTrade trade;
            using (_statistic.Benchmark("Open Stop Trade"))
            {
                trade = WebClient.CreateTrade(new TTTradeCreate
                {
                    Type = TTOrderTypes.Stop,
                    Side = side,
                    Symbol = symbol.Symbol,
                    Amount = amount,
                    Price = price,
                    Comment = "TTWebRobot stop"
                });
            }

            _tradesAll.Add(trade.Id, trade);
            _tradesPending.Add(trade.Id, trade);
            return trade.Type + " " + trade.Side + " trade is opened with Id = " + trade.Id;
        }

        private string CancelTrade()
        {
            if (_tradesPending.Count == 0)
                return "There is no active pending trades";

            TTTrade trade = _tradesPending.ElementAt(_random.Next(_tradesPending.Count)).Value;

            using (_statistic.Benchmark("Cancel Pending Trade"))
            {
                WebClient.CancelTrade(trade.Id);
            }

            _tradesAll.Remove(trade.Id);
            _tradesPending.Remove(trade.Id);
            return trade.Type + " " + trade.Side + " trade with Id = " + trade.Id + " was canceled";
        }

        private string CloseTrade()
        {
            if (_account == null)
                return "Account information is not avaliable!";
            if (_account.AccountingType != TTAccountingTypes.Gross)
                return "Close trade operation is only avaliable for Gross accounts!";
            if (_tradesMarket.Count == 0)
                return "There is no active market trades";

            TTTrade trade = _tradesMarket.ElementAt(_random.Next(_tradesMarket.Count)).Value;

            using (_statistic.Benchmark("Close Position Trade"))
            {
                WebClient.CloseTrade(trade.Id, null);
            }

            _tradesAll.Remove(trade.Id);
            _tradesMarket.Remove(trade.Id);
            return trade.Type + " " + trade.Side + " trade with Id = " + trade.Id + " was closed";
        }

        #endregion
    }
}
