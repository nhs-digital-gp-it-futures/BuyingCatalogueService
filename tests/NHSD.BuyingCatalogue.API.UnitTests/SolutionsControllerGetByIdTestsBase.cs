using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.API.Controllers;
using NHSD.BuyingCatalogue.API.ViewModels;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NHSD.BuyingCatalogue.API.UnitTests
{
    [TestFixture]
    public class SolutionsControllerGetByIdTestsBase
    {
        private Mock<IMediator> _mockMediator;

        private SolutionsController _solutionsController;

        private const string SolutionId = "Sln1"; 

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _solutionsController = new SolutionsController(_mockMediator.Object);
        }

        protected async Task<SolutionByIdViewModel> GetSolutionViewModel(string summary = null, string description = null, string link = null, IEnumerable<string> clientApplicationTypes = null, IEnumerable<string> browsersSupported = null, bool? mobileResponsive = null)
        {
            var solution = new Solution
            {
                Features = new List<string>(), //TODO - Remove

                ClientApplication = new ClientApplication
                {
                    ClientApplicationTypes = new HashSet<string>(new[] { "browser-based" }),
                    BrowsersSupported = new HashSet<string>(browsersSupported ?? new string[0]),
                    MobileResponsive = mobileResponsive
                }
            };

            _mockMediator
                .Setup(m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            return ((GetSolutionByIdResult)((ObjectResult)(await _solutionsController.ById(SolutionId)).Result).Value).Solution;
        }
    }
}




/* For reference


                {
    "solution": {
        "id": "Sln1",
        "name": "MedicOnline",
        "marketingData": {
            "sections": [
                {
                    "mandatory": [
                        "summary"
                    ],
                    "id": "solution-description",
                    "data": {
                        "summary": "An full online medicine system",
                        "description": "",
                        "link": "UrlSln1"
                    },
                    "requirement": "Mandatory",
                    "status": "COMPLETE"
                },
                {
                    "mandatory": [],
                    "id": "features",
                    "data": {
                        "listing": [
                            "Appointments",
                            "Prescribing"
                        ]
                    },
                    "requirement": "Optional",
                    "status": "COMPLETE"
                },
                {
                    "mandatory": [ "client-application-types" ],
                    "id": "client-application-types",
                    "requirement": "Mandatory",
                    "status": "COMPLETE",
                    "data" : {
                        "client-application-types" : ["browser-based", "native-mobile"]
                    }    
                    "sections": [
                        {
                            "id": "browser-based",
                            "requirement": "Mandatory",
                            "status": "INCOMPLETE",
                            "mandatory": [ "browsers-supported", "plug-ins-or-extensions", "connectivity-and-resolution"],
                            "sections": [
                                {
                                    "id": "browsers-supported",
                                    "mandatory": [ "supported-browsers", "mobile-responsive" ],
                                    "data": {
                                        "supported-browsers": [ "Edge", "Google Chrome" ],
                                        "mobile-responsive" : "yes"
                                    },
                                    "requirement": "Mandatory",
                                    "status": "COMPLETE"
                                },
                                {
                                    "mandatory": [],
                                    "id": "plug-ins-or-extensions",
                                    "requirement": "Mandatory",
                                    "status": "INCOMPLETE"
                                },
                                {
                                    "mandatory": [],
                                    "id": "connectivity-and-resolution",
                                    "requirement": "Mandatory",
                                    "status": "INCOMPLETE"
                                },
                                {
                                    "mandatory": [],
                                    "id": "hardware-requirements",
                                    "requirement": "Optional",
                                    "status": "INCOMPLETE"
                                },
                                {
                                    "mandatory": [],
                                    "id": "additional-information",
                                    "requirement": "Optional",
                                    "status": "INCOMPLETE"
                                }
                            ]
                        },
                        {
                            "mandatory": [],
                            "id": "native-mobile",
                            "requirement": "Optional",
                            "status": "INCOMPLETE"
                        },
                        {
                            "mandatory": [],
                            "id": "native-desktop",
                            "requirement": "Optional",
                            "status": "INCOMPLETE"
                        }
                    ]
                }
            ]
        }
    }
}
*/
