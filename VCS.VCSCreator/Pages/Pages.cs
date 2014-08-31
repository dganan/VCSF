using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace VCS
{
	public static class Pages
	{
		public static Uri AvailableCollaborativeSessionsListPageUri
		{
			get
			{
				return new Uri("/Pages/" + typeof(AvailableCollaborativeSessionsListPage).Name + ".xaml", UriKind.Relative);
			}
		}

		public static Uri SLOPlayerPage(string id)
		{
			string uri = Application.Current.Host.Source.AbsoluteUri;

			uri = uri.Substring(0, uri.LastIndexOf("/ClientBin"));

			return new Uri(uri + "/SLOPlayer.aspx?SLOId=" + id + "&User=" + VCSCreator.UserName + "&UserId=" + VCSCreator.UserId, UriKind.Absolute);
		}

		public static Uri SLOEditorPage(string id)
		{
			string uri = Application.Current.Host.Source.AbsoluteUri;

			uri = uri.Substring(0, uri.LastIndexOf("/ClientBin"));

			return new Uri(uri + "/SLOEditor.aspx?SLOId=" + id + "&User=" + VCSCreator.UserName + "&UserId=" + VCSCreator.UserId, UriKind.Absolute);
		}

		public static Uri CreateSLOPageUri
		{
			get
			{
				return new Uri("/Pages/" + typeof(CreateSLOPage).Name + ".xaml", UriKind.Relative);
			}
		}
	}
}
