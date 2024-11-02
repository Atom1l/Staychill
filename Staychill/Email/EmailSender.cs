using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Staychill.Email
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message) // Use the Task with parameter that we created //
        {
            var mail = "staychillshop123@gmail.com"; // Your email //
            var password = "nxjp aomm tpxi dnva"; // Use App Password here if necessary (not the password of the gmail) //

            var client = new SmtpClient("smtp.gmail.com", 587) // Create an instance to connect the smtp server at port 587 //
            {
                EnableSsl = true, // SSL = Secure Socket Layers | make sending data more secure //
                Credentials = new NetworkCredential(mail, password) // Credentials by mail and password //
            };

            var mailMessage = new MailMessage // Create an object to use to send email //
            {
                From = new MailAddress(mail), // From Mail that we've set //
                Subject = subject, // Subject of the mail //
                Body = message, // message //
                IsBodyHtml = true // Set to true if your message contains HTML //
            };
            mailMessage.To.Add(email); // Set the email as a parameter that contain the user mail to sent this email to //

            try
            {
                await client.SendMailAsync(mailMessage); // Use a mailMessage object to transfer data into SendMailAsync Task to work as Asynchronous //
            }
            catch (SmtpException smtpEx) // Prevent Error //
            {
                throw new Exception("SMTP error: " + smtpEx.Message);
            }
            catch (Exception ex) // Prevent Error //
            {
                throw new Exception("General error: " + ex.Message);
            }
        }
    }
}
