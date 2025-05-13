namespace Incentive.Application.Common.Interfaces
{
    /// <summary>
    /// Email service interface
    /// </summary>
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
        Task SendEmailAsync(string[] to, string subject, string body, bool isHtml = false);
        Task SendEmailAsync(string to, string subject, string body, string[] attachments, bool isHtml = false);
        Task SendEmailAsync(string[] to, string subject, string body, string[] attachments, bool isHtml = false);
    }
}
