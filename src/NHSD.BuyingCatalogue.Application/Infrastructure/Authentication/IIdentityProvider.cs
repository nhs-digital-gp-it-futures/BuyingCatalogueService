using System.Security.Claims;

namespace NHSD.BuyingCatalogue.Application.Infrastructure.Authentication
{
    public interface IIdentityProvider
    {
        ClaimsPrincipal Identity { get; }
    }
}
