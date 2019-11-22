using System;
using NHSD.BuyingCatalogue.Solution.API.Controllers;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solution.API.UnitTests
{
    [TestFixture]
    public sealed class SolutionsControllerTests
    {
        [Test]
        public void NullMediatorShouldThrowNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SolutionsController(null));
        }
    }
}
