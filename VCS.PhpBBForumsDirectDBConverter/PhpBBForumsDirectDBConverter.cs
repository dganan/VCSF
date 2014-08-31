using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Net;

namespace VCS
{
	public class PhpBBForumsDirectDBConverter : Converter
	{
		public PhpBBForumsDirectDBConverter()
		{
		}

		public override List<CollaborativeSessionDescriptor> AvailableCollaborativeSessions
		{
			get
			{
				List<CollaborativeSessionDescriptor> forums = new List<CollaborativeSessionDescriptor>();

				using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["phpBB"].ConnectionString))
				{
					using (MySqlCommand comm = new MySqlCommand("SELECT * FROM `phpbb_forums`", conn))
					{
						conn.Open();

						MySqlDataReader reader = comm.ExecuteReader();

						while (reader.Read())
						{
							forums.Add(new CollaborativeSessionDescriptor() {

								Id = reader["forum_id"].ToString(),
								Name = (string)reader["forum_name"],
								Description = (string)reader["forum_desc"],
								Site = new Site()
								{
									Id = "phpBB test site",
									Name = "phpBB test site"
								}
							});
						}
					}
				}

				return forums;
			}
		}

		protected override CollaborativeSession ReadCollaborativeSessionData(string csId)
		{
			CollaborativeSession cs2 = new CollaborativeSession();

			cs2.Id = csId;
			cs2.Name = "";
			cs2.Description = "";

			cs2.Site = new Site();

			return cs2;
		}

		protected override List<UserAccount> ReadUserAccounts(string csId)
		{
			List<UserAccount> users = new List<UserAccount>();

			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["phpBB"].ConnectionString))
			{
				using (MySqlCommand comm = new MySqlCommand("SELECT U.user_id, U.username, U.user_email, U.user_avatar, UA.forum_id, AR.role_type FROM phpbb_users U INNER JOIN phpbb_acl_users UA ON U.user_id = UA.user_id INNER JOIN phpbb_acl_roles AR ON UA.auth_role_id = AR.role_id WHERE UA.forum_id = @forumId OR UA.forum_id = 0 GROUP BY U.user_id, U.username, U.user_email, U.user_avatar, UA.forum_id, AR.role_type ORDER BY U.user_id", conn))
				{
					comm.Parameters.Add (new MySqlParameter ("@forumId", csId));

					conn.Open();

					MySqlDataReader reader = comm.ExecuteReader();

					UserAccount lastUserAccount = null;

					while (reader.Read())
					{
						string userId = reader["user_id"].ToString();

						string role = reader["role_type"].ToString();

						if (role == "a_")
						{
							role = "Administrator";
						}
						else if (role == "m_")
						{
							role = "Teacher";
						}
						else
						{
							role = "Student";
						}

						if (lastUserAccount == null || lastUserAccount.Id != userId)
						{
							UserAccount ua = new UserAccount()
							{
								Id = userId,
								Name = reader["username"].ToString(),
								Email = reader["user_email"].ToString(),
								Role = role.ToRole()
							};

							if (!reader.IsDBNull(reader.GetOrdinal("user_avatar")))
							{
								string avatarFile = reader["user_avatar"].ToString();

								if (!String.IsNullOrWhiteSpace(avatarFile))
								{
									string phpBBUrl = ConfigurationManager.AppSettings["phpBBUrl"];

									phpBBUrl += "download/file.php?avatar=" + avatarFile;

									HttpWebRequest request = (HttpWebRequest)WebRequest.Create(phpBBUrl);

									HttpWebResponse response = (HttpWebResponse)request.GetResponse();

									BinaryReader br = new BinaryReader(response.GetResponseStream());

									ua.Avatar = new byte[response.ContentLength];

									br.Read(ua.Avatar, 0, (int)response.ContentLength);
								}
							}

							lastUserAccount = ua;

							users.Add(ua);
						}
						else 
						{
							if ((role == "Administrator") || (role == "Teacher" && lastUserAccount.Role != Role.Administrator))
							{
								lastUserAccount.Role = role.ToRole ();
							}
						}
					}
				}
			}

			return users;
		}

		protected List<Category> ReadCategories(string csId)
		{
			List<Category> categories = new List<Category>();

			//XElement forum = root.GetElementsByName("Forum").Where(x => x.GetAttributeValue("Id") == cs.Id).FirstOrDefault();

			//if (forum != null)
			//{
			//    foreach (XElement user in forum.GetElementsByName("Categories").SelectMany(x => x.GetElementsByName("Category")))
			//    {
			//        categories.Add(new Category()
			//        {
			//            Id = user.GetAttributeValue("Id"),
			//            Name = user.GetAttributeValue("Name")
			//        });
			//    }
			//}

			return categories;
		}

		protected override List<Post> ReadPosts(string csId)
		{
            List<Post> posts = new List<Post>();

			List<UserAccount> users = ReadUserAccounts(csId);

			//    List<Category> categories = ReadCategories(cs);

			using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["phpBB"].ConnectionString))
			{
				using (MySqlCommand comm = new MySqlCommand("SELECT post_id, topic_id, forum_id, poster_id, post_time, post_text FROM `phpbb_posts` WHERE forum_id = @forumId ORDER BY forum_id, topic_id, post_time", conn))
				{
					comm.Parameters.Add(new MySqlParameter("@forumId", csId));

					conn.Open();

					MySqlDataReader reader = comm.ExecuteReader();

					string lastTopic = null;
					Post lastPost = null;
					
					while (reader.Read())
					{
						string id = reader["post_id"].ToString();

						UserAccount creator = users.Where(x => x.Id == reader["poster_id"].ToString()).FirstOrDefault();
						
						// Conversion desde UNIX timestamp
						DateTime creationDate = new DateTime (1970, 1, 1);

						creationDate.AddSeconds ((uint)reader["post_time"]);

						string content = reader["post_text"].ToString();

						Post post = new Post() { Id = id, Creator = creator, CreationDate = creationDate, Content = content };

						posts.Add(post);

						//        // Add topics / categories

						//        post.Categories = p.GetElementsByName("Category")
						//                                .Select (x=>categories
						//                                    .Where(y => y.Id == x.GetAttributeValue("Id")).FirstOrDefault()).ToList();
						//    }


						string topic = reader["topic_id"].ToString();

						if (topic == lastTopic)
						{
							post.ReplyOf = lastPost;
						}
						else 
						{
							lastTopic = topic;
						}

						lastPost = post;
					}
				}
			}

            return posts;
		}
	}
}
