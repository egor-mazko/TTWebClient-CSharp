using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTWebClient.Domain
{
    public enum TTTickType
    {
        Normal,
        IndicativeBid,
        IndicativeAsk,
        IndicativeBidAsk
    }

    public class TTQuoteHistoryTick
    {
        /// <summary>Timestamp</summary>
        public DateTime Timestamp { get; set; }

        public bool IndicativeTick { get; set; }

        /// <summary>Best bid value</summary>
        public TTFeedLevel2Record BestBid { get; set; }

        /// <summary>Best ask value</summary>
        public TTFeedLevel2Record BestAsk { get; set; }

        public TTTickType TickType { get; set; }
    }
}
