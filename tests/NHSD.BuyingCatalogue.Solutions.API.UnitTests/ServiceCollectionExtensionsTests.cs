using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    internal sealed class ServiceCollectionExtensionsTests
    {
        [Test]
        public void ShouldRegisterControllers()
        {
            IServiceCollection builderServices = null;
            bool optionsExecuted = false;

            Action<MvcOptions> op = options => optionsExecuted = true;

            Action<IMvcBuilder> controllerAction = builder => builderServices = builder.Services;

            var serviceCollection = new ServiceCollection();
            serviceCollection.BuildServiceProvider().GetService<IConfigureOptions<MvcOptions>>().Should().BeNull();

            var serviceCollectionReturned = serviceCollection.RegisterSolutionController(op, controllerAction);

            serviceCollectionReturned.Should().Equal(serviceCollection);
            builderServices.Should().Equal(serviceCollection);

            var options = serviceCollection.BuildServiceProvider().GetService<IConfigureOptions<MvcOptions>>();
            options.Should().NotBeNull();
            var action = ((Microsoft.Extensions.Options.ConfigureNamedOptions<Microsoft.AspNetCore.Mvc.MvcOptions>)options).Action;
            action(new MvcOptions());
            optionsExecuted.Should().BeTrue();
        }

        [Test]
        public void ControllerActionThrowsIfNull()
        {
            var serviceCollection = new ServiceCollection();

            Assert.Throws<ArgumentNullException>(() => serviceCollection.RegisterSolutionController(op => {}, null));
        }
    }
}
