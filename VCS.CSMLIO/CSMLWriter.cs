using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rdfs.org.sioc.ns;
using Properties.purl.org.dc.terms;
using NC3A.SI.Rowlex;

namespace VCS
{
	public static class CSMLWriter
	{
		public static void WriteCSMLFile(CollaborativeSession cs, string file)
		{
			RdfDocument rdfDocument = new RdfDocument();

			Forum_ forum = cs.ToForum_(rdfDocument);

			foreach (UserAccount ua in cs.UserAccounts)
			{
				UserAccount_ ua_ = ua.ToUserAccount_(rdfDocument);

				// Add subscriber relation
				forum.Addhassubscriber_ (ua_);
				//ua_.Addsubscriberof_(forum);
			}

			foreach (Post p in cs.Posts)
			{
				Post_ p_ = p.ToPost_(rdfDocument);

				forum.Addcontainerof(p_);
				//p_.Addhascontainer(forum);
			}

			// replies

			foreach (Post p in cs.Posts)
			{
				Post_ p_ = GetPostById(rdfDocument, p.Id);

				//foreach (Post reply in p.Replies)
				//{
				//    Post_ reply_ = GetPostById(rdfDocument, reply.Id);

				//    p_.Addhasreply_(reply_);

				//    reply_.Addreplyof(p_);
				//}
			}

			rdfDocument.ExportToRdfXml(file);
		}

		private static Post_ GetPostById(RdfDocument rdfDocument, string id)
		{
			return rdfDocument.GetIndividuals(Post_.Uri, true, true, false).Select(x => x as Post_).Where(x => x.id == id).FirstOrDefault();
		}

		private static UserAccount_ GetUserAccountById(RdfDocument rdfDocument, string id)
		{
			return rdfDocument.GetIndividuals(UserAccount_.Uri, true, true, false).Select(x => x as UserAccount_).Where(x => x.id == id).FirstOrDefault();
		}

		private static Forum_ ToForum_(this CollaborativeSession cs, RdfDocument rdfDocument)
		{
			Forum_ forum = new Forum_(rdfDocument);

			forum.id = cs.Id;
			forum.name = cs.Name;
			forum.Comment = cs.Description;
			forum.hashost = cs.Site.ToSite_(rdfDocument);

			return forum;
		}

		private static UserAccount_ ToUserAccount_(this UserAccount user, RdfDocument rdfDocument)
		{
			UserAccount_ ua_ = new UserAccount_(rdfDocument);
	
			ua_.id = user.Id;
			ua_.name = user.Name;
			ua_.email = user.Email;
			ua_.hasfunction_ = user.Role.ToRole_(rdfDocument);

			return ua_;
		}

		private static Post_ ToPost_(this Post p, RdfDocument rdfDocument)
		{
			Post_ p_ = new Post_(rdfDocument);

			p_.Addid(new RdfLiteral(p.Id));

			// creator
			p_.hascreator_ = GetUserAccountById(rdfDocument, p.Creator.Id);
				
			// date created
			p_.httppurlorgdctermscreated = new RdfLiteral(p.CreationDate.ToString("yyyy/MM/dd"));

			// content
			p_.content = p.Content;

			// TODO : Add topics / categories

			return p_;
		}

		private static Site_ ToSite_(this Site site, RdfDocument rdfDocument)
		{
			return new Site_(rdfDocument) { id = site.Id, name = site.Name };		
		}

		private static Role_ ToRole_(this Role role, RdfDocument rdfDocument)
		{
			return new Role_(rdfDocument) { name = role.ToStringValue() };
		}
	}
}
