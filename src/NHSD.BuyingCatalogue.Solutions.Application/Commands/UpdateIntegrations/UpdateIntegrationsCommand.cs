using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateIntegrations
{
    public sealed class UpdateIntegrationsCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public string Url { get; }

        public UpdateIntegrationsCommand(string solutionId, string url)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            Url = url;
        }
    }
}
