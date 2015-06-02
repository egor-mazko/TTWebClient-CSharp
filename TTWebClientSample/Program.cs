using System;
using System.Collections.Generic;
using TTWebClient;
using TTWebClient.Domain;

namespace TTWebClientSample
{
    class Program
    {
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

            // Public trade session status
            TTTradeSessionStatus publictradesession = client.GetPublicTradeSessionStatus().Result;
            Console.WriteLine("TickTrader name: {0}", publictradesession.PlatformName);
            Console.WriteLine("TickTrader company: {0}", publictradesession.PlatformCompany);
            Console.WriteLine("TickTrader timezone offset: {0}", publictradesession.PlatformTimezoneOffset);
            Console.WriteLine("TickTrader session status: {0}", publictradesession.SessionStatus);

            // Public currencies
            List<TTCurrency> publicCurrencies = client.GetPublicAllCurrencies().Result;
            foreach (var c in publicCurrencies)
                Console.WriteLine("Currency: " + c.Name);

            TTCurrency publicCurrency = client.GetPublicCurrency(publicCurrencies[0].Name).Result;
            Console.WriteLine("{0} currency precision: {1}", publicCurrency.Name, publicCurrency.Precision);

            // Public symbols
            List<TTSymbol> publicSymbols = client.GetPublicAllSymbols().Result;
            foreach (var s in publicSymbols)
                Console.WriteLine("Symbol: " + s.Symbol);

            TTSymbol publicSymbol = client.GetPublicSymbol(publicSymbols[0].Symbol).Result;
            Console.WriteLine("{0} symbol precision: {1}", publicSymbol.Symbol, publicSymbol.Precision);

            // Public feed ticks
            List<TTFeedTick> publicTicks = client.GetPublicAllTicks().Result;
            foreach (var t in publicTicks)
                Console.WriteLine("{0} tick: {1}, {2}", t.Symbol, t.BestBid.Price, t.BestAsk.Price);

            TTFeedTick publicTick = client.GetPublicTick(publicTicks[0].Symbol).Result;
            Console.WriteLine("{0} tick timestamp: {1}", publicTick.Symbol, publicTick.Timestamp);

            // Public feed level2 ticks
            List<TTFeedTickLevel2> publicTicksLevel2 = client.GetPublicAllTicksLevel2().Result;
            foreach (var t in publicTicksLevel2)
                Console.WriteLine("{0} level2 book depth: {1}", t.Symbol, Math.Max(t.Bids.Count, t.Asks.Count));

            TTFeedTickLevel2 publicTickLevel2 = client.GetPublicTickLevel2(publicTicksLevel2[0].Symbol).Result;
            Console.WriteLine("{0} level 2 book depth: {1}", publicTickLevel2.Symbol, Math.Max(publicTickLevel2.Bids.Count, publicTickLevel2.Asks.Count));

            Console.WriteLine("--- Web API client methods ---");

            // Currencies
            List<TTCurrency> currencies = client.GetAllCurrencies().Result;
            foreach (var c in currencies)
                Console.WriteLine("Currency: " + c.Name);

            TTCurrency currency = client.GetCurrency(currencies[0].Name).Result;
            Console.WriteLine("{0} currency precision: {1}", currency.Name, currency.Precision);

            // Symbols
            List<TTSymbol> symbols = client.GetAllSymbols().Result;
            foreach (var s in symbols)
                Console.WriteLine("Symbol: " + s.Symbol);

            TTSymbol symbol = client.GetSymbol(symbols[0].Symbol).Result;
            Console.WriteLine("{0} symbol precision: {1}", symbol.Symbol, symbol.Precision);

            // Feed ticks
            List<TTFeedTick> ticks = client.GetAllTicks().Result;
            foreach (var t in ticks)
                Console.WriteLine("{0} tick: {1}, {2}", t.Symbol, t.BestBid.Price, t.BestAsk.Price);

            TTFeedTick tick = client.GetTick(ticks[0].Symbol).Result;
            Console.WriteLine("{0} tick timestamp: {1}", tick.Symbol, tick.Timestamp);

            // Feed level2 ticks
            List<TTFeedTickLevel2> ticksLevel2 = client.GetAllTicksLevel2().Result;
            foreach (var t in ticksLevel2)
                Console.WriteLine("{0} level2 book depth: {1}", t.Symbol, Math.Max(t.Bids.Count, t.Asks.Count));

            TTFeedTickLevel2 tickLevel2 = client.GetTickLevel2(ticksLevel2[0].Symbol).Result;
            Console.WriteLine("{0} level 2 book depth: {1}", tickLevel2.Symbol, Math.Max(tickLevel2.Bids.Count, tickLevel2.Asks.Count));

            // Trade session status
            TTTradeSessionStatus tradesession = client.GetTradeSessionStatus().Result;
            Console.WriteLine("Trade session status: {0}", tradesession.SessionStatus);

            // Account info
            TTAccount account = client.GetAccount().Result;
            Console.WriteLine("Account Id: {0}", account.Id);
            Console.WriteLine("Account name: {0}", account.Name);
            Console.WriteLine("Account group: {0}", account.Group);

            // Account assets
            if (account.AccountingType == TTAccountingTypes.Cash)
            {
                List<TTAsset> assets = client.GetAllAssets().Result;
                foreach (var a in assets)
                    Console.WriteLine("{0} asset: {1}", a.Currency, a.Amount);                
            }

            // Account positions
            if (account.AccountingType == TTAccountingTypes.Net)
            {
                List<TTPosition> positions = client.GetAllPositions().Result;
                foreach (var p in positions)
                    Console.WriteLine("{0} position: {1} {2}", p.Symbol, p.LongAmount, p.ShortAmount);
            }

            // Account trades
            List<TTTrade> trades = client.GetAllTrades().Result;
            foreach (var t in trades)
                Console.WriteLine("{0} trade with type {1} by symbol {2}: {3}", t.Id, t.Type, t.Symbol, t.Amount);

            TTTrade trade = client.GetTrade(trades[0].Id).Result;
            Console.WriteLine("{0} trade with type {1} by symbol {2}: {3}", trade.Id, trade.Type, trade.Symbol, trade.Amount);

            // Create limit order
            if ((account.AccountingType == TTAccountingTypes.Gross) || (account.AccountingType == TTAccountingTypes.Net))
            {
                var limit = client.CreateTrade(new TTTradeCreate
                {
                    Type = TTOrderTypes.Limit, 
                    Side = TTOrderSides.Buy,
                    Symbol = (account.AccountingType == TTAccountingTypes.Gross) ? "EURUSD" : "EUR/USD", 
                    Amount = 10000, 
                    Price = 1.0M,
                    Comment = "Buy limit from Web API sample"
                }).Result;

                limit = client.ModifyTrade(new TTTradeModify
                {
                    Id = limit.Id,
                    Comment = "Modified limit from Web API sample"
                }).Result;

                client.CancelTrade(limit.Id).Wait();
            }
        }
    }
}
