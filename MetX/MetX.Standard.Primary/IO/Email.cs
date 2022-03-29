using System;
using System.Net.Mail;
using System.Threading.Tasks;
using MetX.Standard.Library.Extensions;

namespace MetX.Standard.Primary.IO
{
	/// <summary>
	/// Allows for the sending of a simple email asynchronously on another thread.
	/// </summary>
    // ReSharper disable once UnusedType.Global
    public static class Email
	{
		/// <summary>Allows for the quick and asynchronous sending of a simple email</summary>
        /// <param name="fromName">Display name of the person sending the email</param>
        /// <param name="fromEmail">From Email address</param>
        /// <param name="toName">Display name for the person receiving the email</param>
        /// <param name="toEmail">To Email Address</param>
        /// <param name="subject">Email Subject</param>
        /// <param name="body">Email Body (pure text)</param>
        public static void Send(string fromName, string fromEmail, string toName, string toEmail, string subject, string body)
		{
            var mm = new MailMessage(new MailAddress(fromEmail, fromName), new MailAddress(toEmail, toName))
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = body.IndexOf("<html>", StringComparison.InvariantCultureIgnoreCase) > -1
            };
            Send(mm);
		}

        /// <summary>
        /// Asynchronously sends a MailMessage
        /// </summary>
        /// <param name="mailMessage">The MailMessage to send</param>
        public static Task Send(MailMessage mailMessage)
        {
	        return mailMessage == null 
		        ? null 
		        : ForTasks.FireAndForget(() => new SmtpClient(SmtpImplementation.SmtpServer).Send(mailMessage));
        }
    }
}
