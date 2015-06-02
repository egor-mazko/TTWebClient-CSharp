using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using TTWebClient.Domain;
using TTWebClient.Domain.Tools;

namespace TTWebClient
{
    /// <summary>
    /// TickTrader Web Client
    /// </summary>    
    public class TickTraderWebClient
    {
        #region Private fields

        private readonly string _webApiAddress;
        private readonly string _webApiId;
        private readonly string _webApiKey;
        private readonly string _webApiSecret;
        private readonly MediaTypeFormatterCollection _formatters;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct public TickTrader Web API client
        /// </summary>
        /// <remarks>Public Web API client will access only to public methods that don't require authentication!</remarks>
        /// <param name="webApiAddress">Web API address</param>
        public TickTraderWebClient(string webApiAddress)
        {
            if (webApiAddress == null)
                throw new ArgumentNullException("webApiAddress");

            _webApiAddress = webApiAddress;

            _formatters = new MediaTypeFormatterCollection();
            _formatters.Clear();
            _formatters.Add(new JilMediaTypeFormatter());
        }

        /// <summary>
        /// Construct TickTrader Web API client
        /// </summary>
        /// <param name="webApiAddress">Web API address</param>
        /// <param name="webApiId">Web API token Id</param>
        /// <param name="webApiKey">Web API token key</param>
        /// <param name="webApiSecret">Web API token secret</param>
        public TickTraderWebClient(string webApiAddress, string webApiId, string webApiKey, string webApiSecret)
            : this(webApiAddress)
        {
            _webApiId = webApiId;
            _webApiKey = webApiKey;
            _webApiSecret = webApiSecret;
        }

        #endregion

        #region Public Web API Methods

        /// <summary>
        /// Get public trade session information
        /// </summary>
        /// <returns>Public trade session information</returns>
        public async Task<TTTradeSessionStatus> GetPublicTradeSessionStatus()
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync("api/v1/public/tradesession"))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TTTradeSessionStatus>(_formatters);
            }
        }

        /// <summary>
        /// Get list of all available public currencies
        /// </summary>
        /// <returns>List of all available public currencies</returns>
        public async Task<List<TTCurrency>> GetPublicAllCurrencies()
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync("api/v1/public/currency"))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<List<TTCurrency>>(_formatters);
            }
        }

        /// <summary>
        /// Get public currency by name
        /// </summary>
        /// <param name="currency">Currency name</param>
        /// <returns>Public currency with the given name</returns>
        public async Task<TTCurrency> GetPublicCurrency(string currency)
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync(string.Format("api/v1/public/currency/{0}", WebUtility.UrlEncode(WebUtility.UrlEncode(currency)))))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TTCurrency>(_formatters);
            }
        }

        /// <summary>
        /// Get list of all available public symbols
        /// </summary>
        /// <returns>List of all available public symbols</returns>
        public async Task<List<TTSymbol>> GetPublicAllSymbols()
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync("api/v1/public/symbol"))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<List<TTSymbol>>(_formatters);
            }
        }

        /// <summary>
        /// Get public symbol by name
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <returns>Public symbol with the given name</returns>
        public async Task<TTSymbol> GetPublicSymbol(string symbol)
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync(string.Format("api/v1/public/symbol/{0}", WebUtility.UrlEncode(WebUtility.UrlEncode(symbol)))))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TTSymbol>(_formatters);
            }
        }

        /// <summary>
        /// Get list of all available public feed ticks
        /// </summary>
        /// <returns>List of all available public feed ticks</returns>
        public async Task<List<TTFeedTick>> GetPublicAllTicks()
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync("api/v1/public/tick"))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<List<TTFeedTick>>(_formatters);
            }
        }

        /// <summary>
        /// Get public feed tick by symbol name
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <returns>Public feed tick with the given symbol name</returns>
        public async Task<TTFeedTick> GetPublicTick(string symbol)
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync(string.Format("api/v1/public/tick/{0}", WebUtility.UrlEncode(WebUtility.UrlEncode(symbol)))))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TTFeedTick>(_formatters);
            }
        }

        /// <summary>
        /// Get list of all available public feed level2 ticks
        /// </summary>
        /// <returns>List of all available public feed level2 ticks</returns>
        public async Task<List<TTFeedTickLevel2>> GetPublicAllTicksLevel2()
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync("api/v1/public/level2"))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<List<TTFeedTickLevel2>>(_formatters);
            }
        }

        /// <summary>
        /// Get public feed level2 tick by symbol name
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <returns>Public feed level2 tick with the given symbol name</returns>
        public async Task<TTFeedTickLevel2> GetPublicTickLevel2(string symbol)
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync(string.Format("api/v1/public/level2/{0}", WebUtility.UrlEncode(WebUtility.UrlEncode(symbol)))))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TTFeedTickLevel2>(_formatters);
            }
        }

        #endregion

        #region Web API Methods

        /// <summary>
        /// Get list of all available currencies
        /// </summary>
        /// <returns>List of all available currencies</returns>
        public async Task<List<TTCurrency>> GetAllCurrencies()
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync("api/v1/currency"))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<List<TTCurrency>>(_formatters);
            }
        }

        /// <summary>
        /// Get currency by name
        /// </summary>
        /// <param name="currency">Currency name</param>
        /// <returns>Currency with the given name</returns>
        public async Task<TTCurrency> GetCurrency(string currency)
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync(string.Format("api/v1/currency/{0}", WebUtility.UrlEncode(WebUtility.UrlEncode(currency)))))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TTCurrency>(_formatters);
            }
        }

        /// <summary>
        /// Get list of all available symbols
        /// </summary>
        /// <returns>List of all available symbols</returns>
        public async Task<List<TTSymbol>> GetAllSymbols()
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync("api/v1/symbol"))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<List<TTSymbol>>(_formatters);
            }
        }

        /// <summary>
        /// Get symbol by name
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <returns>Symbol with the given name</returns>
        public async Task<TTSymbol> GetSymbol(string symbol)
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync(string.Format("api/v1/symbol/{0}", WebUtility.UrlEncode(WebUtility.UrlEncode(symbol)))))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TTSymbol>(_formatters);
            }
        }

        /// <summary>
        /// Get list of all available feed tick 
        /// </summary>
        /// <returns>List of all available feed tick</returns>
        public async Task<List<TTFeedTick>> GetAllTicks()
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync("api/v1/tick"))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<List<TTFeedTick>>(_formatters);
            }
        }

        /// <summary>
        /// Get feed tick by symbol name
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <returns>Feed tick for the given symbol</returns>
        public async Task<TTFeedTick> GetTick(string symbol)
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync(string.Format("api/v1/tick/{0}", WebUtility.UrlEncode(WebUtility.UrlEncode(symbol)))))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TTFeedTick>(_formatters);
            }
        }

        /// <summary>
        /// Get list of all available feed level2 tick
        /// </summary>
        /// <returns>List of all available feed level2 tick</returns>
        public async Task<List<TTFeedTickLevel2>> GetAllTicksLevel2()
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync("api/v1/level2"))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<List<TTFeedTickLevel2>>(_formatters);
            }
        }

        /// <summary>
        /// Get feed level2 tick by symbol name
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <returns>Feed level2 tick for the given symbol</returns>
        public async Task<TTFeedTickLevel2> GetTickLevel2(string symbol)
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync(string.Format("api/v1/level2/{0}", WebUtility.UrlEncode(WebUtility.UrlEncode(symbol)))))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TTFeedTickLevel2>(_formatters);
            }
        }

        /// <summary>
        /// Get trade session information
        /// </summary>
        /// <returns>Trade session information</returns>
        public async Task<TTTradeSessionStatus> GetTradeSessionStatus()
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync("api/v1/tradesession"))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TTTradeSessionStatus>(_formatters);
            }
        }

        /// <summary>
        /// Get account information
        /// </summary>
        /// <returns>Account information</returns>
        public async Task<TTAccount> GetAccount()
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync("api/v1/account"))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TTAccount>(_formatters);
            }
        }

        /// <summary>
        /// Get list of all cash account assets (currency with amount)
        /// </summary>
        /// <remarks>
        /// **Works only for cash accounts!**
        /// </remarks>>                
        /// <returns>List of all cash account assets</returns>
        public async Task<List<TTAsset>> GetAllAssets()
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync("api/v1/asset"))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<List<TTAsset>>(_formatters);
            }
        }

        /// <summary>
        /// Get cash account asset (currency with amount) by the given currency name
        /// </summary>
        /// <remarks>
        /// **Works only for cash accounts!**
        /// </remarks>>                        
        /// <param name="currency">Currency name</param>
        /// <returns>Cash account asset for the given currency</returns>
        public async Task<TTAsset> GetAsset(string currency)
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync(string.Format("api/v1/asset/{0}", WebUtility.UrlEncode(WebUtility.UrlEncode(currency)))))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TTAsset>(_formatters);
            }
        }

        /// <summary>
        /// Get list of all available positions
        /// </summary>
        /// <remarks>
        /// **Works only for net accounts!**
        /// </remarks>>                
        /// <returns>List of all available positions</returns>
        public async Task<List<TTPosition>> GetAllPositions()
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync("api/v1/position"))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<List<TTPosition>>(_formatters);
            }
        }

        /// <summary>
        /// Get position by symbol
        /// </summary>
        /// <remarks>
        /// **Works only for net accounts!**
        /// </remarks>>                        
        /// <param name="symbol">Symbol name</param>
        /// <returns>Position</returns>
        public async Task<TTPosition> GetPosition(string symbol)
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync(string.Format("api/v1/position/{0}", WebUtility.UrlEncode(WebUtility.UrlEncode(symbol)))))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TTPosition>(_formatters);
            }
        }

        /// <summary>
        /// Get list of all available trades
        /// </summary>
        /// <returns>List of all available trades</returns>
        public async Task<List<TTTrade>> GetAllTrades()
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync("api/v1/trade"))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<List<TTTrade>>(_formatters);
            }
        }

        /// <summary>
        /// Get trade by symbol
        /// </summary>
        /// <param name="tradeId">Trade Id</param>
        /// <returns>Trade</returns>
        public async Task<TTTrade> GetTrade(long tradeId)
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.GetAsync(string.Format("api/v1/trade/{0}", tradeId)))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TTTrade>(_formatters);
            }
        }

        /// <summary>
        /// Create new trade
        /// </summary>
        /// <remarks>
        /// New trade request is described by the filling following fields:
        /// - **ClientId** (optional) - Client trade Id
        /// - **Type** (required) - Type of trade. Possible values: `"Market"`, `"Limit"`, `"Stop"`
        /// - **Side** (required) - Side of trade. Possible values: `"Buy"`, `"Sell"`
        /// - **Symbol** (required) - Trade symbol (e.g. `"EURUSD"`)
        /// - **Price** (optional) - Price of the `"Limit"` / `"Stop"` trades (for `Market` trades price field is ignored)
        /// - **Amount** (required) - Trade amount 
        /// - **StopLoss** (optional) - Stop loss price
        /// - **TakeProfit** (optional) - Take profit price
        /// - **ExpiredTimestamp** (optional) - Expiration date and time for pending trades (`"Limit"`, `"Stop"`)
        /// - **ImmediateOrCancel** (optional) - "Immediate or cancel" flag (works only for `"Limit"` trades)
        /// - **Comment** (optional) - Client comment
        /// </remarks>        
        /// <param name="request">Create trade request</param>
        /// <returns>Created trade</returns>
        public async Task<TTTrade> CreateTrade(TTTradeCreate request)
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.PostAsync("api/v1/trade", request, new JilMediaTypeFormatter()))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TTTrade>(_formatters);
            }
        }

        /// <summary>
        /// Modify existing trade
        /// </summary>
        /// <remarks>
        /// Modify trade request is described by the filling following fields:
        /// - **Id** (required) - Trade Id
        /// - **Price** (optional) - New price of the `Limit` / `Stop` trades (price of `Market` trades cannot be changed)
        /// - **StopLoss** (optional) - Stop loss price
        /// - **TakeProfit** (optional) - Take profit price
        /// - **ExpiredTimestamp** (optional) - Expiration date and time for pending trades (`Limit`, `Stop`)
        /// - **Comment** (optional) - Client comment
        /// </remarks>        
        /// <param name="request">Modify trade request</param>
        /// <returns>Modified trade</returns>
        public async Task<TTTrade> ModifyTrade(TTTradeModify request)
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.PutAsync("api/v1/trade", request, new JilMediaTypeFormatter()))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TTTrade>(_formatters);
            }
        }

        /// <summary>
        /// Cancel existing pending trade
        /// </summary>
        /// <param name="tradeId">Trade Id to cancel</param>
        public async Task CancelTrade(long tradeId)
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.DeleteAsync(string.Format("api/v1/trade?type=Cancel&id={0}", tradeId)))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());
            }
        }

        /// <summary>
        /// Close existing market trade
        /// </summary>
        /// <param name="tradeId">Trade Id to close</param>
        /// <param name="amount">Amount to close (optional)</param>
        public async Task CloseTrade(long tradeId, decimal? amount)
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.DeleteAsync(amount.HasValue ? string.Format("api/v1/trade?type=Close&id={0}&amount={1}", tradeId, amount.Value) : string.Format("api/v1/trade?type=Close&id={0}", tradeId)))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());
            }
        }

        /// <summary>
        /// Close existing market trade by another one
        /// </summary>
        /// <param name="tradeId">Trade Id to close</param>
        /// <param name="byTradeId">By trade Id</param>
        public async Task CloseByTrade(long tradeId, long byTradeId)
        {
            using (var client = CreateHttpClient())
            using (HttpResponseMessage response = await client.DeleteAsync(string.Format("api/v1/trade?type=CloseBy&id={0}&byid={1}", tradeId, byTradeId)))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Force to ignore server SSL/TLS ceritficate
        /// </summary>
        public static void IgnoreServerCertificate()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;            
        }

        #endregion

        #region Private Methods

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient(new RequestContentHMACHandler(_webApiId, _webApiKey, _webApiSecret));
            client.BaseAddress = new Uri(_webApiAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        #endregion

        #region HMAC Client Handler

        public class RequestContentHMACHandler : HttpClientHandler
        {
            private readonly string _webApiId;
            private readonly string _webApiKey;
            private readonly string _webApiSecret;

            public RequestContentHMACHandler(string webApiId, string webApiKey, string webApiSecret)
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                _webApiId = webApiId;
                _webApiKey = webApiKey;
                _webApiSecret = webApiSecret;
            }
            
            protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                var content = (request.Content != null) ? await request.Content.ReadAsStringAsync() : "";
                var signature = timestamp + _webApiId + _webApiKey + request.Method.Method + request.RequestUri + content;
                var hash = CalculateHmacWithSha256(signature);

                request.Headers.Authorization = new AuthenticationHeaderValue("HMAC", string.Format("{0}:{1}:{2}:{3}", _webApiId, _webApiKey, timestamp, hash));
                return await base.SendAsync(request, cancellationToken);
            }

            private string CalculateHmacWithSha256(string signature)
            {
                var encoding = new System.Text.ASCIIEncoding();
                byte[] keyByte = encoding.GetBytes(_webApiSecret);
                byte[] messageBytes = encoding.GetBytes(signature);
                using (var hmacsha256 = new HMACSHA256(keyByte))
                {
                    byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                    return Convert.ToBase64String(hashmessage);
                }
            }
        }

        #endregion
    }
}
