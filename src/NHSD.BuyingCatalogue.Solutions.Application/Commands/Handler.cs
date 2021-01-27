using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands
{
    internal class Handler<T, TResult> : IRequestHandler<T, TResult>
        where TResult : IResult
        where T : IRequest<TResult> // MediatR requires this
    {
        private readonly IExecutor<T> executor;
        private readonly IValidator<T, TResult> validator;
        private readonly IVerifier<T, TResult> verifier;

        public Handler(IExecutor<T> executor, IValidator<T, TResult> validator, IVerifier<T, TResult> verifier = null)
        {
            this.executor = executor;
            this.validator = validator;
            this.verifier = verifier;
        }

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="request">The command parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this command.</returns>
        public async Task<TResult> Handle(T request, CancellationToken cancellationToken) =>
            await executor.ExecuteIfValidAsync(request, validator, verifier, cancellationToken);
    }
}
