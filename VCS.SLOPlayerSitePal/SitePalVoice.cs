using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace VCS
{
	public class SitePalVoice
	{
		public string Voice { get; set; }
		public string Language { get; set; }
		public string Engine { get; set; }

		public SitePalVoice(string voice, string language, string engine)
		{
			Voice = voice;
			Language = language;
			Engine = engine;
		}

		public string[] ToStringArray()
		{
			return new string[] { Voice.ToString(), Language.ToString(), Engine.ToString() };
		}

		// languages x gender
		private static int[,] nextVoice = new int[3, 2];

		private static SitePalVoice[,][] voices = new SitePalVoice[3, 2][]
		{
			{
				// ENGLISH
				new SitePalVoice [] { new SitePalVoice ("1","1","2"), new SitePalVoice ("4","1","2"), },
				new SitePalVoice [] { new SitePalVoice ("2","1","2"), new SitePalVoice ("5","1","2"), },
			},
			{
				// SPANISH
				new SitePalVoice [] { new SitePalVoice ("1","2","2"), new SitePalVoice ("9","2","2"), },
				new SitePalVoice [] { new SitePalVoice ("2","2","2"), new SitePalVoice ("6","2","2"), },
			},
			{
				// CATALAN
				new SitePalVoice [] { new SitePalVoice ("1","5","2"), new SitePalVoice ("3","5","2"), new SitePalVoice ("1","5","4"), },
				new SitePalVoice [] { new SitePalVoice ("2","5","2"), },
			},
		};

		private static Dictionary<CharacterLanguagePair, SitePalVoice> CharactersVoices { get; set; }

		static SitePalVoice()
		{
			CharactersVoices = new Dictionary<CharacterLanguagePair, SitePalVoice>();
		}

		public static SitePalVoice SitePalVoiceForCharacter(Character character, Language language)
		{
			CharacterLanguagePair key = new CharacterLanguagePair(character, language);

			if (CharactersVoices.ContainsKey(key))
			{
				return CharactersVoices[key];
			}
			else
			{
				int next = nextVoice[(int)language, (int)character.Gender];

				SitePalVoice voice = voices[(int)language, (int)character.Gender][next];

				next = (next + 1 % voices[(int)language, (int)character.Gender].Length);

				nextVoice[(int)language, (int)character.Gender] = next;

				CharactersVoices[key] = voice;

				return voice;
			}
		}
	}

	public class CharacterLanguagePair : Tuple<Character, Language>
	{
		public CharacterLanguagePair(Character character, Language language)
			: base(character, language)
		{
		}

		public override bool Equals(object obj)
		{
			CharacterLanguagePair clp = obj as CharacterLanguagePair;

			if (clp != null)
			{
				return this.Item1.Equals(clp.Item1) && this.Item2.Equals(clp.Item2);
			}

			return false;
		}

		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;

				// Suitable nullity checks etc, of course :)
				hash = hash * 23 + Item1.GetHashCode();
				hash = hash * 23 + Item2.GetHashCode();

				return hash;
			}
		}
	}

	//public class CharacterLanguagePairComparer : IEqualityComparer<CharacterLanguagePair>
	//{
	//    public bool Equals(CharacterLanguagePair x, CharacterLanguagePair y)
	//    {
	//        return x.Item1.Equals(y.Item1) && x.Item2.Equals(y.Item2);
	//    }

	//    public int GetHashCode(CharacterLanguagePair obj)
	//    {
	//        unchecked // Overflow is fine, just wrap
	//        {
	//            int hash = 17;

	//            // Suitable nullity checks etc, of course :)
	//            hash = hash * 23 + obj.Item1.GetHashCode();
	//            hash = hash * 23 + obj.Item2.GetHashCode();

	//            return hash;
	//        }
	//    }
	//}
}
