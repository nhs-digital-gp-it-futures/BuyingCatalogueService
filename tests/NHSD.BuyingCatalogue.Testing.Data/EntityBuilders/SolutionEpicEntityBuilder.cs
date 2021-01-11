using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class SolutionEpicEntityBuilder
    {
        private const int PassedStatusId = 1;
        private readonly SolutionEpicEntity _solutionEpicEntity;

        public SolutionEpicEntityBuilder()
        {
            _solutionEpicEntity = new SolutionEpicEntity
            {
                SolutionId = "SolutionId",
                CapabilityId = Guid.NewGuid(),
                EpicId = "EpicId",
                StatusId = PassedStatusId
            };
        }

        public enum SolutionEpicStatus
        {
            Undefined = 0,
            Passed = 1,
            NotEvidenced = 3,
        }

        public static SolutionEpicEntityBuilder Create()
        {
            return new();
        }

        public SolutionEpicEntityBuilder WithSolutionId(string solutionId)
        {
            _solutionEpicEntity.SolutionId = solutionId;
            return this;
        }

        public SolutionEpicEntityBuilder WithCapabilityId(Guid capabilityId)
        {
            _solutionEpicEntity.CapabilityId = capabilityId;
            return this;
        }

        public SolutionEpicEntityBuilder WithEpicId(string epicId)
        {
            _solutionEpicEntity.EpicId = epicId;
            return this;
        }

        public SolutionEpicEntityBuilder WithStatus(SolutionEpicStatus status)
        {
            _solutionEpicEntity.StatusId = (int)status;
            return this;
        }

        public SolutionEpicEntity Build()
        {
            return _solutionEpicEntity;
        }
    }
}
