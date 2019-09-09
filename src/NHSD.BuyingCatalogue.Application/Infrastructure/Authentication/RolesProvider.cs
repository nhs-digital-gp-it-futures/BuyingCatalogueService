using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Application.Infrastructure.Authentication
{
    public sealed class RolesProvider : IRolesProvider
    {
        public IEnumerable<string> RolesByEmail(string email)
        {
            // TODO     set roles based on email-->contact-->organisation-->org.PrimaryRoleId-->role
            return new[]
            {
                Roles.Public
            };
        }
    }
}
