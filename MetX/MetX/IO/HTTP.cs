using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Data.SqlClient;

namespace MetX.IO
{
	/// <summary>Provides static methods to common web based tasks</summary>
	public static class HTTP
	{
        public static class UserAgents
        {
            public const string IE60XPsp2dotNET2 = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
            public const string Firefox2001XP= "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.1) Gecko/20061204 Firefox/2.0.0.1";
            public const string Nescape81XPGeckoDotNET = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.7.5) Gecko/20060127 Netscape/8.1";
            public const string Opera91XP = "Opera/9.10 (Windows NT 5.1; U; en)";
        }
        /// <summary>Makes an HTTP POST call returning the response (no headers)</summary>
        /// <param name="lcURL">The Web address to post to</param>
        /// <param name="PostData">The URL encoded data to send</param>
        /// <returns>The response text from the post (ASCII encoded).</returns>
        public static string GetURL(string lcURL, string PostData)
		{
			Stream myStream;
            WebRequest myWebRequest = WebRequest.Create(lcURL);
			byte[] uploadData = Encoding.ASCII.GetBytes(PostData);
			
			myWebRequest.Method = "POST";
			myWebRequest.ContentType = "application/x-www-form-urlencoded";
			myWebRequest.ContentLength = uploadData.Length;
			myStream = myWebRequest.GetRequestStream();
			myStream.Write(uploadData, 0, uploadData.Length);
			myStream.Close();
				
			WebResponse myWebResponse = myWebRequest.GetResponse();
			myStream = myWebResponse.GetResponseStream();
			byte[] returnedData = new byte[myWebResponse.ContentLength];
			myStream.Read(returnedData, 0, (int)myWebResponse.ContentLength);

			return Encoding.ASCII.GetString(returnedData);
		}

        /// <summary>Makes an HTTP POST call returning the response (no headers)</summary>
        /// <param name="lcURL">The Web address to post to</param>
        /// <param name="Timeout">The maximum number of seconds to wait for a response</param>
        /// <param name="UserAgent">The UserAgent header value to pass</param>
        /// <returns>The response text from the post (ASCII encoded)</returns>
        public static string GetURL(string lcURL, int Timeout, string UserAgent)
		{	
			//  *** Establish the request
			HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(lcURL);
			//  *** Set properties
			if (Timeout <  1000)
				Timeout = Timeout * 1000;
			loHttp.Timeout = Timeout;
			loHttp.UserAgent = (UserAgent == null || UserAgent.Length == 0 ? UserAgents.IE60XPsp2dotNET2 : UserAgent);
			//  *** Retrieve request info headers
			HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();
			Encoding enc = Encoding.GetEncoding(1252);			//  Windows default Code Page
			StreamReader loResponseStream = new StreamReader(loWebResponse.GetResponseStream(), enc);
			string lcHtml = loResponseStream.ReadToEnd();
			loWebResponse.Close();
			loResponseStream.Close();
			return lcHtml;
		}

        /// <summary>Makes an HTTP POST call returning the response (no headers)</summary>
        /// <param name="lcURL">The Web address to post to</param>
        /// <param name="Timeout">The maximum number of seconds to wait</param>
        /// <returns>The response text from the post (Windows default code page encoded).</returns>
        public static string GetURL(string lcURL, int Timeout)
		{
			//  *** Establish the request
			HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(lcURL);
			//  *** Set properties
			if (Timeout <  1000)
				Timeout = Timeout * 1000;
			loHttp.Timeout = Timeout;
            loHttp.UserAgent = UserAgents.IE60XPsp2dotNET2;
			//  *** Retrieve request info headers
			HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();
			Encoding enc = Encoding.GetEncoding(1252);			//  Windows default Code Page
			StreamReader loResponseStream = new StreamReader(loWebResponse.GetResponseStream(), enc);
			string lcHtml = loResponseStream.ReadToEnd();
			loWebResponse.Close();
			loResponseStream.Close();
			return lcHtml;
		}

        /// <summary>Makes an HTTP POST call returning the response (no headers)</summary>
        /// <param name="lcURL">The Web address to post to</param>
        /// <returns>The response text from the post (Windows default code page encoded).</returns>
        public static string GetURL(string lcURL)
        {
            int Timeout = 30;

            //  *** Establish the request
            HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(lcURL);
            //  *** Set properties
            if (Timeout < 1000)
                Timeout = Timeout * 1000;
            loHttp.Timeout = Timeout;
            loHttp.UserAgent = UserAgents.IE60XPsp2dotNET2;
            //  *** Retrieve request info headers
            HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();
            Encoding enc = Encoding.GetEncoding(1252);			//  Windows default Code Page
            StreamReader loResponseStream = new StreamReader(loWebResponse.GetResponseStream(), enc);
            string lcHtml = loResponseStream.ReadToEnd();
            loWebResponse.Close();
            loResponseStream.Close();
            return lcHtml;
        }

        /// <summary>Makes an HTTP POST call returning the response (no headers)</summary>
        /// <param name="lcURL">The Web address to post to</param>
        /// <returns>The response text from the post (Windows default code page encoded).</returns>
        public static byte[] GetUrlByteArray(string lcURL)
		{	
			int Timeout = 30;

			//  *** Establish the request
			HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(lcURL);
			//  *** Set properties
			if (Timeout <  1000)
				Timeout = Timeout * 1000;
			loHttp.Timeout = Timeout;
            loHttp.UserAgent = UserAgents.IE60XPsp2dotNET2;
			//  *** Retrieve request info headers
			HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();
			Encoding enc = Encoding.GetEncoding(1252);			//  Windows default Code Page
			Stream loResponseStream = loWebResponse.GetResponseStream();
            byte[] ret = new byte[loResponseStream.Length];
            loResponseStream.Read(ret, 0, (int) loResponseStream.Length);
			loWebResponse.Close();
			loResponseStream.Close();
			return ret;
		}

		/// <summary>Pulls the contents of URL to the Response object. NOTE: Response is cleared before the URL's response is written to TheResponse</summary>
		/// <param name="URL">The URL to pull</param>
		/// <param name="TheResponse">The HttpResponse to write the result to</param>
		/// <param name="ClearResponse">True if you want TheResponse.Clear() to be called before writing URL response to TheResponse</param>
		/// <param name="EndResponse">True if you want TheResponse.End() to be called after writing URL response to TheResponse</param>
		public static void PullPage(Uri URL, System.Web.HttpResponse TheResponse, bool ClearResponse, bool EndResponse)
		{
			WebRequest Req;

			Req = WebRequest.Create(URL);
			WebResponse Resp;
			try
			{
				Resp = Req.GetResponse();
			}
			catch (Exception exc)
			{
                if(ClearResponse)
				    TheResponse.Clear();
				TheResponse.Write(exc.Message);
                if (EndResponse)
                    TheResponse.End();
				return ;
			}

			StreamReader netStream = new StreamReader(Resp.GetResponseStream());
			TheResponse.Clear();
			TheResponse.Write(netStream.ReadToEnd());
            if(EndResponse)
			    TheResponse.End();
		}
	}
}
