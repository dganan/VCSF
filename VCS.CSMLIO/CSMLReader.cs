using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NC3A.SI.Rowlex;
using rdfs.org.sioc.ns;

namespace VCS
{
	public static class CSMLReader
	{
		public static CollaborativeSession ReadCSMLFile(string file)
		{
			CollaborativeSession cs = new CollaborativeSession();

			RdfDocument rdfDocument = new RdfDocument(file);


			// Read forum data

			Forum_ forum = rdfDocument.GetIndividuals(Forum_.Uri, true, true, false).FirstOrDefault() as Forum_;

			cs.Id = forum.id;
			cs.Name = forum.httppurlorgdctermstitle;
			cs.Description = forum.httppurlorgdctermsdescription;

			cs.Site = (forum.hashost_ as Site_).ToSite();


			// Read users

			var users = forum.hassubscriber_s.Select(x => x as UserAccount_);

			//var users = rdfDocument.GetIndividuals(UserAccount_.Uri, true, true, false).Select(x => x as UserAccount_);

			cs.UserAccounts = users.Select(x => x.ToUserAccount()).ToList();


			// read posts    

			var posts = forum.containerof_s.Select(x => x as Post_);
			
			//var posts = rdfDocument.GetIndividuals(Post_.Uri, true).Select(x => x as Post_);

			cs.Posts = posts.Select(x => x.ToPost()).ToList();


			// read post relations

			foreach (Post_ p in posts)
			{
				Post post = cs.Posts.Where(x => x.Id == p.id).FirstOrDefault();

				post.Creator = cs.UserAccounts.Where(y => y.Id == (p.hascreator_ as UserAccount_).id).FirstOrDefault();

				foreach (Post_ p2 in p.hasreply_s)
				{
					Post post2 = cs.Posts.Where(x => x.Id == p2.id).FirstOrDefault();

					//post.Replies.Add(post2);
					////post2.ReplyOf = post;
				}
			}

			return cs;
		}

		private static UserAccount ToUserAccount(this UserAccount_ x)
		{
			UserAccount ua = new UserAccount();

			ua.Id = x.id;
			ua.Name = x.name;
			ua.Email = x.email.ToString();
			ua.Role = (x.hasfunction_ as Role_).ToRole();

			return ua;
		}

		private static Post ToPost(this Post_ x)
		{
			Post p = new Post();

			p.Id = x.id;
			p.CreationDate = DateTime.Parse(x.httppurlorgdctermscreated);
			p.Content = x.content;

			// TODO : Add topics / categories

			return p;
		}

		private static Role ToRole(this Role_ x)
		{
			return x.name.ToString().ToRole();
		}

		private static Site ToSite(this Site_ x)
		{
			return new Site() { Id = x.id, Name = x.name };
		}
	}
}
