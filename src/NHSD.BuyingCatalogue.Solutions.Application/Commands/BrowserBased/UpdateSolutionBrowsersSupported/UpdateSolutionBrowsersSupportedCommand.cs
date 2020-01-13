using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.BrowserBased;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowsersSupported
{
    public sealed class UpdateSolutionBrowsersSupportedCommand : IRequest<ISimpleResult>
    {
        /// <summary>
        /// A value to uniquely identify a solution.
        /// </summary>
        public string SolutionId { get; }

        /// <summary>
        /// Updated details of a solution.
        /// </summary>
        public IUpdateBrowserBasedBrowsersSupportedData Data { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionBrowsersSupportedCommand"/> class.
        /// </summary>
        public UpdateSolutionBrowsersSupportedCommand(string solutionId, IUpdateBrowserBasedBrowsersSupportedData data)
        {
            SolutionId = solutionId.ThrowIfNull();
            Data = data.ThrowIfNull();
            Data = data?.Trim();
        }
    }
}
