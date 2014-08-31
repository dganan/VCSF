using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;
using System.IO;
using Moma.IwtForumConverter;

namespace VCS
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ConversionService" in code, svc and config file together.
	public class ConversionService : IConversionService
	{
		public List<CollaborativeSessionDescriptor> GetAvailableCollaborativeSessions(string dataSource)
		{
			List<CollaborativeSessionDescriptor> list = new List<CollaborativeSessionDescriptor>();

			try
			{
				IConverter converter = CreateConverter(dataSource);
				
				list = converter.AvailableCollaborativeSessions;
			}
			catch (Exception e)
			{
				Logger.LogException(e);
			}

			return list;
		}

		private IConverter CreateConverter(string dataSource)
		{
			IConverter converter = null;

			if (dataSource == "DF")
			{
				converter = new TestConverter(ConfigurationManager.AppSettings["CollaborativeSessionsFile"]);
			}
			else if (dataSource == "phpBB")
			{
				converter = new PhpBBForumsUOCConverter();
			}
			else
			{
				converter = new ForumConverter();
			}

			return converter;
		}

		public CollaborativeSession GetCollaborativeSessionById(string id, string dataSource, params string[] parameters)
		{
			CollaborativeSession cs = null;

			try
			{
				IConverter converter = CreateConverter(dataSource);

				cs = converter.ReadCollaborativeSession(id, parameters);
				
				// Clean messages
				foreach (Post p in cs.Posts)
				{
					p.Content = HtmlRemoval.StripTagsRegex(p.Content);

					p.Content = p.Content.Replace("\r\n", " ");
					p.Content = p.Content.Replace("\n\r", " ");
				}


				//int i = 0;

				//foreach (UserAccount ua in cs.UserAccounts)
				//{
				//    if (ua.Avatar != null && ua.Avatar.Length > 0)
				//    {
				//        System.IO.FileStream fs = new System.IO.FileStream("C:\\VCS\\images\\test_"+ i + ".jpg", System.IO.FileMode.Create);

				//        fs.Write(ua.Avatar, 0, ua.Avatar.Length);

				//        fs.Close();

				//        i++;
				//    }
				//}
			}
			catch (Exception e)
			{
				Logger.LogException(e);
			}

			return cs;
		}
	}
}
