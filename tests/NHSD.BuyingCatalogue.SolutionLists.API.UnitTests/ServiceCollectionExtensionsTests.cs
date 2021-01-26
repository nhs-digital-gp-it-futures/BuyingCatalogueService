using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.SolutionLists.API.UnitTests
{
    [TestFixture]
    internal sealed class ServiceCollectionExtensionsTests
    {
        [Test]
        public void ShouldRegisterControllers()
        {
            IServiceCollection builderServices = null;
            bool optionsExecuted = false;

            void ControllerOptions(MvcOptions mvcOptions) => optionsExecuted = true;

            void ControllerAction(IMvcBuilder builder) => builderServices = builder.Services;

            var serviceCollection = new ServiceCollection();
            serviceCollection.BuildServiceProvider().GetService<IConfigureOptions<MvcOptions>>().Should().BeNull();

            var serviceCollectionReturned = serviceCollection.RegisterSolutionListController(
                ControllerOptions,
                ControllerAction);

            serviceCollectionReturned.Should().Equal(serviceCollection);
            builderServices.Should().Equal(serviceCollection);

            var options = serviceCollection.BuildServiceProvider().GetService<IConfigureOptions<MvcOptions>>();

            Assert.NotNull(options);
            var action = ((ConfigureNamedOptions<MvcOptions>)options).Action;
            action(new MvcOptions());
            optionsExecuted.Should().BeTrue();
        }

        [Test]
        public void ControllerActionThrowsIfNull()
        {
            var serviceCollection = new ServiceCollection();

            Assert.Throws<ArgumentNullException>(() => serviceCollection.RegisterSolutionListController(_ => { }, null));
        }
    }
}
