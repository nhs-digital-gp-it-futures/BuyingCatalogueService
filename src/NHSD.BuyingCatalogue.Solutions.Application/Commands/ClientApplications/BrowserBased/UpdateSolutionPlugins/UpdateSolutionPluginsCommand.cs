using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.BrowserBased;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionPlugins
{
    public sealed class UpdateSolutionPluginsCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public IUpdateBrowserBasedPluginsData Data { get; }

        public UpdateSolutionPluginsCommand(string solutionId, IUpdateBrowserBasedPluginsData data)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}
