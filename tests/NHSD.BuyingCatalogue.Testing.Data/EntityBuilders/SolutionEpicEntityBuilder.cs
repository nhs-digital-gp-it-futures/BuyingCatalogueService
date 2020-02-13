using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class SolutionEpicEntityBuilder
    {
        private readonly SolutionEpicEntity _solutionEpicEntity;
        private const int StatusId = 1;

        public static SolutionEpicEntityBuilder Create()
        {
            return new SolutionEpicEntityBuilder();
        }

        public SolutionEpicEntityBuilder()
        {
            //Default
            _solutionEpicEntity = new SolutionEpicEntity()
            {
                SolutionId = "SolutionId",
                CapabilityId = Guid.NewGuid(),
                EpicId = "EpicId",
                StatusId = StatusId
            };
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

        public SolutionEpicEntityBuilder WithStatusId(int statusId)
        {
            _solutionEpicEntity.StatusId = statusId;
            return this;
        }

        public SolutionEpicEntity Build()
        {
            return _solutionEpicEntity;
        }
    }
}
