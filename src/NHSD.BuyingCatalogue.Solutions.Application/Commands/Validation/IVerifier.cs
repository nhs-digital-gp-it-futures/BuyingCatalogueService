using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal interface IVerifier<in T, TResult> where TResult : IResult
    {
        Task<TResult> VerifyAsync(T command);
    }
}
