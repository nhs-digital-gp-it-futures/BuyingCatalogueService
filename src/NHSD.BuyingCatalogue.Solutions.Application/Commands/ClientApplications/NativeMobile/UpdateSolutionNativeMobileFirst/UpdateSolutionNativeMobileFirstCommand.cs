﻿using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileFirst
{
    public sealed class UpdateSolutionNativeMobileFirstCommand : IRequest<ISimpleResult>
    {
        public UpdateSolutionNativeMobileFirstCommand(string solutionId, string mobileFirstDesign)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            MobileFirstDesign = mobileFirstDesign;
        }

        public string SolutionId { get; }

        public string MobileFirstDesign { get; }
    }
}
