using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class SolutionCapabilityEntityBuilder
    {
        private readonly SolutionCapabilityEntity _SolutionCapabilityEntity;

        public static SolutionCapabilityEntityBuilder Create()
        {
            return new SolutionCapabilityEntityBuilder();
        }

        public SolutionCapabilityEntityBuilder()
        {
            //Default
            _SolutionCapabilityEntity = new SolutionCapabilityEntity
            {
                SolutionId = "SolutionId",
                CapabilityId = Guid.NewGuid(),
                StatusId = 1
            };
        }

        public SolutionCapabilityEntityBuilder WithSolutionId(string solutionId)
        {
            _SolutionCapabilityEntity.SolutionId = solutionId;
            return this;
        }

        public SolutionCapabilityEntityBuilder WithCapabilityId(Guid capabilityId)
        {
            _SolutionCapabilityEntity.CapabilityId = capabilityId;
            return this;
        }

        public SolutionCapabilityEntityBuilder WithStatusId(int statusId)
        {
            _SolutionCapabilityEntity.StatusId = statusId;
            return this;
        }

        public SolutionCapabilityEntity Build()
        {
            return _SolutionCapabilityEntity;
        }
    }
}
