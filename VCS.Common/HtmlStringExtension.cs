using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace VCS
{
	public static class HtmlStringExtension
	{
		public static string ToSafeHtmlString(this string s)
		{
			return s.Replace("<", "#lt").Replace(">", "#gt");
		}

		public static string FromSafeHtmlString(this string s)
		{
			return s.Replace("#lt", "<").Replace("#gt", ">");
		}

		public static string RemoveHTMLTags(this string text)
		{
			text = text.Replace("</p>", ". ");
			text = text.Replace("..", ".");
			text = Regex.Replace(text, @"<(.|\n)*?>", string.Empty);

			return text.DecodeHTMLSpecialChars();
		}

		public static string RemoveQuotes(this string text)
		{
			return Regex.Replace(text, @"[[]quote=(.|\n)*[]](.|\n)*[[]/quote[]]", string.Empty);
		}

		public static string DecodeHTMLSpecialChars(this string text)
		{
			var symbols = HTMLSymbols.Where(x => text.Contains(x.Item1));

			foreach (Tuple<string, string> t in symbols)
			{
				text = text.Replace(t.Item1, t.Item2);
			}

			return text;
		}

		public static string EncodeHTMLSpecialChars(this string text)
		{
			var symbols = SpecialChars.Where(x => text.Contains(x.Item2));

			foreach (Tuple<string, string> t in symbols)
			{
				text = text.Replace(t.Item2, t.Item1);
			}

			return text;
		}

		public static Tuple<string, string>[] SpecialChars = new Tuple<string, string>[]
		{
			new Tuple<string, string> ("&aacute;", "á"),
			new Tuple<string, string> ("&Aacute;", "Á"),
			new Tuple<string, string> ("&agrave;", "à"),
			new Tuple<string, string> ("&Agrave;", "À"),
			new Tuple<string, string> ("&acirc;", "â"),
			new Tuple<string, string> ("&Acirc;", "Â"),
			new Tuple<string, string> ("&aring;", "å"),
			new Tuple<string, string> ("&Aring;", "Å"),
			new Tuple<string, string> ("&atilde;", "ã"),
			new Tuple<string, string> ("&Atilde;", "Ã"),
			new Tuple<string, string> ("&auml;", "ä"),
			new Tuple<string, string> ("&Auml;", "Ä"),
			new Tuple<string, string> ("&aelig;", "æ"),
			new Tuple<string, string> ("&AElig;", "Æ"),
			new Tuple<string, string> ("&ccedil;", "ç"),
			new Tuple<string, string> ("&Ccedil;", "Ç"),
			new Tuple<string, string> ("&eacute;", "é"),
			new Tuple<string, string> ("&Eacute;", "É"),
			new Tuple<string, string> ("&egrave;", "è"),
			new Tuple<string, string> ("&Egrave;", "È"),
			new Tuple<string, string> ("&ecirc;", "ê"),
			new Tuple<string, string> ("&Ecirc;", "Ê"),
			new Tuple<string, string> ("&euml;", "ë"),
			new Tuple<string, string> ("&Euml;", "Ë"),
			new Tuple<string, string> ("&iacute;", "í"),
			new Tuple<string, string> ("&Iacute;", "Í"),
			new Tuple<string, string> ("&igrave;", "ì"),
			new Tuple<string, string> ("&Igrave;", "Ì"),
			new Tuple<string, string> ("&icirc;", "î"),
			new Tuple<string, string> ("&Icirc;", "Î"),
			new Tuple<string, string> ("&iuml;", "ï"),
			new Tuple<string, string> ("&Iuml;", "Ï"),
			new Tuple<string, string> ("&ntilde;", "ñ"),
			new Tuple<string, string> ("&Ntilde;", "Ñ"),
			new Tuple<string, string> ("&oacute;", "ó"),
			new Tuple<string, string> ("&Oacute;", "Ó"),
			new Tuple<string, string> ("&ograve;", "ò"),
			new Tuple<string, string> ("&Ograve;", "Ò"),
			new Tuple<string, string> ("&ocirc;", "ô"),
			new Tuple<string, string> ("&Ocirc;", "Ô"),
			new Tuple<string, string> ("&oslash;", "ø"),
			new Tuple<string, string> ("&Oslash;", "Ø"),
			new Tuple<string, string> ("&otilde;", "õ"),
			new Tuple<string, string> ("&Otilde;", "Õ"),
			new Tuple<string, string> ("&ouml;", "ö"),
			new Tuple<string, string> ("&Ouml;", "Ö"),
			new Tuple<string, string> ("&szlig;", "ß"),
			new Tuple<string, string> ("&uacute;", "ú"),
			new Tuple<string, string> ("&Uacute;", "Ú"),
			new Tuple<string, string> ("&ugrave;", "ù"),
			new Tuple<string, string> ("&Ugrave;", "Ù"),
			new Tuple<string, string> ("&ucirc;", "û"),
			new Tuple<string, string> ("&Ucirc;", "Û"),
			new Tuple<string, string> ("&uuml;", "ü"),
			new Tuple<string, string> ("&Uuml;", "Ü"),
			new Tuple<string, string> ("&yuml;", "ÿ"),
		};

		public static Tuple<string, string>[] HTMLSymbols = new Tuple<string, string>[]
		{
			new Tuple<string, string> ("&ndash;", "–"),
			new Tuple<string, string> ("&mdash;", "—"),
			new Tuple<string, string> ("&iexcl;", "¡"),
			new Tuple<string, string> ("&iquest;", "¿"),
			new Tuple<string, string> ("&quot;", "\""),
			new Tuple<string, string> ("&ldquo;", "“"),
			new Tuple<string, string> ("&rdquo;", "”"),
			new Tuple<string, string> ("&lsquo;", "‘"),
			new Tuple<string, string> ("&rsquo;", "’"),
			new Tuple<string, string> ("&laquo;", "«"),
			new Tuple<string, string> ("&raquo;", "»"),
			new Tuple<string, string> ("&nbsp;", " "),
			new Tuple<string, string> ("&amp;", "&"),
			new Tuple<string, string> ("&cent;", "¢"),
			new Tuple<string, string> ("&copy;", "©"),
			new Tuple<string, string> ("&divide;", "÷"),
			new Tuple<string, string> ("&gt;", ">"),
			new Tuple<string, string> ("&lt;", "<"),
			new Tuple<string, string> ("&micro;", "µ"),
			new Tuple<string, string> ("&middot;", "·"),
			new Tuple<string, string> ("&para;", "¶"),
			new Tuple<string, string> ("&plusmn;", "±"),
			new Tuple<string, string> ("&euro;", "€"),
			new Tuple<string, string> ("&pound;", "£"),
			new Tuple<string, string> ("&reg;", "®"),
			new Tuple<string, string> ("&sect;", "§"),
			new Tuple<string, string> ("&trade;", "™"),
			new Tuple<string, string> ("&yen;", "¥"),
			new Tuple<string, string> ("&aacute;", "á"),
			new Tuple<string, string> ("&Aacute;", "Á"),
			new Tuple<string, string> ("&agrave;", "à"),
			new Tuple<string, string> ("&Agrave;", "À"),
			new Tuple<string, string> ("&acirc;", "â"),
			new Tuple<string, string> ("&Acirc;", "Â"),
			new Tuple<string, string> ("&aring;", "å"),
			new Tuple<string, string> ("&Aring;", "Å"),
			new Tuple<string, string> ("&atilde;", "ã"),
			new Tuple<string, string> ("&Atilde;", "Ã"),
			new Tuple<string, string> ("&auml;", "ä"),
			new Tuple<string, string> ("&Auml;", "Ä"),
			new Tuple<string, string> ("&aelig;", "æ"),
			new Tuple<string, string> ("&AElig;", "Æ"),
			new Tuple<string, string> ("&ccedil;", "ç"),
			new Tuple<string, string> ("&Ccedil;", "Ç"),
			new Tuple<string, string> ("&eacute;", "é"),
			new Tuple<string, string> ("&Eacute;", "É"),
			new Tuple<string, string> ("&egrave;", "è"),
			new Tuple<string, string> ("&Egrave;", "È"),
			new Tuple<string, string> ("&ecirc;", "ê"),
			new Tuple<string, string> ("&Ecirc;", "Ê"),
			new Tuple<string, string> ("&euml;", "ë"),
			new Tuple<string, string> ("&Euml;", "Ë"),
			new Tuple<string, string> ("&iacute;", "í"),
			new Tuple<string, string> ("&Iacute;", "Í"),
			new Tuple<string, string> ("&igrave;", "ì"),
			new Tuple<string, string> ("&Igrave;", "Ì"),
			new Tuple<string, string> ("&icirc;", "î"),
			new Tuple<string, string> ("&Icirc;", "Î"),
			new Tuple<string, string> ("&iuml;", "ï"),
			new Tuple<string, string> ("&Iuml;", "Ï"),
			new Tuple<string, string> ("&ntilde;", "ñ"),
			new Tuple<string, string> ("&Ntilde;", "Ñ"),
			new Tuple<string, string> ("&oacute;", "ó"),
			new Tuple<string, string> ("&Oacute;", "Ó"),
			new Tuple<string, string> ("&ograve;", "ò"),
			new Tuple<string, string> ("&Ograve;", "Ò"),
			new Tuple<string, string> ("&ocirc;", "ô"),
			new Tuple<string, string> ("&Ocirc;", "Ô"),
			new Tuple<string, string> ("&oslash;", "ø"),
			new Tuple<string, string> ("&Oslash;", "Ø"),
			new Tuple<string, string> ("&otilde;", "õ"),
			new Tuple<string, string> ("&Otilde;", "Õ"),
			new Tuple<string, string> ("&ouml;", "ö"),
			new Tuple<string, string> ("&Ouml;", "Ö"),
			new Tuple<string, string> ("&szlig;", "ß"),
			new Tuple<string, string> ("&uacute;", "ú"),
			new Tuple<string, string> ("&Uacute;", "Ú"),
			new Tuple<string, string> ("&ugrave;", "ù"),
			new Tuple<string, string> ("&Ugrave;", "Ù"),
			new Tuple<string, string> ("&ucirc;", "û"),
			new Tuple<string, string> ("&Ucirc;", "Û"),
			new Tuple<string, string> ("&uuml;", "ü"),
			new Tuple<string, string> ("&Uuml;", "Ü"),
			new Tuple<string, string> ("&yuml;", "ÿ"),
			new Tuple<string, string> ("&#8211;", "–"),
			new Tuple<string, string> ("&#8212;", "—"),
			new Tuple<string, string> ("&#161;", "¡"),
			new Tuple<string, string> ("&#191;", "¿"),
			new Tuple<string, string> ("&#34;", "\""),
			new Tuple<string, string> ("&#8220;", "“"),
			new Tuple<string, string> ("&#8221;", "”"),
			new Tuple<string, string> ("&#8216;", "‘"),
			new Tuple<string, string> ("&#8217;", "’"),
			new Tuple<string, string> ("&#171;", "«"),
			new Tuple<string, string> ("&#187;", "»"),
			new Tuple<string, string> ("&#40;", "("),
			new Tuple<string, string> ("&#41;", ")"),
			new Tuple<string, string> ("&#160", " "),
			new Tuple<string, string> ("&#38;", "&"),
			new Tuple<string, string> ("&#39;", "'"),
			new Tuple<string, string> ("&#162;", "¢"),
			new Tuple<string, string> ("&#169;", "©"),
			new Tuple<string, string> ("&#247;", "÷"),
			new Tuple<string, string> ("&#62;", ">"),
			new Tuple<string, string> ("&#60;", "<"),
			new Tuple<string, string> ("&#181;", "µ"),
			new Tuple<string, string> ("&#183;", "·"),
			new Tuple<string, string> ("&#182;", "¶"),
			new Tuple<string, string> ("&#177;", "±"),
			new Tuple<string, string> ("&#8364;", "€"),
			new Tuple<string, string> ("&#163;", "£"),
			new Tuple<string, string> ("&#174;", "®"),
			new Tuple<string, string> ("&#167;", "§"),
			new Tuple<string, string> ("&#153;", "™"),
			new Tuple<string, string> ("&#165;", "¥"),
			new Tuple<string, string> ("&#225;", "á"),
			new Tuple<string, string> ("&#193;", "Á"),
			new Tuple<string, string> ("&#224;", "à"),
			new Tuple<string, string> ("&#192;", "À"),
			new Tuple<string, string> ("&#226;", "â"),
			new Tuple<string, string> ("&#194;", "Â"),
			new Tuple<string, string> ("&#229;", "å"),
			new Tuple<string, string> ("&#197;", "Å"),
			new Tuple<string, string> ("&#227;", "ã"),
			new Tuple<string, string> ("&#195;", "Ã"),
			new Tuple<string, string> ("&#228;", "ä"),
			new Tuple<string, string> ("&#196;", "Ä"),
			new Tuple<string, string> ("&#230;", "æ"),
			new Tuple<string, string> ("&#198;", "Æ"),
			new Tuple<string, string> ("&#231;", "ç"),
			new Tuple<string, string> ("&#199;", "Ç"),
			new Tuple<string, string> ("&#233;", "é"),
			new Tuple<string, string> ("&#201;", "É"),
			new Tuple<string, string> ("&#232;", "è"),
			new Tuple<string, string> ("&#200;", "È"),
			new Tuple<string, string> ("&#234;", "ê"),
			new Tuple<string, string> ("&#202;", "Ê"),
			new Tuple<string, string> ("&#235;", "ë"),
			new Tuple<string, string> ("&#203;", "Ë"),
			new Tuple<string, string> ("&#237;", "í"),
			new Tuple<string, string> ("&#205;", "Í"),
			new Tuple<string, string> ("&#236;", "ì"),
			new Tuple<string, string> ("&#204;", "Ì"),
			new Tuple<string, string> ("&#238;", "î"),
			new Tuple<string, string> ("&#206;", "Î"),
			new Tuple<string, string> ("&#239;", "ï"),
			new Tuple<string, string> ("&#207;", "Ï"),
			new Tuple<string, string> ("&#241;", "ñ"),
			new Tuple<string, string> ("&#209;", "Ñ"),
			new Tuple<string, string> ("&#243;", "ó"),
			new Tuple<string, string> ("&#211;", "Ó"),
			new Tuple<string, string> ("&#242;", "ò"),
			new Tuple<string, string> ("&#210;", "Ò"),
			new Tuple<string, string> ("&#244;", "ô"),
			new Tuple<string, string> ("&#212;", "Ô"),
			new Tuple<string, string> ("&#248;", "ø"),
			new Tuple<string, string> ("&#216;", "Ø"),
			new Tuple<string, string> ("&#245;", "õ"),
			new Tuple<string, string> ("&#213;", "Õ"),
			new Tuple<string, string> ("&#246;", "ö"),
			new Tuple<string, string> ("&#214;", "Ö"),
			new Tuple<string, string> ("&#223;", "ß"),
			new Tuple<string, string> ("&#250;", "ú"),
			new Tuple<string, string> ("&#218;", "Ú"),
			new Tuple<string, string> ("&#249;", "ù"),
			new Tuple<string, string> ("&#217;", "Ù"),
			new Tuple<string, string> ("&#251;", "û"),
			new Tuple<string, string> ("&#219;", "Û"),
			new Tuple<string, string> ("&#252;", "ü"),
			new Tuple<string, string> ("&#220;", "Ü"),
			new Tuple<string, string> ("&#255;", "ÿ"),
		};

	}
}
