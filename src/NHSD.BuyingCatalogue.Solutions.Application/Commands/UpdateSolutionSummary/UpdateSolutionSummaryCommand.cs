﻿using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary
{
    public sealed class UpdateSolutionSummaryCommand : IRequest<ISimpleResult>
    {
        public UpdateSolutionSummaryCommand(string solutionId, IUpdateSolutionSummary data)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        /// <summary>
        /// Gets a value to uniquely identify a solution.
        /// </summary>
        public string SolutionId { get; }

        /// <summary>
        /// Gets the updated details of a solution.
        /// </summary>
        public IUpdateSolutionSummary Data { get; }
    }
}
