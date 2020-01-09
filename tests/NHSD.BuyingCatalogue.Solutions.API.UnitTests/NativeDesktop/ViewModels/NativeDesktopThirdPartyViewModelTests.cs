using FluentAssertions;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.NativeDesktop.ViewModels
{
    [TestFixture]
    public sealed class NativeDesktopThirdPartyViewModelTests
    {
        [TestCase("         Component           ", "Component")]
        [TestCase("Component           ", "Component")]
        [TestCase("         Component", "Component")]
        public void TrimComponentReturnsTrimmedCopy(string value, string trimmedValue)
        {
            var viewModel = new UpdateNativeDesktopThirdPartyViewModel();
            viewModel.ThirdPartyComponents = value;
            var result = viewModel.Trim();
            result.ThirdPartyComponents.Should().Be(trimmedValue);
        }

        [TestCase("         Capability           ", "Capability")]
        [TestCase("Capability           ", "Capability")]
        [TestCase("         Capability", "Capability")]
        public void TrimCapabilityReturnsTrimmedCopy(string value, string trimmedValue)
        {
            var viewModel = new UpdateNativeDesktopThirdPartyViewModel();
            viewModel.DeviceCapabilities = value;
            var result = viewModel.Trim();
            result.DeviceCapabilities.Should().Be(trimmedValue);
        }
    }
}
