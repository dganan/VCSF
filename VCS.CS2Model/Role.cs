using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VCS
{
	public enum Role 
	{
		Teacher,
		Student,
		Administrator,
	}

	public static class RoleExtension
	{
		public static string ToStringValue(this Role role)
		{
			return role.ToString();
		}

		public static Role ToRole(this string roleName)
		{
			Role role = Role.Student;

			Enum.TryParse<Role>(roleName, out role);

			return role;
		}
	}
}
