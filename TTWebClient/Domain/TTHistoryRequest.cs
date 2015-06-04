using System;

namespace TTWebClient.Domain
{
    /// <summary>
    /// Streaming directions
    /// </summary>
    public enum TTStreamingDirections
    {
        Forward,
        Backward
    }    
    
    /// <summary>
    /// Trade history request
    /// </summary>
    public class TTTradeHistoryRequest
    {
        /// <summary>Lower timestamp bound of the trade history request (optional)</summary>
        public DateTime? TimestampFrom { get; set; }

        /// <summary>Upper timestamp bound of the trade history request (optional)</summary>
        public DateTime? TimestampTo { get; set; }

        /// <summary>Request paging direction ("Forward" or "Backward"). Default is "Forward" (optional)</summary>
        public TTStreamingDirections? RequestDirection { get; set; }

        /// <summary>Request paging last Id (optional)</summary>
        public string RequestLastId { get; set; }
    }
}
