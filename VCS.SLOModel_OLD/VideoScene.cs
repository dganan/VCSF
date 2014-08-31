using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS.SLOModel_OLD
{
	[DataContract(IsReference = true)]
	public class VideoScene : Scene
	{
		public VideoScene()
		{
		}

		public override SceneType SceneType
		{
			get { return SceneType.Video; }
		}

		public override Scene Clone()
		{
			return null;
		}
	}
}
