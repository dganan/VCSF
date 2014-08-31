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
using System.Collections.Generic;

namespace VCS
{
	public class UserInfo
	{
		public string Name { get; set; }

		public string Ip { get; set; }

		public string Id { get; set; }

		public List<UserEmotionalInfo> EmotionalInfo { get; set; }

		public List<UserAssessmentInfo> AssessmentInfo { get; set; }

		public UserInfo()
		{ 
			EmotionalInfo = new List<UserEmotionalInfo>();

			AssessmentInfo = new List<UserAssessmentInfo>();
		}

		public bool MeetsPrerequisites { get; set; }

		public string CourseId { get; set; }
	}
}
