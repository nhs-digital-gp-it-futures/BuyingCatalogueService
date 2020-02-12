using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class SolutionEpicEntityBuilder
    {
        private readonly SolutionEpicEntity _solutionEpicEntity;
        public static SolutionEpicEntityBuilder Create()
        {
            return new SolutionEpicEntityBuilder();
        }

        /// <summary>
        /// Status Enum from Integration test reference data:
        ///     tests\NHSD.BuyingCatalogue.Testing.Data\SqlResources\ReferenceData.sql
        /// </summary>
        public enum SolutionEpicStatus
        {
            Passed=1,
            NotEvidenced=2
        }

        public SolutionEpicEntityBuilder()
        {
            //Default
            _solutionEpicEntity = new SolutionEpicEntity()
            {
                SolutionId = "SolutionId",
                CapabilityId = Guid.NewGuid(),
                EpicId = "EpicId",
                
                StatusId = 1
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
