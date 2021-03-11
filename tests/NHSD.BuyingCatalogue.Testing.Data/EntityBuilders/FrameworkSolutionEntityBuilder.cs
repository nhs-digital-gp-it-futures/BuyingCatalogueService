using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class FrameworkSolutionEntityBuilder
    {
        private readonly FrameworkSolutionEntity frameworkSolutionEntity;

        public FrameworkSolutionEntityBuilder()
        {
            frameworkSolutionEntity = new FrameworkSolutionEntity
            {
                // ReSharper disable once StringLiteralTypo
                FrameworkId = "NHSDGP001",
                SolutionId = "Sln1",
                IsFoundation = true,
            };
        }

        public static FrameworkSolutionEntityBuilder Create()
        {
            return new();
        }

        public FrameworkSolutionEntityBuilder WithSolutionId(string solutionId)
        {
            frameworkSolutionEntity.SolutionId = solutionId;
            return this;
        }

        public FrameworkSolutionEntityBuilder WithFoundation(bool isFoundation)
        {
            frameworkSolutionEntity.IsFoundation = isFoundation;
            return this;
        }

        public FrameworkSolutionEntityBuilder WithFrameworkId(string frameworkId)
        {
            frameworkSolutionEntity.FrameworkId = frameworkId;
            return this;
        }

        public FrameworkSolutionEntity Build()
        {
            return frameworkSolutionEntity;
        }
    }
}
