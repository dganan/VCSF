using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public class CS2Object
	{
		[DataMember]
		public string Id { get; set; }

        public override bool Equals(object obj)
        {
            CS2Object cs2o = obj as CS2Object;
            
            return cs2o != null && this.GetType().Equals(cs2o.GetType()) && this.Id == cs2o.Id;
        }
	}
}
