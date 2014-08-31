using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public abstract class ScenePart : SLOElement
	{
		// May be is not necessary because secuenciality is implemented in Scene property
		//ScenePart Next { get; set; }
		//ScenePart Previous { get; set; }

		private static long NextScenePartId;

		public override string NextSerial
		{
			get
			{
				NextScenePartId++;

				return (NextScenePartId - 1).ToString();
			}

			set
			{
				long lid;

				if (Int64.TryParse(value, out lid))
				{
					if (lid >= NextScenePartId)
					{
						NextScenePartId = lid + 1;
					}
				}
			}
		}
	}
}
