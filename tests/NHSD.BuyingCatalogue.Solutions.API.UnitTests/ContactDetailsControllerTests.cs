using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    internal sealed class ContactDetailsControllerTests
    {
        private const string SolutionId = "Sln1";

        private static readonly Expression<Func<IContact, bool>> Bob = b =>
            b.FirstName == "Bob"
            && b.LastName == "Builder"
            && b.Name == "Bob Builder"
            && b.Email == "bob@builder.com"
            && b.Department == "building"
            && b.PhoneNumber == "12345678901";

        private static readonly Expression<Func<IContact, bool>> Alice = a =>
            a.FirstName == "Alice"
            && a.LastName == "Wonderland"
            && a.Name == "Alice Wonderland"
            && a.Email == "alice@wonderland.com"
            && a.Department == "prescription"
            && a.PhoneNumber == "0123412345";

        private static readonly Expression<Func<IContact, bool>> Fred = f =>
            f.FirstName == "Fred"
            && f.LastName == "Frog"
            && f.Name == "Fred Frog"
            && f.Email == "fred@frog.com"
            && f.Department == "suppliers"
            && f.PhoneNumber == "04567891234";

        private static readonly IContact Contact1 = Mock.Of(Bob);
        private static readonly IContact Contact2 = Mock.Of(Alice);
        private static readonly IContact Contact3 = Mock.Of(Fred);

        private Mock<IMediator> mockMediator;
        private ContactDetailsController contactDetailsController;
        private ContactsMaxLengthResult validationResult;
        private List<IContact> returnedContacts;

        [SetUp]
        public void Setup()
        {
            mockMediator = new Mock<IMediator>();
            contactDetailsController = new ContactDetailsController(mockMediator.Object);
            mockMediator
                .Setup(m => m.Send(
                    It.Is<GetContactDetailBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => returnedContacts);

            mockMediator
                .Setup(m => m.Send(
                    It.Is<UpdateSolutionContactDetailsCommand>(q => q.SolutionId == SolutionId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => validationResult);

            validationResult = GetContactsMaxLengthResult(Array.Empty<string>(), Array.Empty<string>());
            returnedContacts = new List<IContact>();
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldGetContactDetails(bool hasThirdContact)
        {
            returnedContacts.Add(Contact1);
            returnedContacts.Add(Contact2);

            if (hasThirdContact)
                returnedContacts.Add(Contact3);

            var result = await contactDetailsController.GetContactDetailsAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var contact = result.Value as GetContactDetailsResult;

            Assert.NotNull(contact);
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

            mockMediator.Verify(m => m.Send(
                It.Is<GetContactDetailBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldGetOneContactDetails()
        {
            returnedContacts.Add(Contact1);

            var result = await contactDetailsController.GetContactDetailsAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var contact = result.Value as GetContactDetailsResult;

            Assert.NotNull(contact);
            contact.Contact1.FirstName.Should().BeEquivalentTo(Contact1.FirstName);
            contact.Contact1.LastName.Should().BeEquivalentTo(Contact1.LastName);
            contact.Contact1.DepartmentName.Should().BeEquivalentTo(Contact1.Department);
            contact.Contact1.EmailAddress.Should().BeEquivalentTo(Contact1.Email);
            contact.Contact1.PhoneNumber.Should().BeEquivalentTo(Contact1.PhoneNumber);

            mockMediator.Verify(m => m.Send(
                It.Is<GetContactDetailBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ContactDetailsReturnEmptyWhenContactsIsNull()
        {
            returnedContacts = null;
            var result = await contactDetailsController.GetContactDetailsAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            result.Value.Should().BeOfType<GetContactDetailsResult>();
            result.Value.As<GetContactDetailsResult>().Contact1.Should().BeNull();
            result.Value.As<GetContactDetailsResult>().Contact2.Should().BeNull();
        }

        [Test]
        public async Task TwoNullContactFromEmptyList()
        {
            var result = await contactDetailsController.GetContactDetailsAsync(SolutionId) as ObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

            var contact = result.Value as GetContactDetailsResult;

            Assert.NotNull(contact);
            contact.Contact1.Should().BeNull();
            contact.Contact2.Should().BeNull();

            mockMediator.Verify(m => m.Send(
                It.Is<GetContactDetailBySolutionIdQuery>(q => q.Id == SolutionId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task UpdateContactDetailsSuccessReturnsNoErrors()
        {
            var sentModel = new UpdateSolutionContactDetailsViewModel();
            var result = await contactDetailsController.UpdateContactDetailsAsync(
                SolutionId,
                sentModel) as NoContentResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            mockMediator.Verify(m => m.Send(
                It.Is<UpdateSolutionContactDetailsCommand>(c => c.SolutionId == SolutionId && c.Data == sentModel),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task SubmitForReviewResultFailure()
        {
            validationResult = GetContactsMaxLengthResult(
                new[] { "first-name", "last-name" },
                new[] { "email-address", "last-name", "phone-number" });

            var result = await contactDetailsController.UpdateContactDetailsAsync(
                SolutionId,
                new UpdateSolutionContactDetailsViewModel()) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var actual = result.Value as Dictionary<string, Dictionary<string, string>>;

            Assert.NotNull(actual);
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
            validationResult = GetContactsMaxLengthResult(
                Array.Empty<string>(),
                new[] { "email-address", "last-name", "phone-number" });

            var result = await contactDetailsController.UpdateContactDetailsAsync(
                SolutionId,
                new UpdateSolutionContactDetailsViewModel()) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var actual = result.Value as Dictionary<string, Dictionary<string, string>>;

            Assert.NotNull(actual);
            actual.Count.Should().Be(1);

            actual["contact-2"].Count.Should().Be(3);
            actual["contact-2"]["email-address"].Should().Be("maxLength");
            actual["contact-2"]["last-name"].Should().Be("maxLength");
            actual["contact-2"]["phone-number"].Should().Be("maxLength");
        }

        [Test]
        public async Task ShouldNotReportAsInvalidValidContact2()
        {
            validationResult = GetContactsMaxLengthResult(new[] { "first-name", "last-name" }, Array.Empty<string>());

            var result = await contactDetailsController.UpdateContactDetailsAsync(
                SolutionId,
                new UpdateSolutionContactDetailsViewModel()) as BadRequestObjectResult;

            Assert.NotNull(result);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var actual = result.Value as Dictionary<string, Dictionary<string, string>>;

            Assert.NotNull(actual);
            actual.Count.Should().Be(1);
            actual["contact-1"].Count.Should().Be(2);
            actual["contact-1"]["first-name"].Should().Be("maxLength");
            actual["contact-1"]["last-name"].Should().Be("maxLength");
        }

        private static ContactsMaxLengthResult GetContactsMaxLengthResult(
            IEnumerable<string> contact1Errors,
            IEnumerable<string> contact2Errors)
        {
            var contact1Dict = contact1Errors.ToDictionary(k => k, _ => "maxLength");
            var contact2Dict = contact2Errors.ToDictionary(k => k, _ => "maxLength");

            return new ContactsMaxLengthResult(
                Mock.Of<ISimpleResult>(s => s.ToDictionary() == contact1Dict && s.IsValid == (contact1Dict.Count == 0)),
                Mock.Of<ISimpleResult>(s => s.ToDictionary() == contact2Dict && s.IsValid == (contact2Dict.Count == 0)));
        }
    }
}
