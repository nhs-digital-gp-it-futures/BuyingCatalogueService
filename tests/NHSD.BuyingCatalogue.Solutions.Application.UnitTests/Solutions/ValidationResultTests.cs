﻿using System;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.SubmitForReview;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class ValidationResultTests
    {
        private ValidationResult validationResult;

        [SetUp]
        public void Setup()
        {
            validationResult = new ValidationResult();
        }

        [Test]
        public void ShouldThrowAddNullValidationError()
        {
            Assert.Throws<ArgumentNullException>(() => validationResult.Add((ValidationError)null));
        }

        [Test]
        public void ShouldThrowAddNullValidationResult()
        {
            Assert.Throws<ArgumentNullException>(() => validationResult.Add((ValidationResult)null));
        }
    }
}
