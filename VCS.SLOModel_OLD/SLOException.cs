using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS.SLOModel_OLD
{
	[DataContract(IsReference = true)]
	public class SLOException : Exception
	{
		public SLOException(string message)
			: base(message)
		{ 
		}
	}
}
