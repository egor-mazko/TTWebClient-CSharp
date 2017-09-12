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
    using PropertyValueCollection = ObservableCollection<KeyValuePair<string, object>>;

    internal static class ViewExtensions
    {
        public static string ToShortString(this TTFeedLevel2Record feedL2Rec)
        {
            return $"{feedL2Rec.Price} / {feedL2Rec.Volume}";
        }
        public static string ToLongString(this TTFeedLevel2Record feedL2Rec)
        {
            return $"{feedL2Rec.Type} {feedL2Rec.Price} / {feedL2Rec.Volume}";
        }
    }

    internal class FeedTickView : ObservableObject
    {
        public string Symbol { get; set; }
        public string Timestamp { get; set; }
        public string BestBid { get; set; }
        public string BestAsk { get; set; }

        public FeedTickView(TTFeedTick tick)
        {
            Symbol = tick.Symbol;
            Timestamp = tick.Timestamp.ToString("u");
            BestBid = tick.BestBid.ToShortString();
            BestAsk = tick.BestAsk.ToShortString();
        }
    }

    internal class FeedTickL2View
    {
        public string Symbol { get; set; }
        public string Timestamp { get; set; }
        public string BestBid { get; set; }
        public string BestAsk { get; set; }
        public string Bids { get; set; }
        public string Asks { get; set; }

        public FeedTickL2View(TTFeedTickLevel2 tick)
        {
            Symbol = tick.Symbol;
            Timestamp = tick.Timestamp.ToString("u");
            BestBid = tick.BestBid.ToShortString();
            BestAsk = tick.BestAsk.ToShortString();
            Bids = string.Join(" ", tick.Bids.Select(b => b.ToShortString()));
            Asks = string.Join(" ", tick.Asks.Select(a => a.ToShortString()));
        }
    }

    internal class ClientView : ObservableObject
    {
        private readonly TickTraderWebClient _client;

        private bool _isPublicEnabled;
        private bool _isPrivateEnabled;

        // public fields
        private PropertyValueCollection _publicTradeSession = new PropertyValueCollection();
        private ObservableCollection<TTCurrency> _publicCurrencies;
        private string _publicCurrenciesFilter;
        private ObservableCollection<TTSymbol> _publicSymbols;
        private string _publicSymbolsFilter;
        private ObservableCollection<FeedTickView> _publicTicks;
        private string _publicTicksFilter;
        private ObservableCollection<FeedTickL2View> _publicTicksL2;
        private string _publicTicksL2Filter;
        private ObservableCollection<TTTicker> _publicTickers;
        private string _publicTickersFilter;

        //private fields
        private PropertyValueCollection _accountInfo = new PropertyValueCollection();
        private bool _isAccountNet;
        private bool _isAccountCash;
        private PropertyValueCollection _tradeSession = new PropertyValueCollection();
        private ObservableCollection<TTCurrency> _currencies;
        private string _currenciesFilter;
        private ObservableCollection<TTSymbol> _symbols;
        private string _symbolsFilter;
        private ObservableCollection<FeedTickView> _ticks;
        private string _ticksFilter;
        private ObservableCollection<FeedTickL2View> _ticksL2;
        private string _ticksL2Filter;
        private ObservableCollection<TTAsset> _assets;
        private string _assetCurrency;
        private ObservableCollection<TTPosition> _positions;
        private string _positionId;
        private ObservableCollection<TTTrade> _trades;
        private string _tradeId;

        public Command PublicTradeSessionCommand { get; private set; }
        public Command PublicCurrenciesCommand { get; private set; }
        public Command PublicSymbolsCommand { get; private set; }
        public Command PublicTicksCommand { get; private set; }
        public Command PublicTicksL2Command { get; private set; }
        public Command PublicTickersCommand { get; private set; }

        public Command AccountInfoCommand { get; set; }
        public Command TradeSessionCommand { get; set; }
        public Command CurrenciesCommand { get; private set; }
        public Command SymbolsCommand { get; private set; }
        public Command TicksCommand { get; private set; }
        public Command TicksL2Command { get; private set; }
        public Command AssetsCommand { get; set; }
        public Command PositionsCommand { get; set; }
        public Command TradesCommand { get; set; }

        public bool IsPublicEnabled
        {
            get { return _isPublicEnabled; }
            set
            {
                _isPublicEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsPrivateEnabled
        {
            get { return _isPrivateEnabled; }
            set
            {
                _isPrivateEnabled = value;
                OnPropertyChanged();
            }
        }

        #region Public properties

        public PropertyValueCollection PublicTradeSession
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

        public ObservableCollection<FeedTickL2View> PublicTicksL2
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

        public ObservableCollection<TTTicker> PublicTickers
        {
            get { return _publicTickers; }
            set
            {
                _publicTickers = value;
                OnPropertyChanged();
            }
        }

        public string PublicTickersFilter
        {
            get { return _publicTickersFilter; }
            set
            {
                _publicTickersFilter = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Private properties

        public PropertyValueCollection AccountInfo
        {
            get { return _accountInfo; }
            set
            {
                _accountInfo = value;
                OnPropertyChanged();
            }
        }

        public bool IsAccountNet
        {
            get { return _isAccountNet; }
            set
            {
                _isAccountNet = value;
                OnPropertyChanged();
            }
        }

        public bool IsAccountCash
        {
            get { return _isAccountCash; }
            set
            {
                _isAccountCash = value;
                OnPropertyChanged();
            }
        }

        public PropertyValueCollection TradeSession
        {
            get { return _tradeSession; }
            set
            {
                _tradeSession = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TTCurrency> Currencies
        {
            get { return _currencies; }
            set
            {
                _currencies = value;
                OnPropertyChanged();
            }
        }

        public string CurrenciesFilter
        {
            get { return _currenciesFilter; }
            set
            {
                _currenciesFilter = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TTSymbol> Symbols
        {
            get { return _symbols; }
            set
            {
                _symbols = value;
                OnPropertyChanged();
            }
        }

        public string SymbolsFilter
        {
            get { return _symbolsFilter; }
            set
            {
                _symbolsFilter = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FeedTickView> Ticks
        {
            get { return _ticks; }
            set
            {
                _ticks = value;
                OnPropertyChanged();
            }
        }

        public string TicksFilter
        {
            get { return _ticksFilter; }
            set
            {
                _ticksFilter = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FeedTickL2View> TicksL2
        {
            get { return _ticksL2; }
            set
            {
                _ticksL2 = value;
                OnPropertyChanged();
            }
        }

        public string TicksL2Filter
        {
            get { return _ticksL2Filter; }
            set
            {
                _ticksL2Filter = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TTAsset> Assets
        {
            get { return _assets; }
            set
            {
                _assets = value;
                OnPropertyChanged();
            }
        }

        public string AssetCurrency
        {
            get { return _assetCurrency; }
            set
            {
                _assetCurrency = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TTPosition> Positions
        {
            get { return _positions; }
            set
            {
                _positions = value;
                OnPropertyChanged();
            }
        }

        public string PositionId
        {
            get { return _positionId; }
            set
            {
                _positionId = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TTTrade> Trades
        {
            get { return _trades; }
            set
            {
                _trades = value;
                OnPropertyChanged();
            }
        }

        public string TradeId
        {
            get { return _tradeId; }
            set
            {
                _tradeId = value;
                OnPropertyChanged();
            }
        }

        #endregion

        static ClientView()
        {
            // Optional: Force to ignore server certificate 
            TickTraderWebClient.IgnoreServerCertificate();
        }

        public ClientView(CredsModel creds)
        {
            PublicTradeSessionCommand = new Command(async () => await GetPublicTradeSession());
            PublicCurrenciesCommand = new Command(async () => await GetPublicCurrencies());
            PublicSymbolsCommand = new Command(async () => await GetPublicSymbols());
            PublicTicksCommand = new Command(async () => await GetPublicTicks());
            PublicTicksL2Command = new Command(async () => await GetPublicTicksLevel2());
            PublicTickersCommand = new Command(async () => await GetPublicTickers());

            AccountInfoCommand = new Command(async () => await GetAccount());
            TradeSessionCommand = new Command(async () => await GetTradeSession());
            CurrenciesCommand = new Command(async () => await GetCurrencies());
            SymbolsCommand = new Command(async () => await GetSymbols());
            TicksCommand = new Command(async () => await GetTicks());
            TicksL2Command = new Command(async () => await GetTicksLevel2());
            AssetsCommand = new Command(async () => await GetAssets());
            PositionsCommand = new Command(async () => await GetPositions());
            TradesCommand = new Command(async () => await GetTrades());

            if (creds.IsPublicOnly)
            {
                _client = new TickTraderWebClient(creds.WebApiAddress);
                IsPublicEnabled = true;
                IsPrivateEnabled = false;
            }
            else
            {
                _client = new TickTraderWebClient(creds.WebApiAddress, creds.WebApiId, creds.WebApiKey, creds.WebApiSecret);
                IsPublicEnabled = true;
                IsPrivateEnabled = true;
                AccountInfoCommand.Execute(null);
            }
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
            PublicTradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.SessionStartTime), tradeSession.SessionStartTime.ToString("u")));
            PublicTradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.SessionEndTime), tradeSession.SessionEndTime.ToString("u")));
            PublicTradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.SessionOpenTime), tradeSession.SessionOpenTime.ToString("u")));
            PublicTradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.SessionCloseTime), tradeSession.SessionCloseTime.ToString("u")));
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

            PublicTicksL2 = publicTicksLevel2 != null ? new ObservableCollection<FeedTickL2View>(publicTicksLevel2.Select(t => new FeedTickL2View(t))) : null;
        }

        public async Task GetPublicTickers()
        {
            // Public symbol statistics
            List<TTTicker> publicTickers;

            if (string.IsNullOrEmpty(PublicTickersFilter))
                publicTickers = await _client.GetPublicAllTickersAsync();
            else
                publicTickers = await _client.GetPublicTickerAsync(PublicTickersFilter);

            PublicTickers = publicTickers != null ? new ObservableCollection<TTTicker>(publicTickers) : null;
        }

        #endregion

        #region Account information

        public async Task GetAccount()
        {
            AccountInfo.Clear();
            // Account info
            TTAccount account = await _client.GetAccountAsync();

            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.Id), account.Id));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.Group), account.Group));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.AccountingType), account.AccountingType));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.Name), account.Name));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.Email), account.Email));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.Comment), account.Comment));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.Registered), account.Registered.ToString("u")));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.IsBlocked), account.IsBlocked));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.IsReadonly), account.IsReadonly));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.IsValid), account.IsValid));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.IsWebApiEnabled), account.IsWebApiEnabled));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.Leverage), account.Leverage));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.Balance), account.Balance));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.BalanceCurrency), account.BalanceCurrency));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.Equity), account.Equity));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.Margin), account.Margin));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.MarginLevel), account.MarginLevel));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.MarginCallLevel), account.MarginCallLevel));
            AccountInfo.Add(new KeyValuePair<string, object>(nameof(account.StopOutLevel), account.StopOutLevel));

            IsAccountCash = account.AccountingType == TTAccountingTypes.Cash;
            IsAccountNet = account.AccountingType == TTAccountingTypes.Net;
        }

        #endregion

        #region Account trade session information

        public async Task GetTradeSession()
        {
            TradeSession.Clear();

            // Account trade session
            TTTradeSession tradeSession = await _client.GetTradeSessionAsync();

            TradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.PlatformName), tradeSession.PlatformName));
            TradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.PlatformCompany), tradeSession.PlatformCompany));
            TradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.PlatformAddress), tradeSession.PlatformAddress));
            TradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.PlatformTimezoneOffset), tradeSession.PlatformTimezoneOffset));
            TradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.SessionId), tradeSession.SessionId));
            TradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.SessionStatus), tradeSession.SessionStatus));
            TradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.SessionStartTime), tradeSession.SessionStartTime.ToString("u")));
            TradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.SessionEndTime), tradeSession.SessionEndTime.ToString("u")));
            TradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.SessionOpenTime), tradeSession.SessionOpenTime.ToString("u")));
            TradeSession.Add(new KeyValuePair<string, object>(nameof(tradeSession.SessionCloseTime), tradeSession.SessionCloseTime.ToString("u")));
        }

        #endregion

        #region Account currencies, symbols, feed ticks, feed ticks level2 information

        public async Task GetCurrencies()
        {
            // Account currencies
            List<TTCurrency> currencies;

            if (string.IsNullOrEmpty(CurrenciesFilter))
                currencies = await _client.GetAllCurrenciesAsync();
            else
                currencies = await _client.GetCurrencyAsync(CurrenciesFilter);

            Currencies = currencies != null ? new ObservableCollection<TTCurrency>(currencies) : null;
        }

        public async Task GetSymbols()
        {
            // Account symbols
            List<TTSymbol> symbols;

            if (string.IsNullOrEmpty(SymbolsFilter))
                symbols = await _client.GetAllSymbolsAsync();
            else
                symbols = await _client.GetSymbolAsync(SymbolsFilter);

            Symbols = symbols != null ? new ObservableCollection<TTSymbol>(symbols) : null;
        }

        public async Task GetTicks()
        {
            // Account feed ticks
            List<TTFeedTick> ticks;

            if (string.IsNullOrEmpty(TicksFilter))
                ticks = await _client.GetAllTicksAsync();
            else
                ticks = await _client.GetTickAsync(TicksFilter);

            Ticks = ticks != null ? new ObservableCollection<FeedTickView>(ticks.Select(t => new FeedTickView(t))) : null;
        }

        public async Task GetTicksLevel2()
        {
            // Account feed ticks level2 
            List<TTFeedTickLevel2> ticksLevel2;

            if (string.IsNullOrEmpty(TicksL2Filter))
                ticksLevel2 = await _client.GetAllTicksLevel2Async();
            else
                ticksLevel2 = await _client.GetTickLevel2Async(TicksL2Filter);

            TicksL2 = ticksLevel2 != null ? new ObservableCollection<FeedTickL2View>(ticksLevel2.Select(t => new FeedTickL2View(t))) : null;
        }

        #endregion

        #region Account assets, positions, trades information

        public async Task GetAssets()
        {
            // Account assets
            TTAccount account = await _client.GetAccountAsync();
            List<TTAsset> assets = null;
            if (account.AccountingType == TTAccountingTypes.Cash)
            {
                if (string.IsNullOrEmpty(AssetCurrency))
                    assets = await _client.GetAllAssetsAsync();
                else
                {
                    try
                    {
                        TTAsset asset = await _client.GetAssetAsync(AssetCurrency);
                        assets = new List<TTAsset>(new[] { asset });
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            Assets = assets != null ? new ObservableCollection<TTAsset>(assets) : null;
        }

        public async Task GetPositions()
        {
            // Account positions
            TTAccount account = await _client.GetAccountAsync();
            List<TTPosition> positions = null;
            if (account.AccountingType == TTAccountingTypes.Net)
            {
                if (string.IsNullOrEmpty(PositionId))
                    positions = await _client.GetAllPositionsAsync();
                else
                {
                    try
                    {
                        TTPosition position = await _client.GetPositionAsync(PositionId);
                        positions = new List<TTPosition>(new[] { position });
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            Positions = positions != null ? new ObservableCollection<TTPosition>(positions) : null;
        }

        public async Task GetTrades()
        {
            // Account trades
            List<TTTrade> trades = null;
            if (string.IsNullOrEmpty(TradeId))
                trades = await _client.GetAllTradesAsync();
            else
            {
                long tradeId;
                if (long.TryParse(TradeId, out tradeId))
                {
                    try
                    {
                        TTTrade trade = await _client.GetTradeAsync(tradeId);
                        trades = new List<TTTrade>(new[] { trade });
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            Trades = trades != null ? new ObservableCollection<TTTrade>(trades) : null;
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
