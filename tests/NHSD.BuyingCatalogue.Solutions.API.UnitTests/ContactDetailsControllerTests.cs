using System;
using System.Collections.Generic;
using System.Linq;
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
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetContactDetailBySolutionId;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public class ContactDetailsControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private ContactDetailsController _contactDetailsController;

        private ContactsMaxLengthResult _validationResult;

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

            _validationResult = GetContactsMaxLengthResult(Array.Empty<string>(), Array.Empty<string>());
            _returnedContacts = new List<IContact>();
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldGetContactDetails(bool hasThirdContact)
        {
            _returnedContacts.Add(Contact1);
            _returnedContacts.Add(Contact2);

            if (hasThirdContact)
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
        public async Task ContactDetailsReturnEmptyWhenContactsIsNull()
        {
            _returnedContacts = null;
            var result = (await _contactDetailsController.GetContactDetailsAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            (result.Value as GetContactDetailsResult).Contact1.Should().BeNull();
            (result.Value as GetContactDetailsResult).Contact2.Should().BeNull();
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
            _mockMediator.Verify(x => x.Send(It.Is<UpdateSolutionContactDetailsCommand>(x => x.SolutionId == SolutionId && x.Data == sentModel), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task SubmitForReviewResultFailure()
        {
            _validationResult = GetContactsMaxLengthResult(new[] { "first-name", "last-name" }, new[] { "email-address", "last-name", "phone-number" });

            var result = await _contactDetailsController.UpdateContactDetailsAsync(SolutionId, new UpdateSolutionContactDetailsViewModel()).ConfigureAwait(false) as BadRequestObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);

            var actual = result.Value as Dictionary<string, Dictionary<string, string>>;
            actual.Should().NotBeNull();
            actual.Count.Should().Be(2);
            actual["contact-1"].Count.Should().Be(2);
            actual["contact-1"]["first-name"].Should().Be("maxLength");
            actual["contact-1"]["last-name"].Should().Be("maxLength");

            actual["contact-2"].Count.Should().Be(3);
            actual["contact-2"]["email-address"].Should().Be("maxLength");
            actual["contact-2"]["last-name"].Should().Be("maxLength");
            actual["contact-2"]["phone-number"].Should().Be("maxLength");
        }

        [Test]
        public async Task ShouldNotReportAsInvalidValidContact1()
        {
            _validationResult = GetContactsMaxLengthResult(Array.Empty<string>(), new[] { "email-address", "last-name", "phone-number" } );
 
            var result = await _contactDetailsController.UpdateContactDetailsAsync(SolutionId, new UpdateSolutionContactDetailsViewModel()).ConfigureAwait(false) as BadRequestObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);

            var actual = result.Value as Dictionary<string, Dictionary<string, string>>;
            actual.Should().NotBeNull();
            actual.Count.Should().Be(1);

            actual["contact-2"].Count.Should().Be(3);
            actual["contact-2"]["email-address"].Should().Be("maxLength");
            actual["contact-2"]["last-name"].Should().Be("maxLength");
            actual["contact-2"]["phone-number"].Should().Be("maxLength");
        }

        [Test]
        public async Task ShouldNotReportAsInvalidValidContact2()
        {
            _validationResult = GetContactsMaxLengthResult(new []{ "first-name", "last-name" }, Array.Empty<string>());

            var result = await _contactDetailsController.UpdateContactDetailsAsync(SolutionId, new UpdateSolutionContactDetailsViewModel()).ConfigureAwait(false) as BadRequestObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);

            var actual = result.Value as Dictionary<string, Dictionary<string, string>>;
            actual.Should().NotBeNull();
            actual.Count.Should().Be(1);
            actual["contact-1"].Count.Should().Be(2);
            actual["contact-1"]["first-name"].Should().Be("maxLength");
            actual["contact-1"]["last-name"].Should().Be("maxLength");
        }

        private static ContactsMaxLengthResult GetContactsMaxLengthResult(string[] contact1Errors, string[] contact2Errors)
        {
            var contact1Dict = contact1Errors.ToDictionary(k => k, v => "maxLength");
            var contact2Dict = contact2Errors.ToDictionary(k => k, v => "maxLength");
            return new ContactsMaxLengthResult(Mock.Of<ISimpleResult>(s => s.ToDictionary() == contact1Dict && s.IsValid == (contact1Dict.Count == 0)), Mock.Of<ISimpleResult>(s => s.ToDictionary() == contact2Dict && s.IsValid == (contact2Dict.Count == 0)));
        }
    }
}
