using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution
{
    internal static class ExecutorExtensions
    {
        internal static async Task<TResult> ExecuteIfValidAsync<T, TResult>(
            this IExecutor<T> executor,
            T request,
            IValidator<T, TResult> validator,
            IVerifier<T, TResult> verifier,
            CancellationToken cancellationToken)
            where TResult : IResult
        {
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            if (verifier is not null)
            {
                var verifierResult = await verifier.VerifyAsync(request);

                if (!verifierResult.IsValid)
                {
                    return verifierResult;
                }
            }

            await executor.UpdateAsync(request, cancellationToken);

            return validationResult;
        }
    }
}
