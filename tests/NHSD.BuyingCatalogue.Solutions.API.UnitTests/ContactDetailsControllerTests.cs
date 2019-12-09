using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public class ContactDetailsControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private ContactDetailsController _contactDetailsController;

        private UpdateSolutionContactDetailsValidationResult _validationResult;

        private const string SolutionId = "Sln1";
        private List<IContact> _returnedContacts;

        private static readonly IContact Contact1 = Mock.Of<IContact>(m => m.FirstName == "Bob" &&
                                                                           m.LastName == "Builder" &&
                                                                           m.Name == "Bob Builder" &&
                                                                           m.Email == "bob@builder.com" &&
                                                                           m.Department == "building" &&
                                                                           m.PhoneNumber == "12345678901");

        private static readonly IContact Contact2 = Mock.Of<IContact>(m => m.FirstName == "Alice" &&
                                                                          m.LastName == "Wonderland" &&
                                                                          m.Name == "Alice Wonderland" &&
                                                                          m.Email == "alice@wonderland.com" &&
                                                                          m.Department == "prescription" &&
                                                                          m.PhoneNumber == "0123412345");

        private static readonly IContact Contact3 = Mock.Of<IContact>(m => m.FirstName == "Fred" &&
                                                                          m.LastName == "Frog" &&
                                                                          m.Name == "Fred Frog" &&
                                                                          m.Email == "fred@frog.com" &&
                                                                          m.Department == "suppliers" &&
                                                                          m.PhoneNumber == "04567891234");

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _contactDetailsController = new ContactDetailsController(_mockMediator.Object);
            _mockMediator.Setup(m => m.Send(It.Is<GetContactDetailBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _returnedContacts);

            _mockMediator.Setup(m =>
                m.Send(It.Is<UpdateSolutionContactDetailsCommand>(q => q.SolutionId == SolutionId),
                    It.IsAny<CancellationToken>())).ReturnsAsync(() => _validationResult);

            _validationResult = new UpdateSolutionContactDetailsValidationResult();
            _returnedContacts = new List<IContact>();
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldGetContactDetails(bool hasThirdContact)
        {
            _returnedContacts.Add(Contact1);
            _returnedContacts.Add(Contact2);

            if(hasThirdContact)
                _returnedContacts.Add(Contact3);

            var result = (await _contactDetailsController.GetContactDetailsAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be(200);

            var contact = result.Value as GetContactDetailsResult;

            contact.Contact1.FirstName.Should().BeEquivalentTo(Contact1.FirstName);
            contact.Contact1.LastName.Should().BeEquivalentTo(Contact1.LastName);
            contact.Contact1.DepartmentName.Should().BeEquivalentTo(Contact1.Department);
            contact.Contact1.EmailAddress.Should().BeEquivalentTo(Contact1.Email);
            contact.Contact1.PhoneNumber.Should().BeEquivalentTo(Contact1.PhoneNumber);

            contact.Contact2.FirstName.Should().BeEquivalentTo(Contact2.FirstName);
            contact.Contact2.LastName.Should().BeEquivalentTo(Contact2.LastName);
            contact.Contact2.DepartmentName.Should().BeEquivalentTo(Contact2.Department);
            contact.Contact2.EmailAddress.Should().BeEquivalentTo(Contact2.Email);
            contact.Contact2.PhoneNumber.Should().BeEquivalentTo(Contact2.PhoneNumber);

            _mockMediator.Verify(m => m.Send(It.Is<GetContactDetailBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ShouldGetOneContactDetails()
        {
            _returnedContacts.Add(Contact1);

            var result = (await _contactDetailsController.GetContactDetailsAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be(200);

            var contact = result.Value as GetContactDetailsResult;

            contact.Contact1.FirstName.Should().BeEquivalentTo(Contact1.FirstName);
            contact.Contact1.LastName.Should().BeEquivalentTo(Contact1.LastName);
            contact.Contact1.DepartmentName.Should().BeEquivalentTo(Contact1.Department);
            contact.Contact1.EmailAddress.Should().BeEquivalentTo(Contact1.Email);
            contact.Contact1.PhoneNumber.Should().BeEquivalentTo(Contact1.PhoneNumber);

            _mockMediator.Verify(m => m.Send(It.Is<GetContactDetailBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ContactDetailsNotFoundWhenContactsIsNull()
        {
            _returnedContacts = null;
            var result = (await _contactDetailsController.GetContactDetailsAsync(SolutionId).ConfigureAwait(false)) as NotFoundResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(m => m.Send(It.Is<GetContactDetailBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task TwoNullContactFromEmptyList()
        {
            var result = (await _contactDetailsController.GetContactDetailsAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be(200);

            var contact = result.Value as GetContactDetailsResult;

            contact.Contact1.Should().BeNull();
            contact.Contact2.Should().BeNull();
            
            _mockMediator.Verify(m => m.Send(It.Is<GetContactDetailBySolutionIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateContactDetailsSuccessReturnsNoErrors()
        {
            var sentModel = new UpdateSolutionContactDetailsViewModel();
            var result = await _contactDetailsController.UpdateContactDetailsAsync(SolutionId, sentModel).ConfigureAwait(false) as NoContentResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _mockMediator.Verify(x => x.Send(It.Is<UpdateSolutionContactDetailsCommand>(x => x.SolutionId == SolutionId && x.Details == sentModel), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task SubmitForReviewResultFailure()
        {
            _validationResult = new UpdateSolutionContactDetailsValidationResult { MaxLength = { "This update was too cool for school" } };
            var expectedResponse = new UpdateSolutionContactDetailsResult(_validationResult);

            var result = await _contactDetailsController.UpdateContactDetailsAsync(SolutionId, new UpdateSolutionContactDetailsViewModel()).ConfigureAwait(false) as BadRequestObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);

            var actual = result.Value as UpdateSolutionContactDetailsResult;
            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public void UpdateResultSetsValidationCorrectly()
        {
            var response = new UpdateSolutionContactDetailsResult(_validationResult);
            response.MaxLength.Should().BeEquivalentTo(_validationResult.MaxLength);
        }
    }
}
