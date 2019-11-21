using System;
using NHSD.BuyingCatalogue.API.Controllers;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.API.UnitTests
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
