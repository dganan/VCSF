﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public class Category : CS2Object
	{
		[DataMember]
		public string Name { get; set; }
	}
}
