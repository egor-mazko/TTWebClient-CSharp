using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TTWebClient;

namespace TTWebClientUI
{
    internal class LoadClient
    {
        private readonly TickTraderWebClient _client;
        private readonly List<WebClientMethods> _methods;

        public LoadClient(TTWebClient.TickTraderWebClient client, IEnumerable<WebClientMethods> methods)
        {
            _client = client;
            _methods = new List<WebClientMethods>(methods);
        }

        public async Task Start()
        {
        }

        public void Stop()
        {
        }
    }
}
