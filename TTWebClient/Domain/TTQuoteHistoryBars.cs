using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTWebClient.Domain
{
    public class TTQuoteHistoryBars
    {
        public string Symbol { get; set; }
        public List<TTQuoteHistoryBar> Bars { get; set; }
    }
}
