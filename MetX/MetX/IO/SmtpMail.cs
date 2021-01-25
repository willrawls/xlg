using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading;

// ReSharper disable UnusedMember.Global

namespace MetX.IO
{
    /// <summary>
    ///     Provides methids to send email via smtp, with out CDO SYS installed.
    ///     <para>This is a manual implementation of the SMTP protocol.</para>
    /// </summary>
    public static class SmtpMail
    {
        public enum SmtpResponses
        {
            ConnectSuccess = 220,
            GenericSuccess = 250,
            DataSuccess = 354,
            QuitSuccess = 221
        }

        /// <summary>Get or Set the name of the SMTP relay mail server</summary>
        public static string SmtpServer;

        /// <summary>Send an Email</summary>
        /// <param name="fromName">The displayed name for the FROM address</param>
        /// <param name="fromEmail">The FROM address</param>
        /// <param name="toName">The displayed name for the TO address</param>
        /// <param name="toEmail">The TO address</param>
        /// <param name="subject">The SUBJECT for the email</param>
        /// <param name="body">The text body of the email</param>
        /// <returns>True if the email was sent</returns>
        public static bool Send(string fromName, string fromEmail, string toName, string toEmail, string subject,
            string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = body,
            };
            mailMessage.To.Add(new MailAddress(toEmail, toName));
            
            SmtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            var messageSentSuccessfully = Send(mailMessage);
            return messageSentSuccessfully;
        }


        /// <summary>Sends an email message</summary>
        /// <param name="message">The MailMessage to send</param>
        /// <returns>Returns true if the email was sent.</returns>
        public static bool Send(MailMessage message)
        {
            var ipHostEntry = Dns.GetHostEntry(SmtpServer);
            var target = new IPEndPoint(ipHostEntry.AddressList[0], 25);
            var s = new Socket(target.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(target);

            #region connect

            if (!Check_Response(s, SmtpResponses.ConnectSuccess))
            {
                Console.WriteLine("Server didn't respond.");
                s.Close();
                return false;
            }

            #endregion

            #region send helo

            Senddata(s, string.Format("HELO {0}\r\n", Dns.GetHostName()));
            if (!Check_Response(s, SmtpResponses.GenericSuccess))
            {
                Console.WriteLine("Helo Failed!.");
                s.Close();
                return false;
            }

            #endregion

            #region Send the MAIL command

            Senddata(s, string.Format("MAIL From: {0}\r\n", message.From));
            if (!Check_Response(s, SmtpResponses.GenericSuccess))
            {
                Console.WriteLine("Mail command Failed!.");
                s.Close();
                return false;
            }

            #endregion

            #region Send RCPT commands (one for each recipient)

            foreach (var to in message.To)
            {
                Senddata(s, string.Format("RCPT TO: {0}\r\n", to.Address));
                if (!Check_Response(s, SmtpResponses.GenericSuccess))
                {
                    Console.WriteLine("RCPT command Failed ({0})!.", to.Address);
                    s.Close();
                    return false;
                }
            }

            #endregion

            #region Send RCPT commands (one for each recipient) - CC

            if (message.CC.Count > 0)
                foreach (var to in message.CC)
                {
                    Senddata(s, string.Format("RCPT TO: {0}\r\n", to.Address));
                    if (!Check_Response(s, SmtpResponses.GenericSuccess))
                    {
                        Console.WriteLine("RCPT command Failed ({0})!.", to.Address);
                        s.Close();
                        return false;
                    }
                }

            #endregion

            #region Send the DATA command

            var header = new StringBuilder();
            header.Append("From: " + message.From + "\r\n");
            header.Append("To: ");
            for (var i = 0; i < message.To.Count; i++)
            {
                header.Append(i > 0 ? "," : "");
                header.Append(message.To[i]);
            }

            header.Append("\r\n");
            if (message.CC.Count > 0)
            {
                header.Append("Cc: ");
                for (var i = 0; i < message.CC.Count; i++)
                {
                    header.Append(i > 0 ? "," : "");
                    header.Append(message.CC[i]);
                }

                header.Append("\r\n");
            }

            header.Append("Date: ");
            header.Append(DateTime.Now.ToString("ddd, d M y H:m:s z"));
            header.Append("\r\n");
            header.Append("Subject: " + message.Subject + "\r\n");
            header.Append("X-Mailer: Narayan EMail v2\r\n");
            var msgBody = message.Body;
            if (!msgBody.EndsWith("\r\n"))
                msgBody += "\r\n";
            if (message.Attachments.Count > 0)
            {
                header.Append("MIME-Version: 1.0\r\n");
                header.Append("Content-Type: multipart/mixed; boundary=unique-boundary-1\r\n");
                header.Append("\r\n");
                header.Append("This is a multi-part message in MIME format.\r\n");
                var sb = new StringBuilder();
                sb.Append("--unique-boundary-1\r\n");
                sb.Append("Content-Type: text/plain\r\n");
                sb.Append("Content-Transfer-Encoding: 7Bit\r\n");
                sb.Append("\r\n");
                sb.Append(msgBody + "\r\n");
                sb.Append("\r\n");

                foreach (object o in message.Attachments)
                {
                    var a = o as Attachment;
                    byte[] binaryData;
                    if (a != null)
                    {
                        var f = new FileInfo(a.Name);
                        sb.Append("--unique-boundary-1\r\n");
                        sb.Append("Content-Type: application/octet-stream; file=" + f.Name + "\r\n");
                        sb.Append("Content-Transfer-Encoding: base64\r\n");
                        sb.Append("Content-Disposition: attachment; filename=" + f.Name + "\r\n");
                        sb.Append("\r\n");
                        var fs = f.OpenRead();
                        binaryData = new byte[fs.Length];
                        long bytesRead = fs.Read(binaryData, 0, (int) fs.Length);
                        fs.Close();
                        var base64String = Convert.ToBase64String(binaryData, 0, binaryData.Length);

                        for (var i = 0; i < base64String.Length;)
                        {
                            var nextchunk = 100;
                            if (base64String.Length - (i + nextchunk) < 0)
                                nextchunk = base64String.Length - i;
                            sb.Append(base64String.Substring(i, nextchunk));
                            sb.Append("\r\n");
                            i += nextchunk;
                        }

                        sb.Append("\r\n");
                    }
                }

                msgBody = sb.ToString();
            }

            Senddata(s, "DATA\r\n");
            if (!Check_Response(s, SmtpResponses.DataSuccess))
            {
                Console.WriteLine("Data command Failed!.");
                s.Close();
                return false;
            }

            header.Append("\r\n");
            header.Append(msgBody);
            header.Append(".\r\n");
            header.Append("\r\n");
            header.Append("\r\n");
            Senddata(s, header.ToString());
            if (!Check_Response(s, SmtpResponses.GenericSuccess))
            {
                Console.WriteLine("Data command Failed!.");
                s.Close();
                return false;
            }

            #endregion

            #region quit

            Senddata(s, "QUIT\r\n");
            Check_Response(s, SmtpResponses.QuitSuccess);
            s.Close();

            #endregion

            return true;
        }

        private static void Senddata(Socket s, string msg)
        {
            //			StreamWriter sw= new FileInfo("send.txt").AppendText();
            //			sw.WriteLine(msg);
            //			sw.Close();
            //			sw=null;
            //			GC.Collect();
            var msgBytes = Encoding.ASCII.GetBytes(msg);
            s.Send(msgBytes, 0, msgBytes.Length, SocketFlags.None);
        }

        private static bool Check_Response(Socket s, SmtpResponses responseExpected)
        {
            string sResponse;
            int response;
            var bytes = new byte[1024];
            //			Console.WriteLine("Waiting for {0}", response_expected);
            while (s.Available == 0) Thread.Sleep(100);

            s.Receive(bytes, 0, s.Available, SocketFlags.None);
            sResponse = Encoding.ASCII.GetString(bytes);
            //Console.WriteLine(sResponse);
            response = Convert.ToInt32(sResponse.Substring(0, 3));
            if (response != (int) responseExpected)
                return false;
            return true;
        }
    }
}