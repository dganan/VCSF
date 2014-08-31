using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public class ReferencesScene : Scene
	{
		[DataMember]
		public string SpeechText { get; set; }

		[DataMember]
		public Character Character { get; set; }

		[DataMember]
		public List<ReferencesSceneReference> References { get; set; }

		public string Speech
		{
			get
			{
				if (SpeechText != null)
				{
					return SpeechText.Trim().RemoveHTMLTags() + ". ";
				}
				else
				{
					return "";
				}
			}
		}

		[DataMember]
		public byte[] SpeechAudio { get; set; }

		public ReferencesScene()
			: base()
		{
			References = new List<ReferencesSceneReference>();
		}

		public override SceneType SceneType
		{
			get { return SceneType.References; }
		}

		public override Scene Clone()
		{
			ReferencesScene clone = new ReferencesScene();

			clone.Name = this.Name;

			clone.References = this.References.Select(x => x.Clone()).ToList();

			clone.SpeechText = this.SpeechText;
			clone.Character = this.Character.Clone();

			return clone;
		}

		public override bool CanBeManuallySetAsEndScene
		{
			get { return true; }
		}
	}
}
