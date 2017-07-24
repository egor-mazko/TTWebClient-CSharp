using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTWebClient;
using TTWebClient.Domain;

namespace TTWebClientUI
{
    internal class ClientModel
    {
        private readonly TickTraderWebClient _client;

        static ClientModel()
        {
            // Optional: Force to ignore server certificate 
            TickTraderWebClient.IgnoreServerCertificate();
        }

        public ClientModel(string webApiAddress, string webApiId, string webApiKey, string webApiSecret)
        {
            // Create instance of the TickTrader Web API client
            _client = new TickTraderWebClient(webApiAddress, webApiId, webApiKey, webApiSecret);
        }

        #region Public trade session information

        public void GetPublicTradeSession()
        {
            // Public trade session
            TTTradeSession publictradesession = _client.GetPublicTradeSession();
            Console.WriteLine("TickTrader name: {0}", publictradesession.PlatformName);
            Console.WriteLine("TickTrader company: {0}", publictradesession.PlatformCompany);
            Console.WriteLine("TickTrader address: {0}", publictradesession.PlatformAddress);
            Console.WriteLine("TickTrader timezone offset: {0}", publictradesession.PlatformTimezoneOffset);
            Console.WriteLine("TickTrader session status: {0}", publictradesession.SessionStatus);
        }

        #endregion

        #region Public currencies, symbols, feed ticks, feed ticks level2 information

        public void GetPublicCurrencies()
        {
            // Public currencies
            List<TTCurrency> publicCurrencies = _client.GetPublicAllCurrencies();
            foreach (var c in publicCurrencies)
                Console.WriteLine("Currency: " + c.Name);

            TTCurrency publicCurrency = _client.GetPublicCurrency(publicCurrencies[0].Name).FirstOrDefault();
            if (publicCurrency != null)
                Console.WriteLine("{0} currency precision: {1}", publicCurrency.Name, publicCurrency.Precision);
        }

        public void GetPublicSymbols()
        {
            // Public symbols
            List<TTSymbol> publicSymbols = _client.GetPublicAllSymbols();
            foreach (var s in publicSymbols)
                Console.WriteLine("Symbol: " + s.Symbol);

            TTSymbol publicSymbol = _client.GetPublicSymbol(publicSymbols[0].Symbol).FirstOrDefault();
            if (publicSymbol != null)
                Console.WriteLine("{0} symbol precision: {1}", publicSymbol.Symbol, publicSymbol.Precision);
        }

        public void GetPublicTicks()
        {
            // Public feed ticks
            List<TTFeedTick> publicTicks = _client.GetPublicAllTicks();
            foreach (var t in publicTicks)
                Console.WriteLine("{0} tick: {1}, {2}", t.Symbol, t.BestBid.Price, t.BestAsk.Price);

            TTFeedTick publicTick = _client.GetPublicTick(publicTicks[0].Symbol).FirstOrDefault();
            if (publicTick != null)
                Console.WriteLine("{0} tick timestamp: {1}", publicTick.Symbol, publicTick.Timestamp);
        }

        public void GetPublicTicksLevel2()
        {
            // Public feed ticks level2 
            List<TTFeedTickLevel2> publicTicksLevel2 = _client.GetPublicAllTicksLevel2();
            foreach (var t in publicTicksLevel2)
                Console.WriteLine("{0} level2 book depth: {1}", t.Symbol, Math.Max(t.Bids.Count, t.Asks.Count));

            TTFeedTickLevel2 publicTickLevel2 = _client.GetPublicTickLevel2(publicTicksLevel2[0].Symbol).FirstOrDefault();
            if (publicTickLevel2 != null)
                Console.WriteLine("{0} level2 book depth: {1}", publicTickLevel2.Symbol, Math.Max(publicTickLevel2.Bids.Count, publicTickLevel2.Asks.Count));
        }

        public void GetPublicTickers()
        {
            // Public symbol statistics
            List<TTTicker> publicTickers = _client.GetPublicAllTickers();
            foreach (var t in publicTickers)
                Console.WriteLine("{0} last buy/sell prices : {1} / {2}", t.Symbol, t.LastBuyPrice, t.LastSellPrice);

            TTTicker publicTicker = _client.GetPublicTicker(publicTickers[0].Symbol).FirstOrDefault();
            if (publicTicker != null)
                Console.WriteLine("{0} best bid/ask: {1} / {2}", publicTicker.Symbol, publicTicker.BestBid, publicTicker.BestAsk);
        }

        #endregion

        #region Account information

        public void GetAccount()
        {
            // Account info
            TTAccount account = _client.GetAccount();
            Console.WriteLine("Account Id: {0}", account.Id);
            Console.WriteLine("Account name: {0}", account.Name);
            Console.WriteLine("Account group: {0}", account.Group);
        }

        #endregion

        #region Account trade session information

        public void GetTradeSession()
        {
            // Account trade session
            TTTradeSession tradesession = _client.GetTradeSession();
            Console.WriteLine("Trade session status: {0}", tradesession.SessionStatus);
        }

        #endregion

        #region Account currencies, symbols, feed ticks, feed ticks level2 information

        public void GetCurrencies()
        {
            // Account currencies
            List<TTCurrency> currencies = _client.GetAllCurrencies();
            foreach (var c in currencies)
                Console.WriteLine("Currency: " + c.Name);

            TTCurrency currency = _client.GetCurrency(currencies[0].Name).FirstOrDefault();
            if (currency != null)
                Console.WriteLine("{0} currency precision: {1}", currency.Name, currency.Precision);
        }

        public void GetSymbols()
        {
            // Account symbols
            List<TTSymbol> symbols = _client.GetAllSymbols();
            foreach (var s in symbols)
                Console.WriteLine("Symbol: " + s.Symbol);

            TTSymbol symbol = _client.GetSymbol(symbols[0].Symbol).FirstOrDefault();
            if (symbol != null)
                Console.WriteLine("{0} symbol precision: {1}", symbol.Symbol, symbol.Precision);
        }

        public void GetTicks()
        {
            // Account feed ticks
            List<TTFeedTick> ticks = _client.GetAllTicks();
            foreach (var t in ticks)
                Console.WriteLine("{0} tick: {1}, {2}", t.Symbol, t.BestBid.Price, t.BestAsk.Price);

            TTFeedTick tick = _client.GetTick(ticks[0].Symbol).FirstOrDefault();
            if (tick != null)
                Console.WriteLine("{0} tick timestamp: {1}", tick.Symbol, tick.Timestamp);
        }

        public void GetTicksLevel2()
        {
            // Account feed ticks level2 
            List<TTFeedTickLevel2> ticksLevel2 = _client.GetAllTicksLevel2();
            foreach (var t in ticksLevel2)
                Console.WriteLine("{0} level2 book depth: {1}", t.Symbol, Math.Max(t.Bids.Count, t.Asks.Count));

            TTFeedTickLevel2 tickLevel2 = _client.GetTickLevel2(ticksLevel2[0].Symbol).FirstOrDefault();
            if (tickLevel2 != null)
                Console.WriteLine("{0} level2 book depth: {1}", tickLevel2.Symbol, Math.Max(tickLevel2.Bids.Count, tickLevel2.Asks.Count));
        }

        #endregion

        #region Account assets, positions, trades information

        public void GetAssets()
        {
            // Account assets
            TTAccount account = _client.GetAccount();
            if (account.AccountingType == TTAccountingTypes.Cash)
            {
                List<TTAsset> assets = _client.GetAllAssets();
                foreach (var a in assets)
                    Console.WriteLine("{0} asset: {1}", a.Currency, a.Amount);
            }
        }

        public void GetPositions()
        {
            // Account positions
            TTAccount account = _client.GetAccount();
            if (account.AccountingType == TTAccountingTypes.Net)
            {
                List<TTPosition> positions = _client.GetAllPositions();
                foreach (var p in positions)
                    Console.WriteLine("{0} position: {1} {2}", p.Symbol, p.LongAmount, p.ShortAmount);
            }
        }

        public void GetTrades()
        {
            // Account trades
            List<TTTrade> trades = _client.GetAllTrades();
            foreach (var t in trades)
                Console.WriteLine("{0} trade with type {1} by symbol {2}: {3}", t.Id, t.Type, t.Symbol, t.Amount);

            TTTrade trade = _client.GetTrade(trades[0].Id);
            Console.WriteLine("{0} trade with type {1} by symbol {2}: {3}", trade.Id, trade.Type, trade.Symbol, trade.Amount);
        }

        #endregion

        #region Account trade history information

        public void GetTradeHistory()
        {
            int iterations = 3;
            var request = new TTTradeHistoryRequest { TimestampTo = DateTime.UtcNow, RequestDirection = TTStreamingDirections.Backward, RequestPageSize = 10 };

            // Try to get trade history from now to the past. Request is limited to 30 records!
            while (iterations-- > 0)
            {
                TTTradeHistoryReport report = _client.GetTradeHistory(request);
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

        public void LimitOrder()
        {
            // Create, modify and cancel limit order
            TTAccount account = _client.GetAccount();
            if ((account.AccountingType == TTAccountingTypes.Gross) || (account.AccountingType == TTAccountingTypes.Net))
            {
                // Create limit order
                var limit = _client.CreateTrade(new TTTradeCreate
                {
                    Type = TTOrderTypes.Limit,
                    Side = TTOrderSides.Buy,
                    Symbol = (account.AccountingType == TTAccountingTypes.Gross) ? "EURUSD" : "EUR/USD",
                    Amount = 10000,
                    Price = 1.0M,
                    Comment = "Buy limit from Web API sample"
                });

                // Modify limit order
                limit = _client.ModifyTrade(new TTTradeModify
                {
                    Id = limit.Id,
                    Comment = "Modified limit from Web API sample"
                });

                // Cancel limit order
                _client.CancelTrade(limit.Id);
            }
        }

        #endregion

    }
}
