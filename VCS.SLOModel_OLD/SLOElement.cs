using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS.SLOModel_OLD
{
	[DataContract(IsReference = true)]
	public class SLOElement
    {
		private string id;

		[DataMember]
		public string Id 
		{
			get { return id; }

			set 
			{
				id = value;

				NextSerial = value;				
			} 
		}

		public SLOElement()
		{
			this.Id = NextSerial;
		}

		public override bool Equals(object obj)
		{
			SLOElement sloe = obj as SLOElement;

			return sloe != null && this.GetType().Equals(sloe.GetType()) && this.Id == sloe.Id;
		}

		public virtual string NextSerial { get; set; }
    }
}
