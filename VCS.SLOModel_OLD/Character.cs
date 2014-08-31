using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using VCS;

namespace VCS.SLOModel_OLD
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
			clone.PhotoAvatar = (this.PhotoAvatar == null? null : (byte[])this.PhotoAvatar.Clone());

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
	}
}
