using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Configuration;
using System.Net;

namespace VCS
{
	public class PhpBBForumsUOCConverter : Converter
	{
		public PhpBBForumsUOCConverter()
		{
		}

		public override CollaborativeSession ReadCollaborativeSession(string csId, params string[] parameters)
		{
			CollaborativeSession collaborativeSession = ReadCollaborativeSessionData(csId);

			// Call service to recover JSON data

			var data = RecoverServiceData(csId, parameters);
			
			// process data to convert it into UserAccount and Posts lists
			
			collaborativeSession.UserAccounts = ReadUserAccounts(data);

			collaborativeSession.Posts = ReadPosts(collaborativeSession.UserAccounts, data);

			return collaborativeSession;
		}

		private List<PhpBBUOCMessage> RecoverServiceData(string forumId, params string[] parameters)
		{
			using (var webClient = new System.Net.WebClient())
			{
				var json = webClient.DownloadString(ConfigurationManager.AppSettings["phpBBUrl"] + "?t=" + forumId + "&sid=" + parameters[0]);

				// Convert JSON data into objects

				JavaScriptSerializer serializer = new JavaScriptSerializer();

				List<PhpBBUOCMessage> data = (List<PhpBBUOCMessage>)serializer.Deserialize(json, typeof(List<PhpBBUOCMessage>));

				return data;
			}
		}

		private List<UserAccount> ReadUserAccounts(List<PhpBBUOCMessage> data)
		{
			int i = 0;

			var ua = data.Select(x=> new phpBBUOCAuthor () { author= x.author, picture = x.picture }).Distinct().Select(x => new UserAccount() { Name = x.author, Avatar = GetPhoto(x.picture) }).ToList();

			ua.ForEach((x) => { x.Id = i.ToString(); i++; });

			return ua;
		}

		private byte[] GetPhoto(string pictureurl)
		{
			int index = pictureurl.IndexOf("src=\"") + 5;
			int endindex = pictureurl.IndexOf("\"", index);

			if (index >= 0 && endindex >= 0 && endindex > index)
			{
				pictureurl = pictureurl.Substring(index, endindex - index);

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pictureurl);
				WebResponse response = request.GetResponse();

				Stream stream = response.GetResponseStream();

				const int bufferSize = 4096;

				using (BinaryReader br = new BinaryReader(stream))
				{
					using (MemoryStream ms = new MemoryStream())
					{
						byte[] buffer = new byte[bufferSize];

						int count;

						while ((count = br.Read(buffer, 0, buffer.Length)) != 0)
						{
							ms.Write(buffer, 0, count);
						}

						response.Close();

						return ms.ToArray();
					}
				}
			}

			return null;
		}
		
		private List<Post> ReadPosts(List<UserAccount> users, List<PhpBBUOCMessage> data)
		{
			List<Post> posts = new List<Post>();

			Post lastPost = null;

			int id = 0;

			foreach (PhpBBUOCMessage m in data)
			{
				Post p = new Post();

				p.Id = id.ToString();
				p.Creator = users.Where(x => x.Name == m.author).FirstOrDefault();
				p.Content = m.message;
				p.CreationDate = m.DateTime;

				p.ReplyOf = lastPost;

				posts.Add(p);
				lastPost = p;

				id++;
			}

			return posts;
		}

		protected override CollaborativeSession ReadCollaborativeSessionData(string csId)
		{
			CollaborativeSession cs2 = new CollaborativeSession();

			cs2.Id = csId;
			cs2.Name = "";
			cs2.Description = "";

			cs2.Site = new Site ();

			return cs2;
		}

		#region NOT USED

		public override List<CollaborativeSessionDescriptor> AvailableCollaborativeSessions
		{
			get
			{
				List<CollaborativeSessionDescriptor> forums = new List<CollaborativeSessionDescriptor>();

				// May be this functionality will not be available because the SLOs will be only converted from the forum itself, not through the VCS Creator UI

				forums.Add(new CollaborativeSessionDescriptor() { Id = "0", Name="Test" });

				return forums;
			}
		}
		

		protected override List<UserAccount> ReadUserAccounts(string csId)
		{
			List<UserAccount> users = new List<UserAccount>();

			// not used			
			
			return users;
		}

		protected List<Category> ReadCategories(string csId)
		{
			List<Category> categories = new List<Category>();

			// not available

			return categories;
		}

		protected override List<Post> ReadPosts(string csId)
		{
            List<Post> posts = new List<Post>();

			// not used			


            return posts;
		}

		#endregion
	}

	public class phpBBUOCAuthor : IEquatable<phpBBUOCAuthor>
	{
		public string author { get; set; }
		public string picture { get; set; }

		public bool Equals(phpBBUOCAuthor other)
		{
			return this.author == other.author;
		}
	}

	public class PhpBBUOCMessage 
	{
		public string author { get; set; }
		public string subject { get; set; }
		public string message { get; set; }
		public string date { get; set; }
		public string picture { get; set; }

		public DateTime DateTime
		{
			get
			{
				DateTime dt = DateTime.Now;

				DateTime.TryParseExact(date.Substring(0, date.Length-3), "ddd MMM dd, yyyy h:mm", CultureInfo.GetCultureInfo("es-ES"), DateTimeStyles.None, out dt);

				if (date.EndsWith("pm"))
				{
					dt = dt.AddHours(12);
				}

				return dt;
			}
		}
	}
}

