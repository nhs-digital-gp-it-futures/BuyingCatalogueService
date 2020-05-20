using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.NativeMobile;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileConnectionDetails
{
    public sealed class UpdateSolutionMobileConnectionDetailsCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public IUpdateNativeMobileConnectionDetailsData Data { get; }

        public UpdateSolutionMobileConnectionDetailsCommand(string solutionId, IUpdateNativeMobileConnectionDetailsData data)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}
