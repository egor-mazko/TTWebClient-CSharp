using System;

namespace TTWebClient.Domain
{
    /// <summary>
    /// Order types
    /// </summary>
    public enum TTOrderTypes
    {
        Market = 0,
        Limit = 1,
        Stop = 2,
        Position = 3,
        StopLimit = 4,
    }

    /// <summary>
    /// Order sides
    /// </summary>    
    public enum TTOrderSides
    {
        Buy = 0,
        Sell = 1
    }

    /// <summary>
    /// Order statuses
    /// </summary>    
    public enum TTOrderStatuses
    {
        New = 0,
        Calculated = 1,
        Filled = 3,
        Canceled = 4,
        Rejected = 5,
        Expired = 6,
        PartiallyFilled = 7,
        Activated = 8,
        Executing = 9,
        Invalid = 99
    }

    public enum TTContingentOrderTriggerTypes
    {
        None,
        OnPendingOrderExpired,
        OnPendingOrderPartiallyFilled,
        OnTime
    }

    /// <summary>
    /// Trade information
    /// </summary>
    public class TTTrade
    {
        /// <summary>Trade Id</summary>
        public long Id { get; set; }

        /// <summary>Trade client Id</summary>
        public string ClientId { get; set; }

        /// <summary>Account Id</summary>
        public long AccountId { get; set; }

        /// <summary>Trade type</summary>
        public TTOrderTypes Type { get; set; }

        /// <summary>Trade initial type</summary>
        public TTOrderTypes InitialType { get; set; }

        /// <summary>Trade side</summary>
        public TTOrderSides Side { get; set; }

        /// <summary>Trade status</summary>
        public TTOrderStatuses Status { get; set; }

        /// <summary>Symbol</summary>
        public string Symbol { get; set; }

        /// <summary>Symbol Precision</summary>
        public int? SymbolPrecision { get; set; }

        /// <summary>Price</summary>
        public decimal Price { get; set; }

        /// <summary>Stop price</summary>
        public decimal? StopPrice { get; set; }

        /// <summary>Amount</summary>
        public decimal Amount { get; set; }

        /// <summary>Initial amount</summary>
        public decimal InitialAmount { get; set; }

        /// <summary>Stop loss</summary>
        public decimal? StopLoss { get; set; }

        /// <summary>Take profit</summary>
        public decimal? TakeProfit { get; set; }

        /// <summary>Margin</summary>
        public decimal? Margin { get; set; }

        /// <summary>Profit</summary>
        public decimal? Profit { get; set; }

        /// <summary>Commission</summary>
        public decimal? Commission { get; set; }

        /// <summary>Agent commission</summary>
        public decimal? AgentCommission { get; set; }

        /// <summary>Swap</summary>
        public decimal? Swap { get; set; }

        /// <summary>Immediate-Or-Cancel flag</summary>
        public bool ImmediateOrCancel { get; set; }

        /// <summary>One Cancels The Other flag</summary>
        public bool OneCancelsTheOther { get; set; }

        /// <summary>Created timestamp</summary>
        public DateTime Created { get; set; }

        /// <summary>Expired timestamp</summary>
        public DateTime? Expired { get; set; }

        /// <summary>Modified timestamp</summary>
        public DateTime? Modified { get; set; }

        /// <summary>Filled timestamp</summary>
        public DateTime? Filled { get; set; }

        /// <summary>Position created timestamp</summary>
        public DateTime? PositionCreated { get; set; }

        /// <summary>Comment</summary>
        public string Comment { get; set; }

        /// <summary>Client application (optional)</summary>
        public string ClientApp { get; set; }

        /// <summary>Slippage (optional)</summary>
        public decimal? Slippage { get; set; }

        /// <summary>Commission</summary>
        public decimal? Rebate { get; set; }

        /// <summary>Related Trade Id (may concerns to option OneCancelsTheOther)</summary>
        public long? RelatedTradeId { get; set; }

        /// <summary>Contingent order flag</summary>
        public bool ContingentOrder { get; set; }

        /// <summary>Trigger Type</summary>
        public TTContingentOrderTriggerTypes? TriggerType { get; set; }

        /// <summary>Trigger Time</summary>
        public DateTime? TriggerTime { get; set; }

        /// <summary>Order Id Triggered By</summary>
        public long? OrderIdTriggeredBy { get; set; }

        /// <summary>Close Price</summary>
        public decimal? ClosePrice { get; set; }
    }
}
