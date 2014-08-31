using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS.SLOModel_OLD
{
	[DataContract(IsReference = true)]
	public class DialogScenePart : ScenePart
	{
		[DataMember]
		public Character Character { get; set; }

		[DataMember]
		public Emoticon EmotionalState { get; set; }

		[DataMember]
		public List<DialogSpecialMark> SpecialMarks { get; set; }

		[DataMember]
		public List<string> SpeechActs { get; set; }

		[DataMember]
		public List<string> Keywords { get; set; }

		[DataMember]
		public string Speech { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public Language Language { get; set; }

		[DataMember]
		public byte [] SpeechAudio { get; set; }

		public DialogScenePart()
		{
			SpeechActs = new List<string>();

			Keywords = new List<string>();

			EmotionalState = Emoticon.None;

			SpecialMarks = new List<DialogSpecialMark>();
		}

		public DialogScenePart Clone(List<Character> originalCharacters, List<Character> clonedCharacters)
		{
			DialogScenePart clone = new DialogScenePart();

			//clone.Id = this.Id;
			clone.Name = this.Name;
			clone.EmotionalState = this.EmotionalState;
			clone.SpecialMarks = this.SpecialMarks;

			clone.SpeechActs = this.SpeechActs.Select(x => x).ToList();

			clone.Keywords = this.Keywords.Select(x => x).ToList();
				
			clone.Speech = this.Speech;
			clone.SpeechAudio = (this.SpeechAudio == null? null : (byte[])this.SpeechAudio.Clone());
			clone.Language = this.Language;

			clone.Character = clonedCharacters[originalCharacters.IndexOf(this.Character)];

			return clone;
		}
	}
}
