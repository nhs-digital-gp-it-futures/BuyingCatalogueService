using System;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Capabilities;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Data
{
    internal static class CapabilityTestData
	{
		internal static ICapabilityListResult Default()
		{
			var id = Guid.NewGuid();

            var capability = new Mock<ICapabilityListResult>();
            capability.Setup(c => c.Id).Returns(id);
            capability.Setup(c => c.Name).Returns($"Capability {id}");
            capability.Setup(c => c.IsFoundation).Returns(false);
            return capability.Object;
        }
	}
}
