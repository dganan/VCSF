using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace VCS
{
	public class TestConverter : Converter
	{
		private XDocument document;
		private XElement root;

		public TestConverter(string file)
		{
			document = XDocument.Load(file);

			root = document.Root;
		}

		public override List<CollaborativeSessionDescriptor> AvailableCollaborativeSessions
		{
			get
			{
				List<CollaborativeSessionDescriptor> forums = new List<CollaborativeSessionDescriptor>();
								
				foreach (XElement forum in root.GetElementsByName("Forum"))
				{
					forums.Add(new CollaborativeSessionDescriptor() { 
						
						Id = forum.GetAttributeValue("Id"),
						Name = forum.GetAttributeValue("Name"),
						Description = forum.GetAttributeValue("Description"),
						Site = new Site () { Id = forum.GetElementsByName("Site").First().GetAttributeValue("Id"),
											 Name = forum.GetElementsByName("Site").First().GetAttributeValue("Name") }
					});
				}

				return forums;
			}
		}

		protected override CollaborativeSession ReadCollaborativeSessionData(string csId)
		{
			XElement csElement = root.GetElementsByName("Forum").Where(x => x.GetAttributeValue("Id") == csId).FirstOrDefault();

			CollaborativeSession cs2 = new CollaborativeSession();

			cs2.Id = csId;

			if (csElement != null)
			{
				cs2.Name = csElement.GetAttributeValue("Name");
				cs2.Description = csElement.GetAttributeValue("Description");
				cs2.Site = new Site() { Id = csElement.GetElementsByName("Site").First().GetAttributeValue("Id"), Name = csElement.GetElementsByName("Site").First().GetAttributeValue("Name") };
			}

			return cs2;
		}

		protected override List<UserAccount> ReadUserAccounts(string csId)
		{
			List<UserAccount> users = new List<UserAccount>();

			XElement forum = root.GetElementsByName("Forum").Where(x => x.GetAttributeValue("Id") == csId).FirstOrDefault();

           	if (forum != null)
			{
				foreach (XElement user in root.GetElementsByName("Users").SelectMany(x => x.GetElementsByName("User")))
				{
					UserAccount ua = new UserAccount()
					{
						Id = user.GetAttributeValue("Id"),
						Name = user.GetAttributeValue("Name"),
						Email = user.GetAttributeValue("Email"),
						Role = user.GetAttributeValue("Role").ToRole()
					};

					string avatarFile = user.GetAttributeValue("Avatar");

					if (avatarFile != null)
					{
						if (File.Exists(avatarFile))
						{
							using (FileStream fs = new FileStream(avatarFile, FileMode.Open))
							{
								byte[] bytes = new byte[fs.Length];

								fs.Read(bytes, 0, bytes.Length);

								ua.Avatar = bytes;
							}
						}
					}

                    users.Add(ua);
				}
			}

			return users;
		}

		protected List<Category> ReadCategories(string csId)
		{
			List<Category> categories = new List<Category>();

			XElement forum = root.GetElementsByName("Forum").Where(x => x.GetAttributeValue("Id") == csId).FirstOrDefault();

			if (forum != null)
			{
				foreach (XElement user in forum.GetElementsByName("Categories").SelectMany(x => x.GetElementsByName("Category")))
				{
					categories.Add(new Category()
					{
						Id = user.GetAttributeValue("Id"),
						Name = user.GetAttributeValue("Name")
					});
				}
			}

			return categories;
		}

		protected override List<Post> ReadPosts(string csId)
		{
            List<Post> posts = new List<Post>();

            List<UserAccount> users = ReadUserAccounts(csId);

            XElement forum = root.GetElementsByName("Forum").Where(x => x.GetAttributeValue("Id") == csId).FirstOrDefault();

            if (forum != null)
            {
				List<Category> categories = ReadCategories(csId);

                foreach (XElement p in forum.GetElementsByName("Posts").SelectMany(x => x.GetElementsByName("Post")))
                {
                    string id = p.GetAttributeValue("Id");
                    UserAccount creator = users.Where(x => x.Id == p.GetAttributeValue("CreatorId")).FirstOrDefault();
                    DateTime creationDate = DateTime.Parse (p.GetAttributeValue("CreationDate"));
					string content = p.GetAttributeValue("Content");

					if (content == null)
					{
						content = p.Value;
					}

					//content = content.ToSafeHtmlString();

					Post post = new Post() { Id = id, Creator = creator, CreationDate = creationDate, Content = content };
					
					posts.Add(post);

					// Add topics / categories

					post.Categories = p.GetElementsByName("Category")
											.Select (x=>categories
												.Where(y => y.Id == x.GetAttributeValue("Id")).FirstOrDefault()).ToList();
				}

				// Read posts replies

				foreach (XElement post in forum.GetElementsByName("Posts").SelectMany(x => x.GetElementsByName("Post")))
				{
					Post p = posts.Where(x => x.Id == post.GetAttributeValue("Id")).FirstOrDefault();

					foreach (XElement replies in post.GetElementsByName("Reply"))
					{
						Post reply = posts.Where(x => x.Id == replies.GetAttributeValue("Id")).FirstOrDefault();

						reply.ReplyOf = p;
						
						//p.Replies.Add(reply);
					}
				}
            }

            return posts;
		}
	}
}
