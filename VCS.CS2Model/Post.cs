using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public class Post : CS2Object
	{
		[DataMember]
		public UserAccount Creator { get; set; }

		[DataMember]
		public DateTime CreationDate { get; set; }

		[DataMember]
		public string Content { get; set; }

		[DataMember]
		public Post ReplyOf { get; set; }

		[DataMember]
		public List<Category> Categories { get; set; }

		public Post ()
		{
			Categories = new List<Category>();
		}
	}
}
