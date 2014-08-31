using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;

namespace VCS
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ActivityLogService" in code, svc and config file together.
	public class ActivityLogService : IActivityLogService
	{
		public void LogActivity(Activity activity)
		{
			try
			{
				if (ConfigurationManager.AppSettings["LogActivity"].ToString() == "true")
				{
					using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ActivityLogDB"].ConnectionString))
					{
						conn.Open();

						using (SqlCommand comm = new SqlCommand("INSERT INTO ActivityLog ([ip], [userName], [action], [date]) VALUES (@ip, @userName, @action, @date); SELECT SCOPE_IDENTITY()", conn))
						{
							comm.Parameters.Add(new SqlParameter("ip", activity.IP));
							comm.Parameters.Add(new SqlParameter("userName", activity.UserName));
							comm.Parameters.Add(new SqlParameter("action", activity.Action));
							comm.Parameters.Add(new SqlParameter("date", activity.Date));

							int id = Int32.Parse(comm.ExecuteScalar().ToString());

							if (activity.Arguments != null)
							{
								foreach (Argument arg in activity.Arguments)
								{
									using (SqlCommand comm2 = new SqlCommand("INSERT INTO Arguments ([id_activity], [key], [value]) VALUES (@id_activity, @key, @value)", conn))
									{
										comm2.Parameters.Add(new SqlParameter("id_activity", id));
										comm2.Parameters.Add(new SqlParameter("key", arg.Key));
										comm2.Parameters.Add(new SqlParameter("value", arg.Value));

										comm2.ExecuteNonQuery();
									}
								}
							}
						}

						conn.Close();
					}
				}
			}
			catch (Exception e)
			{
				Logger.LogException(e);
			}
		}
	}
}
