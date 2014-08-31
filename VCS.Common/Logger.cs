using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;

namespace VCS
{
	public static class Logger
	{
		private static string ficheroLog;

		// devuelve el path del fichero de log
		public static string FicheroLog
		{
			set { ficheroLog = value; } 
		}

		// Escribe una entrada de excepción en el fichero de log
		public static void LogException(Exception error)
		{
			if (error is VCSException && (error as VCSException).Logged)
			{
				return;
			}

			StreamWriter sw = null;

			try
			{
				sw = OpenLogFile();

				WriteLogHeader(sw);

				WriteLogEntry(sw, error.Message, error.StackTrace);

				if (error is VCSException && error.InnerException != null)
				{
					LogException(error.InnerException);
				}

				WriteLogFooter(sw);

				if (error is VCSException)
				{
					(error as VCSException).Logged = true;
				}
			}
			catch
			{
				// throw new VCSException("Error log is not correctly configured", null, false);
			}
			finally 
			{
				if (sw != null)
				{
					sw.Close();
				}
			}
		}

		private static StreamWriter OpenLogFile()
		{
			if (ficheroLog == null)
			{
				ficheroLog = ConfigurationManager.AppSettings["LogsDirectory"] + "\\VCS_" + DateTime.Today.ToString("yyyy_MM_dd") + ".log";
			}

			StreamWriter sw = new StreamWriter(new FileStream(ficheroLog, FileMode.Append));

			return sw;
		}

		private static void WriteLogFooter(StreamWriter sw)
		{
			sw.WriteLine();
			sw.WriteLine("============================================================");
			sw.WriteLine("======== {0} ", DateTime.Now);
			sw.WriteLine();
		}

		private static void WriteLogHeader(StreamWriter sw)
		{
			sw.WriteLine();
			sw.WriteLine("============================================================");
			sw.WriteLine();
		}

		// Escribe una entrada de mensaje en el fichero de log
		public static void LogMessage(string mensaje)
		{
			StreamWriter sw = null;

			try
			{
				sw = OpenLogFile();

				WriteLogHeader(sw);

				WriteLogEntry(sw, mensaje);

				WriteLogFooter(sw);
			}
			catch
			{
				// throw new VCSException("Error log is not correctly configured", null, false);
			}
			finally
			{
				if (sw != null)
				{
					sw.Close();
				}
			}
		}

		private static void WriteLogEntry(StreamWriter sw, params string[] errorLines)
		{
			foreach (string errorLine in errorLines)
			{
				sw.WriteLine(errorLine);
			}
		}
	}
}
