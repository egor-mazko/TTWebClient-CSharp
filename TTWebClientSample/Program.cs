using System;
using System.Collections.Generic;
using System.Linq;
using TTWebClient;
using TTWebClient.Domain;

namespace TTWebClientSample
{
    class Program
    {
        #region Main method

        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: TTWebClientSample.exe WebApiAddress WebApiId WebApiKey WebApiSecret");
                return;
            }

            string webApiAddress = args[0];
            string webApiId = args[1];
            string webApiKey = args[2];
            string webApiSecret = args[3];

            // Optional: Force to ignore server certificate 
            TickTraderWebClient.IgnoreServerCertificate();

            // Create instance of the TickTrader Web API client
            var client = new TickTraderWebClient(webApiAddress, webApiId, webApiKey, webApiSecret);

            Console.WriteLine("--- Public Web API methods ---");

            GetPublicTradeSession(client);

            GetPublicCurrencies(client);
            GetPublicSymbols(client);
            GetPublicTicks(client);
            GetPublicTicksLevel2(client);

            Console.WriteLine("--- Web API client methods ---");

            GetAccount(client);
            GetTradeSession(client);

            GetCurrencies(client);
            GetSymbols(client);
            GetTicks(client);
            GetTicksLevel2(client);

            GetAssets(client);
            GetPositions(client);
            GetTrades(client);
            GetTradeHistory(client);

            LimitOrder(client);
        }

        #endregion

        #region Public trade session information

        public static void GetPublicTradeSession(TickTraderWebClient client)
        {
            // Public trade session
            TTTradeSession publictradesession = client.GetPublicTradeSession();
            Console.WriteLine("TickTrader name: {0}", publictradesession.PlatformName);
            Console.WriteLine("TickTrader company: {0}", publictradesession.PlatformCompany);
            Console.WriteLine("TickTrader address: {0}", publictradesession.PlatformAddress);
            Console.WriteLine("TickTrader timezone offset: {0}", publictradesession.PlatformTimezoneOffset);
            Console.WriteLine("TickTrader session status: {0}", publictradesession.SessionStatus);            
        }

        #endregion

        #region Public currencies, symbols, feed ticks, feed ticks level2 information

        public static void GetPublicCurrencies(TickTraderWebClient client)
        {
            // Public currencies
            List<TTCurrency> publicCurrencies = client.GetPublicAllCurrencies();
            foreach (var c in publicCurrencies)
                Console.WriteLine("Currency: " + c.Name);

            TTCurrency publicCurrency = client.GetPublicCurrency(publicCurrencies[0].Name).FirstOrDefault();
            if (publicCurrency != null)
                Console.WriteLine("{0} currency precision: {1}", publicCurrency.Name, publicCurrency.Precision);
        }

        public static void GetPublicSymbols(TickTraderWebClient client)
        {
            // Public symbols
            List<TTSymbol> publicSymbols = client.GetPublicAllSymbols();
            foreach (var s in publicSymbols)
                Console.WriteLine("Symbol: " + s.Symbol);

            TTSymbol publicSymbol = client.GetPublicSymbol(publicSymbols[0].Symbol).FirstOrDefault();
            if (publicSymbol != null)
                Console.WriteLine("{0} symbol precision: {1}", publicSymbol.Symbol, publicSymbol.Precision);
        }

        public static void GetPublicTicks(TickTraderWebClient client)
        {
            // Public feed ticks
            List<TTFeedTick> publicTicks = client.GetPublicAllTicks();
            foreach (var t in publicTicks)
                Console.WriteLine("{0} tick: {1}, {2}", t.Symbol, t.BestBid.Price, t.BestAsk.Price);

            TTFeedTick publicTick = client.GetPublicTick(publicTicks[0].Symbol).FirstOrDefault();
            if (publicTick != null)
                Console.WriteLine("{0} tick timestamp: {1}", publicTick.Symbol, publicTick.Timestamp);
        }

        public static void GetPublicTicksLevel2(TickTraderWebClient client)
        {
            // Public feed ticks level2 
            List<TTFeedTickLevel2> publicTicksLevel2 = client.GetPublicAllTicksLevel2();
            foreach (var t in publicTicksLevel2)
                Console.WriteLine("{0} level2 book depth: {1}", t.Symbol, Math.Max(t.Bids.Count, t.Asks.Count));

            TTFeedTickLevel2 publicTickLevel2 = client.GetPublicTickLevel2(publicTicksLevel2[0].Symbol).FirstOrDefault();
            if (publicTickLevel2 != null)
                Console.WriteLine("{0} level2 book depth: {1}", publicTickLevel2.Symbol, Math.Max(publicTickLevel2.Bids.Count, publicTickLevel2.Asks.Count));
        }

        public static void GetPublicTickers(TickTraderWebClient client)
        {
            // Public symbol statistics
            List<TTTicker> publicTickers = client.GetPublicAllTickers();
            foreach (var t in publicTickers)
                Console.WriteLine("{0} last buy/sell prices : {1} / {2}", t.Symbol, t.LastBuyPrice, t.LastSellPrice);

            TTTicker publicTicker = client.GetPublicTicker(publicTickers[0].Symbol).FirstOrDefault();
            if (publicTicker != null)
                Console.WriteLine("{0} best bid/ask: {1} / {2}", publicTicker.Symbol, publicTicker.BestBid, publicTicker.BestAsk);
        }

        #endregion

        #region Account information

        public static void GetAccount(TickTraderWebClient client)
        {
            // Account info
            TTAccount account = client.GetAccount();
            Console.WriteLine("Account Id: {0}", account.Id);
            Console.WriteLine("Account name: {0}", account.Name);
            Console.WriteLine("Account group: {0}", account.Group);
        }

        #endregion

        #region Account trade session information

        public static void GetTradeSession(TickTraderWebClient client)
        {
            // Account trade session
            TTTradeSession tradesession = client.GetTradeSession();
            Console.WriteLine("Trade session status: {0}", tradesession.SessionStatus);
        }

        #endregion

        #region Account currencies, symbols, feed ticks, feed ticks level2 information

        public static void GetCurrencies(TickTraderWebClient client)
        {
            // Account currencies
            List<TTCurrency> currencies = client.GetAllCurrencies();
            foreach (var c in currencies)
                Console.WriteLine("Currency: " + c.Name);

            TTCurrency currency = client.GetCurrency(currencies[0].Name).FirstOrDefault();
            if (currency != null)
                Console.WriteLine("{0} currency precision: {1}", currency.Name, currency.Precision);
        }

        public static void GetSymbols(TickTraderWebClient client)
        {
            // Account symbols
            List<TTSymbol> symbols = client.GetAllSymbols();
            foreach (var s in symbols)
                Console.WriteLine("Symbol: " + s.Symbol);

            TTSymbol symbol = client.GetSymbol(symbols[0].Symbol).FirstOrDefault();
            if (symbol != null)
                Console.WriteLine("{0} symbol precision: {1}", symbol.Symbol, symbol.Precision);
        }

        public static void GetTicks(TickTraderWebClient client)
        {
            // Account feed ticks
            List<TTFeedTick> ticks = client.GetAllTicks();
            foreach (var t in ticks)
                Console.WriteLine("{0} tick: {1}, {2}", t.Symbol, t.BestBid.Price, t.BestAsk.Price);

            TTFeedTick tick = client.GetTick(ticks[0].Symbol).FirstOrDefault();
            if (tick != null)
                Console.WriteLine("{0} tick timestamp: {1}", tick.Symbol, tick.Timestamp);
        }

        public static void GetTicksLevel2(TickTraderWebClient client)
        {
            // Account feed ticks level2 
            List<TTFeedTickLevel2> ticksLevel2 = client.GetAllTicksLevel2();
            foreach (var t in ticksLevel2)
                Console.WriteLine("{0} level2 book depth: {1}", t.Symbol, Math.Max(t.Bids.Count, t.Asks.Count));

            TTFeedTickLevel2 tickLevel2 = client.GetTickLevel2(ticksLevel2[0].Symbol).FirstOrDefault();
            if (tickLevel2 != null)
                Console.WriteLine("{0} level2 book depth: {1}", tickLevel2.Symbol, Math.Max(tickLevel2.Bids.Count, tickLevel2.Asks.Count));
        }

        #endregion

        #region Account assets, positions, trades information

        public static void GetAssets(TickTraderWebClient client)
        {
            // Account assets
            TTAccount account = client.GetAccount();
            if (account.AccountingType == TTAccountingTypes.Cash)
            {
                List<TTAsset> assets = client.GetAllAssets();
                foreach (var a in assets)
                    Console.WriteLine("{0} asset: {1}", a.Currency, a.Amount);
            }
        }

        public static void GetPositions(TickTraderWebClient client)
        {
            // Account positions
            TTAccount account = client.GetAccount();
            if (account.AccountingType == TTAccountingTypes.Net)
            {
                List<TTPosition> positions = client.GetAllPositions();
                foreach (var p in positions)
                    Console.WriteLine("{0} position: {1} {2}", p.Symbol, p.LongAmount, p.ShortAmount);
            }
        }

        public static void GetTrades(TickTraderWebClient client)
        {
            // Account trades
            List<TTTrade> trades = client.GetAllTrades();
            foreach (var t in trades)
                Console.WriteLine("{0} trade with type {1} by symbol {2}: {3}", t.Id, t.Type, t.Symbol, t.Amount);

            TTTrade trade = client.GetTrade(trades[0].Id);
            Console.WriteLine("{0} trade with type {1} by symbol {2}: {3}", trade.Id, trade.Type, trade.Symbol, trade.Amount);
        }

        #endregion

        #region Account trade history information

        public static void GetTradeHistory(TickTraderWebClient client)
        {
            int iterations = 3;
            var request = new TTTradeHistoryRequest { TimestampTo = DateTime.UtcNow, RequestDirection = TTStreamingDirections.Backward, RequestPageSize = 10 };

            // Try to get trade history from now to the past. Request is limited to 30 records!
            while (iterations-- > 0)
            {
                TTTradeHistoryReport report = client.GetTradeHistory(request);
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

        public static void LimitOrder(TickTraderWebClient client)
        {
            // Create, modify and cancel limit order
            TTAccount account = client.GetAccount();
            if ((account.AccountingType == TTAccountingTypes.Gross) || (account.AccountingType == TTAccountingTypes.Net))
            {
                // Create limit order
                var limit = client.CreateTrade(new TTTradeCreate
                {
                    Type = TTOrderTypes.Limit,
                    Side = TTOrderSides.Buy,
                    Symbol = (account.AccountingType == TTAccountingTypes.Gross) ? "EURUSD" : "EUR/USD",
                    Amount = 10000,
                    Price = 1.0M,
                    Comment = "Buy limit from Web API sample"
                });

                // Modify limit order
                limit = client.ModifyTrade(new TTTradeModify
                {
                    Id = limit.Id,
                    Comment = "Modified limit from Web API sample"
                });

                // Cancel limit order
                client.CancelTrade(limit.Id);
            }
        }

        #endregion
    }
}
