using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCS
{
	[Serializable]
	public class VCSException : Exception
	{
		public string UserMessage { get; private set; }

		public bool Logged { get; set; }

		public VCSException(string message, string userMessage = null, bool log = true)
			: base(message)
		{
			if (userMessage != null)
			{
				UserMessage = userMessage;
			}
			else
			{
				UserMessage = "Unexpected error";
			}

			if (log)
			{
				Logger.LogException(this);
			}
		}

		public VCSException(Exception inner, string userMessage = null, bool log = true)
			: base(inner.Message, inner)
		{
			if (userMessage != null)
			{
				UserMessage = userMessage;
			}
			else
			{
				UserMessage = "Unexpected error";
			}

			if (log)
			{
				Logger.LogException(inner);
			}
		}

        public VCSException(string message, Exception inner, string userMessage = null, bool log = true)
            : base(message, inner)
        {
            if (userMessage != null)
            {
                UserMessage = userMessage;
            }
            else
            {
				UserMessage = "Unexpected error";
            }

            if (log)
            {
                Logger.LogException(this);
            }
        }
    }
}
