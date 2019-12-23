using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands
{
    internal class Handler<T, TResult> : IRequestHandler<T, TResult>
        where TResult : Validation.IResult
        where T : IRequest<TResult> //Mediatr requires this
    {
        private readonly IExecutor<T> _executor;
        private readonly IValidator<T, TResult> _validator;

        /// <summary>
        /// Initialises a new instance of the <see cref="Handler"/> class.
        /// </summary>
        public Handler(IExecutor<T> executor, IValidator<T, TResult> validator)
        {
            _executor = executor;
            _validator = validator;
        }

        /// <summary>
        /// Executes the action of this command.
        /// </summary>
        /// <param name="request">The command parameters.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A task representing an operation to get the result of this command.</returns>
        public async Task<TResult> Handle(T request, CancellationToken cancellationToken) =>
            await _executor.ExecuteIfValidAsync(request, _validator, cancellationToken).ConfigureAwait(false);
    }
}
