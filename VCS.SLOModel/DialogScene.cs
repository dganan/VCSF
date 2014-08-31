using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public class DialogScene : Scene
	{
		[DataMember]
		public List<DialogScenePart> DialogSceneParts { get; set; }

		public DialogScene() : base ()
		{
			DialogSceneParts = new List<DialogScenePart>();
		}

		public override SceneType SceneType
		{
			get { return SceneType.Dialog; }
		}

		public override Scene Clone()
		{
			DialogScene clone = new DialogScene();

			clone.Name = this.Name;

			clone.DialogSceneParts = this.DialogSceneParts.Select(x => x.Clone()).ToList();

			return clone;
		}

		public override List<Character> UsedCharacters
		{
			get
			{
				return this.DialogSceneParts.Select(x => x.Character).Distinct().ToList();
			}
		}
	}
}
