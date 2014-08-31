using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;
using System.IO;

namespace VCS
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SLORepositoryService" in code, svc and config file together.
	public class SLORepositoryService : ISLORepositoryService
	{
		public List<SLODescriptor> GetAvailableSLOs(string userid)
		{
			List<SLODescriptor> list = new List<SLODescriptor>();

			try
			{
				list = RepositoryContentIO.GetAvailableSLOs();
			}
			catch (Exception e)
			{
				Logger.LogException(e);
			}

			return list;
		}

		public SLO GetSLOById(string id, string userid)
		{
			SLO slo = null;

			try
			{
				slo = RepositoryContentIO.GetSLOById(id);
			}
			catch (Exception e)
			{
				Logger.LogException(e);
			}

			return slo;
		}


		public string CreateSLOFromCollaborativeSession(string csid, string csthread, string sid, string source, string name, bool automaticCategorization, string userid, out string error)
		{
			string id = null;

			error = "";

			try
			{
				ConversionService cservice = new ConversionService();

				CollaborativeSession cs = cservice.GetCollaborativeSessionById(csid, source, sid);

				if (csthread != null)
				{
					cs = ReconstructCollaborativeSession(cs, csthread);
				}

				SLO slo = CS2toSLO.CreateSLO(cs, automaticCategorization);

				if (String.IsNullOrWhiteSpace(slo.Name))
				{
					slo.Name = name;
				}

				id = RepositoryContentIO.InsertSLO(slo);
			}
			catch (Exception e)
			{
				Logger.LogException(e);

				error = "An error has occurred while creating the new SLO";
			}

			return id;
		}

		private CollaborativeSession ReconstructCollaborativeSession(CollaborativeSession cs, string threadstr)
		{
			CollaborativeSession newcs = cs;

			//if (Int32.TryParse(threadstr, out thread))
			{
				newcs = new CollaborativeSession();

				newcs.Description = cs.Description;
				newcs.Name = cs.Name;
				newcs.Site = cs.Site;
				newcs.Url = cs.Url;

				newcs.Posts = cs.Posts.Where (x=>x.Categories.Where (y=>y.Id == threadstr).Count() > 0).ToList();
				newcs.UserAccounts = newcs.Posts.Select(x => x.Creator).Distinct().ToList();
				
				//List<Post> mainPosts = cs.Posts.Where(x => x.ReplyOf == null).ToList();

				//if (thread >= 0 && thread < mainPosts.Count())
				//{
				//    List<Post> all = new List<Post>();
				//    List<Post> current = new List<Post>();

				//    all.Add(mainPosts[thread]);
				//    current.Add(mainPosts[thread]);

				//    while (current.Count > 0)
				//    {
				//        List<Post> temp = new List<Post>();

				//        foreach (Post p in current)
				//        {
				//            temp.AddRange(cs.Posts.Where(x => x.ReplyOf != null && x.ReplyOf.Id == p.Id));
				//        }

				//        all.AddRange(temp);

				//        current = temp;
				//    }

				//    newcs.Posts = all;
				//    newcs.UserAccounts = all.Select(x => x.Creator).Distinct().ToList();
				//}
			}

			return newcs;
		}

		public void InsertSLO(SLO slo, string userid, out string error)
		{
			error = "";

			try
			{
				RepositoryContentIO.InsertSLO(slo);
			}
			catch (Exception e)
			{
				Logger.LogException(e);

				error = "An error has occurred while inserting the new SLO";
			}
		}

		public void UpdateSLO(SLO slo, string userid, out string error)
		{
			error = "";

			try
			{
				RepositoryContentIO.UpdateSLO(slo);
			}
			catch (Exception e)
			{
				Logger.LogException(e);

				error = "An error has occurred while updating the SLO";
			}
		}
	}
}
