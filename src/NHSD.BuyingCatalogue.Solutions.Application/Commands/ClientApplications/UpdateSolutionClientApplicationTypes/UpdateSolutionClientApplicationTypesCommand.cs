﻿using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.UpdateSolutionClientApplicationTypes
{
    public sealed class UpdateSolutionClientApplicationTypesCommand : IRequest<ISimpleResult>
    {
        /// <summary>
        /// Gets a value to uniquely identify a solution.
        /// </summary>
        public string SolutionId { get; }

        /// <summary>
        /// Gets the updated details of a solution.
        /// </summary>
        public IUpdateSolutionClientApplicationTypes Data { get; }

        public UpdateSolutionClientApplicationTypesCommand(string solutionId, IUpdateSolutionClientApplicationTypes data)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}
