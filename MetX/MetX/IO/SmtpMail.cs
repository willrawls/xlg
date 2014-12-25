using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Net.Mail;

namespace MetX.IO
{
	/// <summary>Provides methids to send email via smtp, with out CDO SYS installed.
	/// <para>This is a manual implementation of the SMTP protocol.</para>
	/// </summary>
	public static class SmtpMail
	{
		/// <summary>Get or Set the name of the SMTP relay mail server</summary>
		public static string SmtpServer;
		
		private enum SMTP_Responses: int
		{
			CONNECT_SUCCESS = 220,
			GENERIC_SUCCESS = 250,
			DATA_SUCCESS	= 354,
			QUIT_SUCCESS	= 221
    		
		}
		
		/// <summary>Send an Email</summary>
		/// <param name="FromName">The displayed name for the FROM address</param>
		/// <param name="FromEmail">The FROM address</param>
		/// <param name="ToName">The displayed name for the TO address</param>
		/// <param name="ToEmail">The TO address</param>
		/// <param name="Subject">The SUBJECT for the email</param>
		/// <param name="Body">The text body of the email</param>
		/// <returns>True if the email was sent</returns>
		public static bool Send(string FromName, string FromEmail, string ToName, string ToEmail, string Subject, string Body)
		{
			bool ret = false;
			MailMessage mm = new MailMessage();
			mm.From = new MailAddress(FromEmail, FromName);
			mm.To.Add(new MailAddress(ToEmail, ToName));
			mm.Subject = Subject;
			mm.Body = Body;
            //if(SendViaCustomCode)
            //{
				SmtpServer = System.Configuration.ConfigurationManager.AppSettings["SmtpServer"];
				ret = Send(mm);
            //}
            //else
            //{
            //    SmtpMail.SmtpServer = System.Configuration.ConfigurationManager.AppSettings["SmtpServer"];
            //    try
            //    {
            //        SmtpMail.Send(mm);
            //        ret = true;
            //    }
            //    catch { }
            //}
			return ret;
		}

		
		/// <summary>Sends an email message</summary>
		/// <param name="message">The MailMessage to send</param>
		/// <returns>Returns true if the email was sent.</returns>
		public static bool Send(MailMessage message)
		{
			IPHostEntry lipa = Dns.GetHostEntry(SmtpServer);
			IPEndPoint target = new IPEndPoint(lipa.AddressList[0], 25);
			Socket s= new Socket(target.AddressFamily, SocketType.Stream,ProtocolType.Tcp);
			s.Connect(target);
    			#region connect
			if(!Check_Response(s, SMTP_Responses.CONNECT_SUCCESS))
			{
				Console.WriteLine("Server didn't respond.");
				s.Close();
				return false;
			}
    			#endregion
    			#region send helo
			Senddata(s, string.Format("HELO {0}\r\n", Dns.GetHostName() ));
			if(!Check_Response(s, SMTP_Responses.GENERIC_SUCCESS))
			{
				Console.WriteLine("Helo Failed!.");
				s.Close();
				return false;
			}
    			#endregion
    			#region Send the MAIL command
			Senddata(s, string.Format("MAIL From: {0}\r\n", message.From ));
			if(!Check_Response(s, SMTP_Responses.GENERIC_SUCCESS))
			{
				Console.WriteLine("Mail command Failed!.");
				s.Close();
				return false;
			}
    			#endregion
    			#region Send RCPT commands (one for each recipient)
			foreach (MailAddress To in message.To)
			{
				Senddata(s, string.Format("RCPT TO: {0}\r\n", To.Address));
				if(!Check_Response(s, SMTP_Responses.GENERIC_SUCCESS))
				{
					Console.WriteLine("RCPT command Failed ({0})!.", To.Address);
					s.Close();
					return false;
				}
			}
    			#endregion
    			#region Send RCPT commands (one for each recipient) - CC
			if(message.CC.Count > 0)
			{
                foreach (MailAddress To in message.CC)
				{
					Senddata(s, string.Format("RCPT TO: {0}\r\n", To.Address));
					if(!Check_Response(s, SMTP_Responses.GENERIC_SUCCESS))
					{
						Console.WriteLine("RCPT command Failed ({0})!.", To.Address);
						s.Close();
						return false;
					}
				}
			}
    			#endregion
    			#region Send the DATA command
			StringBuilder Header=new StringBuilder();
			Header.Append("From: " + message.From + "\r\n");
			Header.Append("To: ");
            for (int i = 0; i < message.To.Count; i++)
			{
				Header.Append( i > 0 ? "," : "" );
                Header.Append(message.To[i]);
			}
			Header.Append("\r\n");
			if(message.CC.Count > 0)
			{
				Header.Append("Cc: ");
                for (int i = 0; i < message.CC.Count; i++)
				{
					Header.Append( i > 0 ? "," : "" );
                    Header.Append(message.CC[i]);
				}
				Header.Append("\r\n");
			}
			Header.Append( "Date: " );
			Header.Append(DateTime.Now.ToString("ddd, d M y H:m:s z" ));
			Header.Append("\r\n");
			Header.Append("Subject: " + message.Subject+ "\r\n");
			Header.Append( "X-Mailer: Narayan EMail v2\r\n" );
			string MsgBody = message.Body;
			if(!MsgBody.EndsWith("\r\n"))
				MsgBody+="\r\n";
			if(message.Attachments.Count>0)
			{
				Header.Append( "MIME-Version: 1.0\r\n" );
				Header.Append( "Content-Type: multipart/mixed; boundary=unique-boundary-1\r\n" );
				Header.Append("\r\n");
				Header.Append( "This is a multi-part message in MIME format.\r\n" );
				StringBuilder sb = new StringBuilder();
				sb.Append("--unique-boundary-1\r\n");
				sb.Append("Content-Type: text/plain\r\n");
				sb.Append("Content-Transfer-Encoding: 7Bit\r\n");
				sb.Append("\r\n");
				sb.Append(MsgBody + "\r\n");
				sb.Append("\r\n");
    				
				foreach(object o in message.Attachments)
				{
					Attachment a = o as Attachment;
					byte[] binaryData;
					if(a!=null)
					{
						FileInfo f = new FileInfo(a.Name);
						sb.Append("--unique-boundary-1\r\n");
						sb.Append("Content-Type: application/octet-stream; file=" + f.Name + "\r\n");
						sb.Append("Content-Transfer-Encoding: base64\r\n");
						sb.Append("Content-Disposition: attachment; filename=" + f.Name + "\r\n");
						sb.Append("\r\n");
						FileStream fs = f.OpenRead();
						binaryData = new Byte[fs.Length];
						long bytesRead = fs.Read(binaryData, 0, (int)fs.Length);
						fs.Close();
						string base64String = System.Convert.ToBase64String(binaryData, 0,binaryData.Length);
    						
						for(int i=0; i< base64String.Length ; )
						{
							int nextchunk=100;
							if(base64String.Length - (i + nextchunk ) <0)
								nextchunk = base64String.Length -i;
							sb.Append(base64String.Substring(i, nextchunk));
							sb.Append("\r\n");
							i+=nextchunk;
    						
						}
						sb.Append("\r\n");
    					
					}
				}
				MsgBody=sb.ToString();
			}
    			
			Senddata(s, ("DATA\r\n"));
			if(!Check_Response(s, SMTP_Responses.DATA_SUCCESS))
			{
				Console.WriteLine("Data command Failed!.");
				s.Close();
				return false;
			}
			Header.Append( "\r\n" );
			Header.Append( MsgBody );
			Header.Append( ".\r\n" );
			Header.Append( "\r\n" );
			Header.Append( "\r\n" );
			Senddata(s, Header.ToString());
			if(!Check_Response(s, SMTP_Responses.GENERIC_SUCCESS ))
			{
				Console.WriteLine("Data command Failed!.");
				s.Close();
				return false;
			}			
    			#endregion
    			#region quit
			Senddata(s, "QUIT\r\n");
			Check_Response(s, SMTP_Responses.QUIT_SUCCESS );
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
			byte[] _msg = Encoding.ASCII.GetBytes(msg);
			s.Send(_msg , 0, _msg .Length, SocketFlags.None);
		}
		
		private static bool Check_Response(Socket s, SMTP_Responses response_expected )
		{
			string sResponse;
			int response;
			byte[] bytes = new byte[1024];
			//			Console.WriteLine("Waiting for {0}", response_expected);
			while (s.Available==0)
			{
				System.Threading.Thread.Sleep(100);
			}
    			
			s.Receive(bytes, 0, s.Available, SocketFlags.None);
			sResponse = Encoding.ASCII.GetString(bytes);
			//Console.WriteLine(sResponse);
			response = Convert.ToInt32(sResponse.Substring(0,3));
			if(response != (int)response_expected)
				return false;
			return true;
		}
	}
}
