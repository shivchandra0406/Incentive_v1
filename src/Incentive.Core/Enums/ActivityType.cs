namespace Incentive.Core.Enums
{
    /// <summary>
    /// Types of activities that can be performed on a deal
    /// </summary>
    public enum ActivityType
    {
        Created,
        Updated,
        StatusChanged,
        PaymentReceived,
        NoteAdded,
        EmailSent,
        CallMade,
        MeetingScheduled,
        DocumentSent,
        DocumentSigned,
        Delivered,
        Cancelled,
        Other
    }
}
