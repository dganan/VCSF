using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public class SLO : SLOElement
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string SourceUrl { get; set; }

		[DataMember]
		public List<Scene> Scenes { get; set; }

		[DataMember]
		public List<Character> Characters { get; set; }

		[DataMember]
		public double? ActivityWeight
		{
			get { return Character.ActivityWeight; }
			set { Character.ActivityWeight = value; }
		}

		[DataMember]
		public double? QualityWeight 
		{
			get { return Character.QualityWeight; }
			set { Character.QualityWeight = value; }
		}

        [DataMember]
        public double? PassivityWeight
        {
            get { return Character.PassivityWeight; }
            set { Character.PassivityWeight = value; }
        }

        [DataMember]
        public double? SocialNetworkWeight
        {
            get { return Character.SocialNetworkWeight; }
            set { Character.SocialNetworkWeight = value; }
        }

		public void OrderCharacters()
		{
			Characters = Characters.OrderBy(x => x.Name).ToList();
		}

		public SLO()
		{
			Scenes = new List<Scene>();

			Characters = new List<Character>();
		}

		public Character WorstCharacter
		{
			get
			{
				return Characters.Where(x => x.Punctuation != null).OrderBy(x => x.Punctuation).FirstOrDefault();
			}
		}

		public Character BestCharacter
		{
			get
			{
				return Characters.Where(x => x.Punctuation != null).OrderByDescending(x => x.Punctuation).FirstOrDefault();
			}
		}
	}
}
