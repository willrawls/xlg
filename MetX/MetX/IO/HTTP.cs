using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace MetX.IO
{
	/// <summary>Provides static methods to common web based tasks</summary>
	public static class Http
	{
        public static class UserAgents
        {
            public const string Ie60XPsp2DotNet2 = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
            public const string Firefox2001Xp= "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.1) Gecko/20061204 Firefox/2.0.0.1";
            public const string Nescape81XpGeckoDotNet = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.7.5) Gecko/20060127 Netscape/8.1";
            public const string Opera91Xp = "Opera/9.10 (Windows NT 5.1; U; en)";
        }
        /// <summary>Makes an HTTP POST call returning the response (no headers)</summary>
        /// <param name="lcUrl">The Web address to post to</param>
        /// <param name="postData">The URL encoded data to send</param>
        /// <returns>The response text from the post (ASCII encoded).</returns>
        public static string GetUrl(string lcUrl, string postData)
		{
            WebRequest myWebRequest = WebRequest.Create(lcUrl);
			byte[] uploadData = Encoding.ASCII.GetBytes(postData);
			
			myWebRequest.Method = "POST";
			myWebRequest.ContentType = "application/x-www-form-urlencoded";
			myWebRequest.ContentLength = uploadData.Length;
			Stream myStream = myWebRequest.GetRequestStream();
			myStream.Write(uploadData, 0, uploadData.Length);
			myStream.Close();
				
			WebResponse myWebResponse = myWebRequest.GetResponse();
			myStream = myWebResponse.GetResponseStream();
            if (myStream == null) return null;
			byte[] returnedData = new byte[myWebResponse.ContentLength];
			myStream.Read(returnedData, 0, (int)myWebResponse.ContentLength);

			return Encoding.ASCII.GetString(returnedData);
		}

        /// <summary>Makes an HTTP POST call returning the response (no headers)</summary>
        /// <param name="lcUrl">The Web address to post to</param>
        /// <param name="timeout">The maximum number of seconds to wait for a response</param>
        /// <param name="userAgent">The UserAgent header value to pass</param>
        /// <returns>The response text from the post (ASCII encoded)</returns>
        public static string GetUrl(string lcUrl, int timeout, string userAgent)
		{	
			//  *** Establish the request
			HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(lcUrl);
			//  *** Set properties
			if (timeout <  1000)
				timeout = timeout * 1000;
			loHttp.Timeout = timeout;
			loHttp.UserAgent = (userAgent == null || userAgent.Length == 0 ? UserAgents.Ie60XPsp2DotNet2 : userAgent);
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
        /// <param name="lcUrl">The Web address to post to</param>
        /// <param name="timeout">The maximum number of seconds to wait</param>
        /// <returns>The response text from the post (Windows default code page encoded).</returns>
        public static string GetUrl(string lcUrl, int timeout)
		{
			//  *** Establish the request
			HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(lcUrl);
			//  *** Set properties
			if (timeout <  1000)
				timeout = timeout * 1000;
			loHttp.Timeout = timeout;
            loHttp.UserAgent = UserAgents.Ie60XPsp2DotNet2;
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
        /// <param name="lcUrl">The Web address to post to</param>
        /// <returns>The response text from the post (Windows default code page encoded).</returns>
        public static string GetUrl(string lcUrl)
        {
            int timeout = 30;

            //  *** Establish the request
            HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(lcUrl);
            //  *** Set properties
            if (timeout < 1000)
                timeout = timeout * 1000;
            loHttp.Timeout = timeout;
            loHttp.UserAgent = UserAgents.Ie60XPsp2DotNet2;
            //  *** Retrieve request info headers
            HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();
            Encoding enc = Encoding.GetEncoding(1252);			//  Windows default Code Page
            Stream responseStream = loWebResponse.GetResponseStream();
            if (responseStream != null)
            {
                StreamReader loResponseStream = new StreamReader(responseStream, enc);
                string lcHtml = loResponseStream.ReadToEnd();
                loWebResponse.Close();
                loResponseStream.Close();
                return lcHtml;
            }
            return null;
        }

        /// <summary>Makes an HTTP POST call returning the response (no headers)</summary>
        /// <param name="lcUrl">The Web address to post to</param>
        /// <returns>The response text from the post (Windows default code page encoded).</returns>
        public static byte[] GetUrlByteArray(string lcUrl)
		{	
			int timeout = 30;

			//  *** Establish the request
			HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(lcUrl);
			//  *** Set properties
			if (timeout <  1000)
				timeout = timeout * 1000;
			loHttp.Timeout = timeout;
            loHttp.UserAgent = UserAgents.Ie60XPsp2DotNet2;
			//  *** Retrieve request info headers
			HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();
			Encoding enc = Encoding.GetEncoding(1252);			//  Windows default Code Page
            byte[] ret = {};
            using (Stream loResponseStream = loWebResponse.GetResponseStream())
            {
                if(loResponseStream != null)
                {
                    ret = new byte[loResponseStream.Length];
                    loResponseStream.Read(ret, 0, (int) loResponseStream.Length);
                    loResponseStream.Close();        
                }
            }
            loWebResponse.Close();
			return ret;
		}

		/// <summary>Pulls the contents of URL to the Response object. NOTE: Response is cleared before the URL's response is written to TheResponse</summary>
		/// <param name="url">The URL to pull</param>
		/// <param name="theResponse">The HttpResponse to write the result to</param>
		/// <param name="clearResponse">True if you want TheResponse.Clear() to be called before writing URL response to TheResponse</param>
		/// <param name="endResponse">True if you want TheResponse.End() to be called after writing URL response to TheResponse</param>
		public static void PullPage(Uri url, HttpResponse theResponse, bool clearResponse, bool endResponse)
		{
		    WebRequest req = WebRequest.Create(url);
			WebResponse resp;
			try
			{
				resp = req.GetResponse();
			}
			catch (Exception exc)
			{
                if(clearResponse)
				    theResponse.Clear();
				theResponse.Write(exc.Message);
                if (endResponse)
                    theResponse.End();
				return ;
			}

			theResponse.Clear();
            Stream responseStream = resp.GetResponseStream();
            if(responseStream != null)
            {
                StreamReader netStream = new StreamReader(responseStream);
                theResponse.Write(netStream.ReadToEnd());
            }
            if(endResponse)
			    theResponse.End();
		}
	}
}
