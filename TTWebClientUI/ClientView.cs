using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTWebClient;
using TTWebClient.Domain;

namespace TTWebClientUI
{
    internal class FeedTickView : ObservableObject
    {
        public string Symbol { get; set; }
        public DateTime Timestamp { get; set; }
        public string BestBid { get; set; }
        public string BestAsk { get; set; }

        public FeedTickView(TTFeedTick tick)
        {
            Symbol = tick.Symbol;
            Timestamp = tick.Timestamp;
            BestBid = $"{tick.BestBid.Price}/{tick.BestBid.Volume}";
            BestAsk = $"{tick.BestAsk.Price}/{tick.BestAsk.Volume}";
        }
    }

    internal class FeedTickL2
    {
        public string Symbol { get; set; }
        public DateTime Timestamp { get; set; }
        public string BestBid { get; set; }
        public string BestAsk { get; set; }
        public string Bids { get; set; }
        public string Asks { get; set; }

        public FeedTickL2(TTFeedTickLevel2 tick)
        {
            Symbol = tick.Symbol;
            Timestamp = tick.Timestamp;
            BestBid = $"{tick.BestBid.Price}/{tick.BestBid.Volume}";
            BestAsk = $"{tick.BestAsk.Price}/{tick.BestAsk.Volume}";
            Bids = string.Join(" ", tick.Bids.Select(b => $"{b.Price}/{b.Volume}"));
            Asks = string.Join(" ", tick.Asks.Select(b => $"{b.Price}/{b.Volume}"));
        }
    }

    internal class ClientView : ObservableObject
    {
        private readonly TickTraderWebClient _client;
        private ObservableCollection<KeyValuePair<string, object>> _publicTradeSession = new ObservableCollection<KeyValuePair<string, object>>();
        private ObservableCollection<TTCurrency> _publicCurrencies;
        private string _publicCurrenciesFilter;
        private ObservableCollection<TTSymbol> _publicSymbols;
        private string _publicSymbolsFilter;
        private ObservableCollection<FeedTickView> _publicTicks;
        private string _publicTicksFilter;
        private ObservableCollection<FeedTickL2> _publicTicksL2;
        private string _publicTicksL2Filter;

        public ObservableCollection<KeyValuePair<string, object>> PublicTradeSession
        {
            get { return _publicTradeSession; }
            set
            {
                _publicTradeSession = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TTCurrency> PublicCurrencies
        {
            get { return _publicCurrencies; }
            set
            {
                _publicCurrencies = value;
                OnPropertyChanged();
            }
        }

        public string PublicCurrenciesFilter
        {
            get { return _publicCurrenciesFilter; }
            set
            {
                _publicCurrenciesFilter = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TTSymbol> PublicSymbols
        {
            get { return _publicSymbols; }
            set
            {
                _publicSymbols = value;
                OnPropertyChanged();
            }
        }

        public string PublicSymbolsFilter
        {
            get { return _publicSymbolsFilter; }
            set
            {
                _publicSymbolsFilter = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FeedTickView> PublicTicks
        {
            get { return _publicTicks; }
            set
            {
                _publicTicks = value;
                OnPropertyChanged();
            }
        }

        public string PublicTicksFilter
        {
            get { return _publicTicksFilter; }
            set
            {
                _publicTicksFilter = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FeedTickL2> PublicTicksL2
        {
            get { return _publicTicksL2; }
            set
            {
                _publicTicksL2 = value;
                OnPropertyChanged();
            }
        }

        public string PublicTicksL2Filter
        {
            get { return _publicTicksL2Filter; }
            set
            {
                _publicTicksL2Filter = value;
                OnPropertyChanged();
            }
        }

        static ClientView()
        {
            // Optional: Force to ignore server certificate 
            TickTraderWebClient.IgnoreServerCertificate();
        }

        public ClientView(string webApiAddress, string webApiId, string webApiKey, string webApiSecret)
        {
            // Create instance of the TickTrader Web API client
            _client = new TickTraderWebClient(webApiAddress, webApiId, webApiKey, webApiSecret);
        }

        public ClientView(CredsModel creds)
        {
            _client = new TickTraderWebClient(creds.WebApiAddress, creds.WebApiId, creds.WebApiKey, creds.WebApiSecret);
        }

        #region Public trade session information

        public async Task GetPublicTradeSession()
        {
            PublicTradeSession.Clear();
            // Public trade session
            var tradeSession = await _client.GetPublicTradeSessionAsync();
            PublicTradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.PlatformName), tradeSession.PlatformName));
            PublicTradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.PlatformCompany), tradeSession.PlatformCompany));
            PublicTradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.PlatformAddress), tradeSession.PlatformAddress));
            PublicTradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.PlatformTimezoneOffset), tradeSession.PlatformTimezoneOffset));
            PublicTradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.SessionId), tradeSession.SessionId));
            PublicTradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.SessionStatus), tradeSession.SessionStatus));
            PublicTradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.SessionStartTime), tradeSession.SessionStartTime));
            PublicTradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.SessionEndTime), tradeSession.SessionEndTime));
            PublicTradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.SessionOpenTime), tradeSession.SessionOpenTime));
            PublicTradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.PlatformAddress), tradeSession.PlatformAddress));
        }

        #endregion

        #region Public currencies, symbols, feed ticks, feed ticks level2 information

        public async Task GetPublicCurrencies()
        {
            // Public currencies
            List<TTCurrency> publicCurrencies;

            if (string.IsNullOrEmpty(PublicCurrenciesFilter))
                publicCurrencies = await _client.GetPublicAllCurrenciesAsync();
            else
                publicCurrencies = await _client.GetPublicCurrencyAsync(PublicCurrenciesFilter);

            PublicCurrencies = publicCurrencies != null ? new ObservableCollection<TTCurrency>(publicCurrencies) : null;
        }

        public async Task GetPublicSymbols()
        {
            // Public symbols
            List<TTSymbol> publicSymbols;

            if (string.IsNullOrEmpty(PublicSymbolsFilter))
                publicSymbols = await _client.GetPublicAllSymbolsAsync();
            else
                publicSymbols = await _client.GetPublicSymbolAsync(PublicSymbolsFilter);

            PublicSymbols = publicSymbols != null ? new ObservableCollection<TTSymbol>(publicSymbols) : null;
        }

        public async Task GetPublicTicks()
        {
            // Public feed ticks
            List<TTFeedTick> publicTicks;

            if (string.IsNullOrEmpty(PublicTicksFilter))
                publicTicks = await _client.GetPublicAllTicksAsync();
            else
                publicTicks = await _client.GetPublicTickAsync(PublicTicksFilter);

            PublicTicks = publicTicks != null ? new ObservableCollection<FeedTickView>(publicTicks.Select(t=>new FeedTickView(t))) : null;
        }

        public async Task GetPublicTicksLevel2()
        {
            // Public feed ticks level2 
            List<TTFeedTickLevel2> publicTicksLevel2;

            if (string.IsNullOrEmpty(PublicTicksL2Filter))
                publicTicksLevel2 = await _client.GetPublicAllTicksLevel2Async();
            else
                publicTicksLevel2 = await _client.GetPublicTickLevel2Async(PublicTicksL2Filter);

            PublicTicksL2 = publicTicksLevel2 != null ? new ObservableCollection<FeedTickL2>(publicTicksLevel2.Select(t => new FeedTickL2(t))) : null;
        }

        public async Task GetPublicTickers()
        {
            // Public symbol statistics
            List<TTTicker> publicTickers = await _client.GetPublicAllTickersAsync();
            foreach (var t in publicTickers)
                Console.WriteLine("{0} last buy/sell prices : {1} / {2}", t.Symbol, t.LastBuyPrice, t.LastSellPrice);

            TTTicker publicTicker = (await _client.GetPublicTickerAsync(publicTickers[0].Symbol)).FirstOrDefault();
            if (publicTicker != null)
                Console.WriteLine("{0} best bid/ask: {1} / {2}", publicTicker.Symbol, publicTicker.BestBid, publicTicker.BestAsk);
        }

        #endregion

        #region Account information

        public async Task GetAccount()
        {
            // Account info
            TTAccount account = await _client.GetAccountAsync();
            Console.WriteLine("Account Id: {0}", account.Id);
            Console.WriteLine("Account name: {0}", account.Name);
            Console.WriteLine("Account group: {0}", account.Group);
        }

        #endregion

        #region Account trade session information

        public async Task GetTradeSession()
        {
            // Account trade session
            TTTradeSession tradesession = await _client.GetTradeSessionAsync();
            Console.WriteLine("Trade session status: {0}", tradesession.SessionStatus);
        }

        #endregion

        #region Account currencies, symbols, feed ticks, feed ticks level2 information

        public async Task GetCurrencies()
        {
            // Account currencies
            List<TTCurrency> currencies = await _client.GetAllCurrenciesAsync();
            foreach (var c in currencies)
                Console.WriteLine("Currency: " + c.Name);

            TTCurrency currency = (await _client.GetCurrencyAsync(currencies[0].Name)).FirstOrDefault();
            if (currency != null)
                Console.WriteLine("{0} currency precision: {1}", currency.Name, currency.Precision);
        }

        public async Task GetSymbols()
        {
            // Account symbols
            List<TTSymbol> symbols = await _client.GetAllSymbolsAsync();
            foreach (var s in symbols)
                Console.WriteLine("Symbol: " + s.Symbol);

            TTSymbol symbol = (await _client.GetSymbolAsync(symbols[0].Symbol)).FirstOrDefault();
            if (symbol != null)
                Console.WriteLine("{0} symbol precision: {1}", symbol.Symbol, symbol.Precision);
        }

        public async Task GetTicks()
        {
            // Account feed ticks
            List<TTFeedTick> ticks = await _client.GetAllTicksAsync();
            foreach (var t in ticks)
                Console.WriteLine("{0} tick: {1}, {2}", t.Symbol, t.BestBid.Price, t.BestAsk.Price);

            TTFeedTick tick = (await _client.GetTickAsync(ticks[0].Symbol)).FirstOrDefault();
            if (tick != null)
                Console.WriteLine("{0} tick timestamp: {1}", tick.Symbol, tick.Timestamp);
        }

        public async Task GetTicksLevel2()
        {
            // Account feed ticks level2 
            List<TTFeedTickLevel2> ticksLevel2 = await _client.GetAllTicksLevel2Async();
            foreach (var t in ticksLevel2)
                Console.WriteLine("{0} level2 book depth: {1}", t.Symbol, Math.Max(t.Bids.Count, t.Asks.Count));

            TTFeedTickLevel2 tickLevel2 = (await _client.GetTickLevel2Async(ticksLevel2[0].Symbol)).FirstOrDefault();
            if (tickLevel2 != null)
                Console.WriteLine("{0} level2 book depth: {1}", tickLevel2.Symbol, Math.Max(tickLevel2.Bids.Count, tickLevel2.Asks.Count));
        }

        #endregion

        #region Account assets, positions, trades information

        public async Task GetAssets()
        {
            // Account assets
            TTAccount account = await _client.GetAccountAsync();
            if (account.AccountingType == TTAccountingTypes.Cash)
            {
                List<TTAsset> assets = await _client.GetAllAssetsAsync();
                foreach (var a in assets)
                    Console.WriteLine("{0} asset: {1}", a.Currency, a.Amount);
            }
        }

        public async Task GetPositions()
        {
            // Account positions
            TTAccount account = await _client.GetAccountAsync();
            if (account.AccountingType == TTAccountingTypes.Net)
            {
                List<TTPosition> positions = await _client.GetAllPositionsAsync();
                foreach (var p in positions)
                    Console.WriteLine("{0} position: {1} {2}", p.Symbol, p.LongAmount, p.ShortAmount);
            }
        }

        public async Task GetTrades()
        {
            // Account trades
            List<TTTrade> trades = await _client.GetAllTradesAsync();
            foreach (var t in trades)
                Console.WriteLine("{0} trade with type {1} by symbol {2}: {3}", t.Id, t.Type, t.Symbol, t.Amount);

            TTTrade trade = await _client.GetTradeAsync(trades[0].Id);
            Console.WriteLine("{0} trade with type {1} by symbol {2}: {3}", trade.Id, trade.Type, trade.Symbol, trade.Amount);
        }

        #endregion

        #region Account trade history information

        public async Task GetTradeHistory()
        {
            int iterations = 3;
            var request = new TTTradeHistoryRequest { TimestampTo = DateTime.UtcNow, RequestDirection = TTStreamingDirections.Backward, RequestPageSize = 10 };

            // Try to get trade history from now to the past. Request is limited to 30 records!
            while (iterations-- > 0)
            {
                TTTradeHistoryReport report = await _client.GetTradeHistoryAsync(request);
                foreach (var record in report.Records)
                {
                    Console.WriteLine("TradeHistory record: Id={0}, TransactionType={1}, TransactionReason={2}, Symbol={3}, TradeId={4}", record.Id, record.TransactionType, record.TransactionReason, record.Symbol, record.TradeId);
                    request.RequestLastId = record.Id;
                }

                // Stop for last report
                if (report.IsLastReport)
                    break;
            }
        }

        #endregion

        #region Create, modify and cancel limit order

        public async Task LimitOrder()
        {
            // Create, modify and cancel limit order
            TTAccount account = await _client.GetAccountAsync();
            if ((account.AccountingType == TTAccountingTypes.Gross) || (account.AccountingType == TTAccountingTypes.Net))
            {
                // Create limit order
                var limit = await _client.CreateTradeAsync(new TTTradeCreate
                {
                    Type = TTOrderTypes.Limit,
                    Side = TTOrderSides.Buy,
                    Symbol = (account.AccountingType == TTAccountingTypes.Gross) ? "EURUSD" : "EUR/USD",
                    Amount = 10000,
                    Price = 1.0M,
                    Comment = "Buy limit from Web API sample"
                });

                // Modify limit order
                limit = await _client.ModifyTradeAsync(new TTTradeModify
                {
                    Id = limit.Id,
                    Comment = "Modified limit from Web API sample"
                });

                // Cancel limit order
                await _client.CancelTradeAsync(limit.Id);
            }
        }

        #endregion

    }
}
