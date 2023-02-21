using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System.Security.Claims;
using System.Text.Json;

namespace RecipeThesaurus
{
    public class RoleClaimAction : ClaimAction
    {
        private const string RoleClaimType = "roles";

        public RoleClaimAction() : base(RoleClaimType, ClaimValueTypes.String)
        {
        }

        public override void Run(JsonElement userData, ClaimsIdentity identity, string issuer)
        {
            //Map array of roles to separate role claims
            if (userData.TryGetProperty(RoleClaimType, out var roles))
            {
                if (roles.ValueKind == JsonValueKind.Array)
                {
                    var enumerator = roles.EnumerateArray();
                    while (enumerator.MoveNext())
                    {
                        AddRoleClaim(identity, enumerator.Current.ToString(), issuer);
                    }
                }
                else if (roles.ValueKind == JsonValueKind.String)
                {
                    AddRoleClaim(identity, roles.ToString(), issuer);
                }

            };
        }

        private void AddRoleClaim(ClaimsIdentity identity, string role, string issuer)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String, issuer));
        }
    }
}
