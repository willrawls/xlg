using System;
using System.Threading;
using System.Collections;
using System.Net.Mail;

namespace MetX.IO
{
	/// <summary>
	/// Allows for the sending of a simple emamil asynchronously on another thread.
	/// </summary>
	public class Email
	{
		/// <summary>The email message to use with Send()</summary>
		//public MailMessage mm;
        //Thread t;

        /// <summary>Allows for the quick and asynchronous sending of a simple email</summary>
        /// <param name="FromName">Display name of the person sending the email</param>
        /// <param name="FromEmail">From Email address</param>
        /// <param name="ToName">Display name for the person receiving the email</param>
        /// <param name="ToEmail">To Email Address</param>
        /// <param name="Subject">Email Subject</param>
        /// <param name="Body">Email Body (pure text)</param>
        public static void SendMail(string FromName, string FromEmail, string ToName, string ToEmail, string Subject, string Body)
		{
            // Join();
            MailMessage mm = new MailMessage(new MailAddress(FromEmail, FromName), new MailAddress(ToEmail, ToName));
			mm.Subject = Subject;
			mm.Body = Body;
            mm.IsBodyHtml = (Body.IndexOf("<HTML>") > -1 || Body.IndexOf("<html>") > -1);
            Send(mm);
		}

        /// <summary>
        /// Asynchronously sends a MailMessage
        /// </summary>
        /// <param name="mm">The MailMessage to send</param>
        public static void Send(MailMessage mm)
        {
            Email m = new Email();
            ThreadPool.QueueUserWorkItem(new WaitCallback(Start), mm);
        }
        /// <summary>Private function for sending the asychronous email on a new thread</summary>
        private static void Start(object objMailMessage)
        {
            try
            {
                new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SmtpServer"]).Send((MailMessage)objMailMessage);
            }
            catch { }
        }

    }
}
