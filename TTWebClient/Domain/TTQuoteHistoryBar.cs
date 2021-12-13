using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTWebClient.Domain
{
    public class TTQuoteHistoryBar
    {
        public DateTime Timestamp { get; set; }

        public double Open { get; set; }

        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
    }
}
