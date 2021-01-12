﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests.Models;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests
{
    [TestFixture]
    public class SolutionCapabilityRepositoryTests
    {
        private const string Solution1Id = "Sln1";
        private const string Solution2Id = "Sln2";

        private const string SupplierId = "Sup 1";

        private readonly List<CapabilityDetails> _capDetails = new()
        {
            CreateCapability("Cap1", "Desc1", "Ref1", "1.0.0", "http://cap1.link"),
            CreateCapability("Cap2", "Desc2", "Ref2", "1.0.0", "http://cap2.link"),
            CreateCapability("Cap3", "Desc3", "Ref3", "1.0.0", "http://cap3.link"),
        };

        private ISolutionCapabilityRepository _solutionCapabilityRepository;

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync();

            await SupplierEntityBuilder.Create()
                .WithId(SupplierId)
                .Build()
                .InsertAsync();

            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(Solution1Id)
                .WithName(Solution1Id)
                .WithPublishedStatusId((int)PublishedStatus.Published)
                .WithSupplierId(SupplierId)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(Solution1Id)
                .Build()
                .InsertAsync();

            TestContext testContext = new TestContext();
            _solutionCapabilityRepository = testContext.SolutionCapabilityRepository;
        }

        [Test]
        public async Task ShouldHaveOneCapabilityAsync()
        {
            await InsertCapabilityAsync(_capDetails[0]);

            await InsertSolutionCapabilityAsync(Solution1Id, _capDetails[0].Id);

            var solutionCapabilityRequest =
                await _solutionCapabilityRepository.ListSolutionCapabilitiesAsync(Solution1Id, CancellationToken.None);

            var solutionCapability = solutionCapabilityRequest.Should().ContainSingle().Subject;
            solutionCapability.CapabilityId.Should().Be(_capDetails[0].Id);
            solutionCapability.CapabilityName.Should().Be(_capDetails[0].Name);
            solutionCapability.CapabilityDescription.Should().Be(_capDetails[0].Desc);
        }

        [Test]
        public async Task ShouldHaveMultipleCapabilitiesAsync()
        {
            foreach (var capability in _capDetails)
            {
                await InsertCapabilityAsync(capability);
                await InsertSolutionCapabilityAsync(Solution1Id, capability.Id);
            }

            var solutionCapabilityResponse =
                await _solutionCapabilityRepository.ListSolutionCapabilitiesAsync(Solution1Id, CancellationToken.None);

            solutionCapabilityResponse.Count().Should().Be(3);
        }

        [Test]
        public async Task HasMultipleSolutionsAsync()
        {
            await CatalogueItemEntityBuilder
                .Create()
                .WithCatalogueItemId(Solution2Id)
                .WithName(Solution2Id)
                .WithPublishedStatusId((int)PublishedStatus.Published)
                .WithSupplierId(SupplierId)
                .Build()
                .InsertAsync();

            await SolutionEntityBuilder.Create()
                .WithId(Solution2Id)
                .Build()
                .InsertAsync();

            await InsertCapabilityAsync(_capDetails[0]);
            await InsertCapabilityAsync(_capDetails[1]);

            await InsertSolutionCapabilityAsync(Solution1Id, _capDetails[0].Id);
            await InsertSolutionCapabilityAsync(Solution2Id, _capDetails[1].Id);

            var solutionCapabilityResponseSolution1 =
                await _solutionCapabilityRepository.ListSolutionCapabilitiesAsync(Solution1Id, CancellationToken.None);

            var solutionCapabilityResponseSolution2 =
                await _solutionCapabilityRepository.ListSolutionCapabilitiesAsync(Solution2Id, CancellationToken.None);

            var solutionCapability1 = solutionCapabilityResponseSolution1.Should().ContainSingle().Subject;
            solutionCapability1.CapabilityId.Should().Be(_capDetails[0].Id);
            solutionCapability1.CapabilityName.Should().Be(_capDetails[0].Name);
            solutionCapability1.CapabilityDescription.Should().Be(_capDetails[0].Desc);

            var solutionCapability2 = solutionCapabilityResponseSolution2.Should().ContainSingle().Subject;
            solutionCapability2.CapabilityId.Should().Be(_capDetails[1].Id);
            solutionCapability2.CapabilityName.Should().Be(_capDetails[1].Name);
            solutionCapability2.CapabilityDescription.Should().Be(_capDetails[1].Desc);
        }

        [Test]
        public async Task NoCapabilitiesAsync()
        {
            var solutionCapabilityResponse =
                await _solutionCapabilityRepository.ListSolutionCapabilitiesAsync(Solution1Id, CancellationToken.None);

            solutionCapabilityResponse.Should().BeEmpty();
        }

        [Test]
        public async Task NoFailedCapabilitiesAsync()
        {
            await InsertCapabilityAsync(_capDetails[0]);
            await InsertCapabilityAsync(_capDetails[1]);

            await InsertSolutionCapabilityAsync(Solution1Id, _capDetails[0].Id);
            await InsertSolutionCapabilityAsync(Solution1Id, _capDetails[1].Id, false);

            var solutionCapabilityResponseSolution1 =
                await _solutionCapabilityRepository.ListSolutionCapabilitiesAsync(Solution1Id, CancellationToken.None);

            var solutionCapability1 = solutionCapabilityResponseSolution1.Should().ContainSingle().Subject;
            solutionCapability1.CapabilityId.Should().Be(_capDetails[0].Id);
            solutionCapability1.CapabilityName.Should().Be(_capDetails[0].Name);
            solutionCapability1.CapabilityDescription.Should().Be(_capDetails[0].Desc);
        }

        [Test]
        public async Task NoSolutionsAsync()
        {
            var solutionCapabilityResponse =
                await _solutionCapabilityRepository.ListSolutionCapabilitiesAsync(Solution2Id, CancellationToken.None);

            solutionCapabilityResponse.Should().BeEmpty();
        }

        [Test]
        public async Task UpdateSolutionWithOneCapabilityAsync()
        {
            await InsertCapabilityAsync(_capDetails[0]);
            await InsertSolutionCapabilityAsync(Solution1Id, _capDetails[0].Id);

            await InsertCapabilityAsync(_capDetails[1]);

            IEnumerable<string> capabilityReferences = new List<string> { _capDetails[1].Reference };

            await _solutionCapabilityRepository
                .UpdateCapabilitiesAsync(
                    Mock.Of<IUpdateCapabilityRequest>(c =>
                        c.SolutionId == Solution1Id && c.NewCapabilitiesReference == capabilityReferences),
                    new CancellationToken());

            var solutionCapabilities =
                (await _solutionCapabilityRepository.ListSolutionCapabilitiesAsync(Solution1Id, CancellationToken.None)).ToList();

            solutionCapabilities.Count.Should().Be(1);

            solutionCapabilities[0].CapabilityId.Should().Be(_capDetails[1].Id);
            solutionCapabilities[0].CapabilityName.Should().Be(_capDetails[1].Name);
            solutionCapabilities[0].CapabilityDescription.Should().Be(_capDetails[1].Desc);
        }

        [Test]
        public async Task UpdateSolutionWithMultipleCapabilitiesAsync()
        {
            await InsertCapabilityAsync(_capDetails[0]);
            await InsertSolutionCapabilityAsync(Solution1Id, _capDetails[0].Id);

            await InsertCapabilityAsync(_capDetails[1]);
            await InsertCapabilityAsync(_capDetails[2]);

            IEnumerable<string> capabilityReferences = new List<string> { _capDetails[1].Reference, _capDetails[2].Reference };

            await _solutionCapabilityRepository
                .UpdateCapabilitiesAsync(
                    Mock.Of<IUpdateCapabilityRequest>(c =>
                        c.SolutionId == Solution1Id && c.NewCapabilitiesReference == capabilityReferences),
                    new CancellationToken());

            var solutionCapabilities =
                (await _solutionCapabilityRepository.ListSolutionCapabilitiesAsync(Solution1Id, CancellationToken.None)).ToList();

            solutionCapabilities.Count.Should().Be(2);

            solutionCapabilities[0].CapabilityId.Should().Be(_capDetails[1].Id);
            solutionCapabilities[0].CapabilityName.Should().Be(_capDetails[1].Name);
            solutionCapabilities[0].CapabilityDescription.Should().Be(_capDetails[1].Desc);

            solutionCapabilities[1].CapabilityId.Should().Be(_capDetails[2].Id);
            solutionCapabilities[1].CapabilityName.Should().Be(_capDetails[2].Name);
            solutionCapabilities[1].CapabilityDescription.Should().Be(_capDetails[2].Desc);
        }

        [Test]
        public void ShouldThrowIfCapabilityRequestIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _solutionCapabilityRepository.UpdateCapabilitiesAsync(null, new CancellationToken()));
        }

        [Test]
        public async Task ValidationIfOneCapabilityRefDoesNotExistThenCountIsZero()
        {
            await InsertCapabilityAsync(_capDetails[0]);
            await InsertSolutionCapabilityAsync(Solution1Id, _capDetails[0].Id);

            IEnumerable<string> capabilityReferences = new List<string> { _capDetails[1].Reference };

            var count = await _solutionCapabilityRepository
                .GetMatchingCapabilitiesCountAsync(capabilityReferences, new CancellationToken());

            count.Should().Be(0);
        }

        [Test]
        public async Task ValidationIfASingleCapabilityRefInAListOfThreeDoesNotExistThenCountIsTwo()
        {
            await InsertCapabilityAsync(_capDetails[0]);
            await InsertSolutionCapabilityAsync(Solution1Id, _capDetails[0].Id);

            await InsertCapabilityAsync(_capDetails[2]);
            await InsertSolutionCapabilityAsync(Solution1Id, _capDetails[2].Id);

            IEnumerable<string> capabilityReferences = new List<string> { _capDetails[0].Reference, _capDetails[1].Reference, _capDetails[2].Reference };

            var count = await _solutionCapabilityRepository
                .GetMatchingCapabilitiesCountAsync(capabilityReferences, new CancellationToken());

            count.Should().Be(2);
        }

        [Test]
        public async Task ValidationIfNoCapabilityRefsThenCountIsZero()
        {
            await InsertCapabilityAsync(_capDetails[0]);
            await InsertSolutionCapabilityAsync(Solution1Id, _capDetails[0].Id);

            IEnumerable<string> capabilityReferences = new List<string>();

            var count = await _solutionCapabilityRepository
                .GetMatchingCapabilitiesCountAsync(capabilityReferences, new CancellationToken());

            count.Should().Be(0);
        }

        private static CapabilityDetails CreateCapability(string name, string desc, string reference, string version, string sourceUrl)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Desc = desc,
                Reference = reference,
                Version = version,
                SourceUrl = sourceUrl,
            };
        }

        private static async Task InsertCapabilityAsync(CapabilityDetails capability)
        {
            await CapabilityEntityBuilder.Create()
                .WithId(capability.Id)
                .WithName(capability.Name)
                .WithDescription(capability.Desc)
                .WithCapabilityRef(capability.Reference)
                .WithVersion(capability.Version)
                .WithSourceUrl(capability.SourceUrl)
                .Build()
                .InsertAsync();
        }

        private static async Task InsertSolutionCapabilityAsync(string solutionId, Guid capId, bool passed = true)
        {
            await SolutionCapabilityEntityBuilder.Create()
                .WithCapabilityId(capId)
                .WithSolutionId(solutionId)
                .WithStatusId(passed ? 1 : 3)
                .Build()
                .InsertAsync();
        }
    }
}
