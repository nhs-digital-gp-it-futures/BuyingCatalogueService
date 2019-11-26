using System;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
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
