using System;
using System.Collections.Generic;
using System.Text;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class UpdateSolutionClientApplicationRequest : IUpdateSolutionClientApplicationRequest
    {
        public string Id { get; set; }

        public string ClientApplication { get; set; }
    }
}
