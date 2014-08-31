using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS.SLOModel_OLD
{
	[DataContract(IsReference = true)]
	public class DialogScene : Scene
	{
		[DataMember]
		public List<Character> Characters { get; set; }

		public void OrderCharacters ()
		{
			Characters = Characters.OrderBy(x => x.Name).ToList();
		}

		[DataMember]
		public List<DialogScenePart> DialogSceneParts { get; set; }

		public DialogScene() : base ()
		{
			Characters = new List<Character>();

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

			clone.Characters = this.Characters.Select(x => x.Clone()).ToList();

			clone.DialogSceneParts = this.DialogSceneParts.Select(x => x.Clone(this.Characters, clone.Characters)).ToList();

			return clone;
		}
	}
}
