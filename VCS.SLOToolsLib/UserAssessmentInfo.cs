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
	public class UserAssessmentInfo
	{
		public int Scene { get; set; }

		public List<QuestionResult> QuestionsResults { get; set; }

		public double Score { get; set; }

		public DateTime When { get; set; }

		public UserAssessmentInfo()
		{
			QuestionsResults = new List<QuestionResult>();
		}
	}
}
