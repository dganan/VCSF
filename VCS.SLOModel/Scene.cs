using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	[KnownType(typeof(DialogScene))]
	[KnownType(typeof(VideoScene))]
	[KnownType(typeof(AssessmentScene))]
	[KnownType(typeof(EmotionalTestScene))]
	[KnownType(typeof(ReferencesScene))]
	
	[XmlInclude(typeof(DialogScene))]
	[XmlInclude(typeof(VideoScene))]
	[XmlInclude(typeof(AssessmentScene))]
	[XmlInclude(typeof(EmotionalTestScene))]
	[XmlInclude(typeof(ReferencesScene))]
	public class Scene : SLOElement
	{
		private static long NextSceneId;

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int Order { get; set; }

		[DataMember]
		public virtual bool IsEndScene { get; set; }

		//public List<ScenePart> SceneParts { get; set; }

		// May be is not necessary because secuenciality is implemented in StoryBoard property
		//Scene Next { get; set; }
		//Scene Previous { get; set; }

		public Scene()
			: base()
		{
		}

		public virtual SceneType SceneType
		{
			get;
			private set;
		}

		public virtual Scene Clone()
		{
			return null;
		}

		public virtual bool CanBeManuallySetAsEndScene
		{
			get { return true; }
		}

		public virtual List<Scene> ScenesToJump { get { return new List<Scene>(); } }

		public override string NextSerial
		{
			get
			{
				NextSceneId++;

				return (NextSceneId - 1).ToString();
			}

			set
			{
				long lid;

				if (Int64.TryParse(value, out lid))
				{
					if (lid >= NextSceneId)
					{
						NextSceneId = lid + 1;
					}
				}
			}
		}

		public virtual List<Character> UsedCharacters
		{
			get 
			{
				return new List<Character>();
			}
		}
	}
}
