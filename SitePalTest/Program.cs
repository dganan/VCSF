using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

namespace SitePalTest
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://vhost.oddcast.com/mng/testMngExportTTSAudio.php");

				ASCIIEncoding encoding = new ASCIIEncoding();
				
				string postData = "username=user";
				postData += "&password=pass";

				byte[] data = encoding.GetBytes(postData);

				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = data.Length;

				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(data, 0, data.Length);

					requestStream.Close();

					// Get the response.
					WebResponse response = request.GetResponse();
					
					// Display the status.
					Console.WriteLine(((HttpWebResponse)response).StatusDescription);
					
					// Get the stream containing content returned by the server.
					using (Stream responseStream = response.GetResponseStream())
					{
						// Open the stream using a StreamReader for easy access.
						StreamReader reader = new StreamReader(responseStream);
					
						// Read the content.
						string responseFromServer = reader.ReadToEnd();
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}
