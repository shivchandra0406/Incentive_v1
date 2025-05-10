namespace Incentive.Core.Enums
{
    /// <summary>
    /// Status of a deal in the pipeline
    /// </summary>
    public enum DealStatus
    {
        New,
        OnHold,
        Cancelled,
        Won,
        Lost,
        PartiallyPaid,
        FullyPaid
    }
}
