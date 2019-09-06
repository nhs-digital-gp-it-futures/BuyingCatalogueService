using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace NHSD.BuyingCatalogue.Application.Infrastructure.Authentication
{
    public interface IBearerAuthentication
    {
        Task OnTokenValidated(TokenValidatedContext context);
        Task OnMessageReceived(MessageReceivedContext context);
    }
}
