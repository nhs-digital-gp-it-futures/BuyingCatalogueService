using System;
using JetBrains.Annotations;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class SolutionEpicEntityBuilder
    {
        private const int PassedStatusId = 1;
        private readonly SolutionEpicEntity solutionEpicEntity;

        public SolutionEpicEntityBuilder()
        {
            solutionEpicEntity = new SolutionEpicEntity
            {
                SolutionId = "SolutionId",
                CapabilityId = Guid.NewGuid(),
                EpicId = "EpicId",
                StatusId = PassedStatusId,
            };
        }

        public enum SolutionEpicStatus
        {
            // ReSharper disable once UnusedMember.Global (CA1008: enums should have zero value)
            Undefined = 0,

            Passed = 1,

            [UsedImplicitly]
            NotEvidenced = 2,
        }

        public static SolutionEpicEntityBuilder Create()
        {
            return new();
        }

        public SolutionEpicEntityBuilder WithSolutionId(string solutionId)
        {
            solutionEpicEntity.SolutionId = solutionId;
            return this;
        }

        public SolutionEpicEntityBuilder WithCapabilityId(Guid capabilityId)
        {
            solutionEpicEntity.CapabilityId = capabilityId;
            return this;
        }

        public SolutionEpicEntityBuilder WithEpicId(string epicId)
        {
            solutionEpicEntity.EpicId = epicId;
            return this;
        }

        public SolutionEpicEntityBuilder WithStatus(SolutionEpicStatus status)
        {
            solutionEpicEntity.StatusId = (int)status;
            return this;
        }

        public SolutionEpicEntity Build()
        {
            return solutionEpicEntity;
        }
    }
}
