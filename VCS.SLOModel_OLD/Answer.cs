using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS.SLOModel_OLD
{
	[DataContract(IsReference = true)]
	public class Answer : SLOElement
	{
		private static long NextAnswerId;

		[DataMember]
		public string AnswerText { get; set; }

		[DataMember]
		public bool IsCorrectAnswer { get; set; }

		public Answer Clone()
		{
			Answer clone = new Answer();

			clone.AnswerText = this.AnswerText;
			clone.IsCorrectAnswer = this.IsCorrectAnswer;

			return clone;
		}

		public override string NextSerial
		{
			get
			{
				NextAnswerId++;

				return (NextAnswerId - 1).ToString();
			}

			set
			{
				long lid;

				if (Int64.TryParse(value, out lid))
				{
					if (lid >= NextAnswerId)
					{
						NextAnswerId = lid + 1;
					}
				}
			}
		}
	}
}