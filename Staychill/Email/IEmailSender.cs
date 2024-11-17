namespace Staychill.Email
{
    // Interface of services to send email to customer // 
    public interface IEmailSender
    {
        // Set as Task to make it asynchronous (not blocking another working-method in web-app)  //
        Task SendEmailAsync(string email, string subject, string message); // No method yet but can use at another time later //
        Task SendEmailWithAttachmentAsync(string email, string subject, string message, byte[] attachmentData, string attachmentFileName);
    }
}
