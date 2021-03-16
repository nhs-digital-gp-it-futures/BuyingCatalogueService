using System;
using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class SolutionResult
    {
        public SolutionResult(ISolution solution)
        {
            if (solution is null)
                return;

            Id = solution.Id;
            Name = solution.Name;
            SupplierName = solution.SupplierName;
            LastUpdated = solution.LastUpdated;
            IsFoundation = solution.IsFoundation;
            FrameworkShortNames = solution.FrameworkShortNames;

            Sections = new Sections(solution);
        }

        public string Id { get; }

        public string Name { get; }

        public string SupplierName { get; }

        public bool? IsFoundation { get; }

        public DateTime? LastUpdated { get; }

        public Sections Sections { get; }

        public IEnumerable<string> FrameworkShortNames { get; }
    }
}
