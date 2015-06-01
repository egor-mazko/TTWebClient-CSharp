# TTWebClient-CSharp
C# Web API client for TickTrader

## Creating Web API client
```c#
string webApiAddress = "tpdemo.fxopen.com";
string webApiId = "8bd43d1f-39a4-45cd-a876-6acc0586533d";
string webApiKey = "qXhpBKFkndWWGYQ2";
string webApiSecret = "dSccqQmtaPc2xB68GD6A7KBgpfRhHJkFe5AchGShbDGzyn8H8ThjPspCq6Yh8cTz";

// Optional: Force to ignore server certificate
TickTraderWebClient.IgnoreServerCeritficate();

// Create instance of the TickTrader Web API client
var client = new TickTraderWebClient(webApiAddress, webApiId, webApiKey, webApiSecret);
```

## Access to public Tick Trader information
```c#
// Public currency
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
```

## Access to currency, symbol, feed ticks information
```c#
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
```
