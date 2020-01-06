using System.Collections.Generic;
using System.Linq;
using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

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
        public UpdateSolutionBrowsersSupportedViewModel Data { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionBrowsersSupportedCommand"/> class.
        /// </summary>
        public UpdateSolutionBrowsersSupportedCommand(string solutionId, UpdateSolutionBrowsersSupportedViewModel data)
        {
            SolutionId = solutionId.ThrowIfNull();
            Data = data.ThrowIfNull();
            Data.MobileResponsive = Data.MobileResponsive?.Trim();
            Data.BrowsersSupported = Data.BrowsersSupported == null ?
                new HashSet<string>() : 
                new HashSet<string>(Data.BrowsersSupported.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()));
        }
    }
}
