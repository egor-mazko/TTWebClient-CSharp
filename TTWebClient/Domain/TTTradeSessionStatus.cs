using System;

namespace TTWebClient.Domain
{
    /// <summary>
    /// Trading session status
    /// </summary>
    public enum TTTradingSessionStatus
    {
        /// <summary>Trading session is closed</summary>
        Closed,
        /// <summary>Trading session is opened</summary>
        Opened
    }

    /// <summary>
    /// Trading session status
    /// </summary>
    public class TTTradeSessionStatus
    {
        /// <summary>Tick Trader Server name</summary>
        public string PlatformName { get; set; }

        /// <summary>Tick Trader Server owner company</summary>
        public string PlatformCompany { get; set; }

        /// <summary>Tick Trader Server timezone offset in hours from UTC</summary>
        public int PlatformTimezoneOffset { get; set; }

        /// <summary>Trading session Id</summary>
        /// <remarks>GUID for Trading Session (empty GUID for closed session)</remarks>
        public string SessionId { get; set; }

        /// <summary>Trading session status</summary>
        /// <remarks>State of the trading session. Possible values: Closed, Opened</remarks>
        public TTTradingSessionStatus SessionStatus { get; set; }

        /// <summary>Trading session start time</summary>
        /// <remarks>Start time of the current trading session (the same meaning for opened and closed sessions)</remarks>
        public DateTime SessionStartTime { get; set; }

        /// <summary>Trading session end time</summary>
        /// <remarks>End time of the current trading session (the same meaning for opened and closed sessions)</remarks>
        public DateTime SessionEndTime { get; set; }

        /// <summary>Trading session open time</summary>
        /// <remarks>
        /// Provides the open time of the current trading session in case of current session is opened. 
        /// Provides the open time of the next open trading session in case of current session is closed.
        /// </remarks>
        public DateTime SessionOpenTime { get; set; }

        /// <summary>Trading session close time</summary>
        /// <remarks>
        /// Provides the close time of the current trading session in case of current session is opened. 
        /// Provides the close time of the next open trading session in case of current session is closed.
        /// </remarks> 
        public DateTime SessionCloseTime { get; set; }
    }
}
