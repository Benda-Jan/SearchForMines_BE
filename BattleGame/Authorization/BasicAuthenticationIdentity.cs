using System;
using System.Security.Principal;

namespace BattleGame.Authorization
{
	public class BasicAuthenticationIdentity : IIdentity
	{
		public BasicAuthenticationIdentity(string? name)
		{
			AuthenticationType = "Basic";
			IsAuthenticated = true;
			Name = name;
		}

		public string? AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? Name { get; set; }

    }
}

