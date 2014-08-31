using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GOPISLOsRescue
{
	class Program
	{
		static void Main(string[] args)
		{
			StreamReader srOK = new StreamReader(@"C:\VCS\SLORepository\5_.slo");
			StreamReader srKO = new StreamReader(@"C:\VCS\SLORepository\5.slo");

			StreamWriter swNEW = new StreamWriter(@"C:\VCS\SLORepository\new.slo");

			string lineOK = srOK.ReadLine();
			string lineKO = srKO.ReadLine();

			bool ok = true;
			int lines = 6000;
			int line = 0;

			while (ok && line < lines && lineOK != null)
			{
				ok = ProcessLine(lineOK, lineKO, swNEW);

				lineOK = srOK.ReadLine();
				lineKO = srKO.ReadLine();
				line++;
			}

			if (ok)
			{
				while (lineOK != null)
				{
					swNEW.WriteLine(lineOK);
					lineOK = srOK.ReadLine();
				}
			}
			
			srOK.Close();
			srKO.Close();
			swNEW.Close();
		}

		private static bool ProcessLine(string lineOK, string lineKO, StreamWriter swNEW)
		{
			if (lineOK.Trim() == lineKO.Trim())
			{
				swNEW.WriteLine(lineOK);
			}			
			else if (String.IsNullOrWhiteSpace(lineOK) || String.IsNullOrWhiteSpace(lineKO))
			{
				return false;
			}
			else if (lineOK.Trim().StartsWith("<Id z:Id="))
			{
				if (lineKO.Trim().StartsWith("<Id z:Id="))
				{
					swNEW.WriteLine(lineKO);
				}
				else
				{
					return false;
				}
			}
			else if (lineOK.Trim().StartsWith("<Character z:Ref="))
			{
				if (lineKO.Trim().StartsWith("<Character z:Ref="))
				{
					swNEW.WriteLine(lineKO);
				}
				else
				{
					return false;
				}
			}
			else if (lineOK.Trim().StartsWith("<Language>"))
			{
				if (lineKO.Trim().StartsWith("<Language>"))
				{
					swNEW.WriteLine(lineKO);
				}
				else
				{
					return false;
				}
			}
			else if (lineOK.Trim().StartsWith("<Name z:Id="))
			{
				if (lineKO.Trim().StartsWith("<Name z:Id="))
				{
					swNEW.WriteLine(lineKO);
				}
				else
				{
					return false;
				}
			}
			else if (lineOK.Trim().StartsWith("<Speech z:Id="))
			{
				if (lineKO.Trim().StartsWith("<Speech z:Id="))
				{
					swNEW.WriteLine(lineKO);
				}
				else
				{
					return false;
				}
			}
			else 
			{
				if (lineOK.Trim().Length < 10 || lineKO.Trim().Length < 10)
				{
					return false;
				}

				if (lineOK.Trim().Substring(0, 10) == lineKO.Trim().Substring(0, 10))
				{
					swNEW.WriteLine(lineKO);
				}
				else 
				{
					return false;
				}
			}

			return true;
		}
	}
}
