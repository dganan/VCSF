using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using ICSharpCode.SharpZipLib.BZip2;
using System.Text.RegularExpressions;

namespace VCS
{
	public static class Utils
	{
		public static byte[] DecompressBytes(byte[] compressedBytes)
		{
			MemoryStream ms = new MemoryStream(compressedBytes);

			MemoryStream ms2 = new MemoryStream();

			BZip2InputStream bzis = new BZip2InputStream(ms);

			byte[] bytesBuffer = new byte[bzis.Length];

			int i = bzis.Read(bytesBuffer, 0, bytesBuffer.Length);

			while (i > 0)
			{
				ms2.Write(bytesBuffer, 0, i);

				i = bzis.Read(bytesBuffer, 0, bytesBuffer.Length);
			}

			bzis.Close();

			return ms2.ToArray();
		}
	}
}
