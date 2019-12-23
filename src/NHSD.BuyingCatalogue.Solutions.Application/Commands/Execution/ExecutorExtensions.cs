using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution
{
    internal static class ExecutorExtensions
    {
        internal static async Task<TResult> ExecuteIfValidAsync<T, TResult>(this IExecutor<T> executor, T request, IValidator<T, TResult> validator, CancellationToken cancellationToken) where TResult : IResult
        {
            var validationResult = validator.Validate(request);

            if (validationResult.IsValid)
            {
                await executor.UpdateAsync(request, cancellationToken).ConfigureAwait(false);
            }

            return validationResult;
        }
    }
}
