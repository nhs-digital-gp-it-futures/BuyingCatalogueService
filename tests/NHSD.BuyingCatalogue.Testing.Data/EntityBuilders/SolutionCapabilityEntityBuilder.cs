using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class SolutionCapabilityEntityBuilder
    {
        private readonly SolutionCapabilityEntity solutionCapabilityEntity;

        public SolutionCapabilityEntityBuilder()
        {
            solutionCapabilityEntity = new SolutionCapabilityEntity
            {
                SolutionId = "SolutionId",
                CapabilityId = Guid.NewGuid(),
                StatusId = 1,
            };
        }

        public static SolutionCapabilityEntityBuilder Create()
        {
            return new();
        }

        public SolutionCapabilityEntityBuilder WithSolutionId(string solutionId)
        {
            solutionCapabilityEntity.SolutionId = solutionId;
            return this;
        }

        public SolutionCapabilityEntityBuilder WithCapabilityId(Guid capabilityId)
        {
            solutionCapabilityEntity.CapabilityId = capabilityId;
            return this;
        }

        public SolutionCapabilityEntityBuilder WithStatusId(int statusId)
        {
            solutionCapabilityEntity.StatusId = statusId;
            return this;
        }

        public SolutionCapabilityEntity Build()
        {
            return solutionCapabilityEntity;
        }
    }
}
