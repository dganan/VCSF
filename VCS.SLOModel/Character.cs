using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	[DataContract(IsReference = true)]
	public partial class Character : SLOElement
	{
		private static long NextCharacterId;

		//[DataMember]
		//public string UserId { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public AnimationAvatar AnimationAvatar { get; set; }

		[DataMember]
		public byte[] PhotoAvatar { get; set; }

		[DataMember]
		public bool UseAnimatedAvatar { get; set; }

		//[DataMember]
		//public string VocalTimbre { get; set; }

		[DataMember]
		public Gender Gender { get; set; }

		[DataMember]
		public Age Age { get; set; }

		[DataMember]
		public double? Activity { get; set; }

		[DataMember]
		public double? Quality { get; set; }

        [DataMember]
        public double? Passivity { get; set; }

        [DataMember]
        public double? SocialNetwork { get; set; }

		private static double? activityWeight;
		private static double? qualityWeight;
        private static double? passivityWeight;
        private static double? socialNetworkWeight;

		public static double? ActivityWeight
		{
			get
			{
				if (activityWeight == null)
				{
					return 1 / 4;
				}

				return activityWeight.Value;
			}

			set { activityWeight = value; }
		}

		public static double? QualityWeight
		{
			get
			{
				if (qualityWeight == null)
				{
					return 1 / 4;
				}

				return qualityWeight.Value;
			}

			set { qualityWeight = value; }
		}

        public static double? PassivityWeight
        {
            get
            {
                if (passivityWeight == null)
                {
                    return 1 / 4;
                }

                return passivityWeight.Value;
            }

            set { passivityWeight = value; }
        }

        public static double? SocialNetworkWeight
        {
            get
            {
                if (socialNetworkWeight == null)
                {
                    return 1 / 4;
                }

                return socialNetworkWeight.Value;
            }

            set { socialNetworkWeight = value; }
        }

		static Character()
		{
			ActivityWeight = 1 / 4;
			QualityWeight = 1 / 4;
            PassivityWeight = 1 / 4;
            SocialNetworkWeight = 1 / 4;
        }

		//public override bool Equals(object obj)
		//{
		//    Character c = obj as Character;

		//    if (c == null) return false;

		//    return c.Id == this.Id;
		//}

		public Character Clone()
		{
			Character clone = new Character();

			clone.Name = this.Name;
			//clone.UserId = this.UserId;
			clone.AnimationAvatar = this.AnimationAvatar;
			clone.PhotoAvatar = (this.PhotoAvatar == null ? null : (byte[])this.PhotoAvatar.Clone());

			clone.UseAnimatedAvatar = this.UseAnimatedAvatar;
			clone.Gender = this.Gender;
			clone.Age = this.Age;

			return clone;
		}

		public override string NextSerial
		{
			get
			{
				NextCharacterId++;

				return (NextCharacterId - 1).ToString();
			}

			set
			{
				long lid;

				if (Int64.TryParse(value, out lid))
				{
					if (lid >= NextCharacterId)
					{
						NextCharacterId = lid + 1;
					}
				}
			}
		}

        public static double? IndicatorsToPunctuation(double? activity, double? quality, double? passivity, double? socialNetwork)
		{
            if (activity == null || quality == null || passivity == null || socialNetwork == null)
			{
				return null;
			}

            return (ActivityWeight * activity) + (QualityWeight * quality) + (PassivityWeight * (10 - passivity)) + (SocialNetworkWeight * socialNetwork);
		}

		public double? Punctuation
		{
			get
			{
				return IndicatorsToPunctuation(Activity, Quality, Passivity, SocialNetwork);
			}
		}

		public IndicatorColor ActivityColor
		{
			get
			{
				return ActivityValueToColor(Activity);
			}
		}

		public static IndicatorColor ActivityValueToColor(double? activity)
		{
			if (activity == null)
			{
				return IndicatorColor.None;
			}
			else if (activity >= 8)
			{
				return IndicatorColor.Green;
			}
			else if (activity >= 4)
			{
				return IndicatorColor.Yellow;
			}

			return IndicatorColor.Red;
		}

		public IndicatorColor QualityColor
		{
			get
			{
				return QualityValueToColor(Quality);
			}
		}

		public static IndicatorColor QualityValueToColor(double? quality)
		{
			if (quality == null)
			{
				return IndicatorColor.None;
			}
			else if (quality >= 6.5)
			{
				return IndicatorColor.Green;
			}
			else if (quality >= 4.5)
			{
				return IndicatorColor.Yellow;
			}

			return IndicatorColor.Red;
		}

		public IndicatorColor PassivityColor
		{
			get
			{
				return PassivityValueToColor(Passivity);
			}
		}

		public static IndicatorColor PassivityValueToColor(double? passivity)
		{
			if (passivity == null)
			{
				return IndicatorColor.None;
			}
			else if (passivity >= 5)
			{
				return IndicatorColor.Red;
			}
			else if (passivity >= 2)
			{
				return IndicatorColor.Yellow;
			}

			return IndicatorColor.Green;
		}

        public IndicatorColor SocialNetworkColor
        {
            get
            {
                return SocialNetworkValueToColor(SocialNetwork);
            }
        }

        public static IndicatorColor SocialNetworkValueToColor(double? socialNetwork)
        {
            if (socialNetwork == null)
            {
                return IndicatorColor.None;
            }
            else if (socialNetwork >= 5)
            {
                return IndicatorColor.Red;
            }
            else if (socialNetwork >= 2)
            {
                return IndicatorColor.Yellow;
            }

            return IndicatorColor.Green;
        }
    }

	public enum IndicatorColor
	{
		None,
		Green,
		Yellow,
		Red,
	}
}
