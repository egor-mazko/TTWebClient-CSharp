using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTWebClient.Domain
{
    public class TTQuoteHistoryTicks
    {
        public List<TTQuoteHistoryTick> Ticks { get; set; }
        public string Symbol { get; set; }
    }
}
