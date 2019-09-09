using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Application.Infrastructure.Authentication
{
    public interface IRolesProvider
    {
        IEnumerable<string> RolesByEmail(string email);
    }
}
