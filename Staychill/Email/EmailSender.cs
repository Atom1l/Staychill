using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Staychill.Email
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "staychillshop123@gmail.com"; // Your email
            var password = "nxjp aomm tpxi dnva"; // Use App Password here if necessary (not the password of the Gmail)

            var client = new SmtpClient("smtp.gmail.com", 587) // Create an instance to connect the SMTP server at port 587
            {
                EnableSsl = true, // SSL = Secure Socket Layers | make sending data more secure
                Credentials = new NetworkCredential(mail, password) // Credentials by mail and password
            };

            var mailMessage = new MailMessage // Create an object to use to send the email
            {
                From = new MailAddress(mail), // From Mail that we've set
                Subject = subject, // Subject of the mail
                Body = message, // Message
                IsBodyHtml = true // Set to true if your message contains HTML
            };
            mailMessage.To.Add(email); // Set the email as a parameter that contains the user's mail to send this email to

            try
            {
                await client.SendMailAsync(mailMessage); // Use a mailMessage object to transfer data into SendMailAsync Task to work as Asynchronous
            }
            catch (SmtpException smtpEx) // Prevent Error
            {
                throw new Exception("SMTP error: " + smtpEx.Message);
            }
            catch (Exception ex) // Prevent Error
            {
                throw new Exception("General error: " + ex.Message);
            }
        }

        // New method to send email with an attachment (PDF)
        public async Task SendEmailWithAttachmentAsync(string email, string subject, string message, byte[] attachment, string attachmentName)
        {
            var mail = "staychillshop123@gmail.com"; // Your email
            var password = "nxjp aomm tpxi dnva"; // Use App Password here if necessary (not the password of the Gmail)

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mail),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            // Attach the PDF if the attachment is provided
            if (attachment != null)
            {
                var stream = new MemoryStream(attachment);
                var pdfAttachment = new Attachment(stream, attachmentName, "application/pdf");
                mailMessage.Attachments.Add(pdfAttachment);
            }

            try
            {
                await client.SendMailAsync(mailMessage);
            }
            catch (SmtpException smtpEx)
            {
                throw new Exception("SMTP error: " + smtpEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("General error: " + ex.Message);
            }
        }
    }

}
