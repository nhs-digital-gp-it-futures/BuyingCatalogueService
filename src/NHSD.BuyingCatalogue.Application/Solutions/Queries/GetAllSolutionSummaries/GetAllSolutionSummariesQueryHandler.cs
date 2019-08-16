using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Domain;
using NHSD.BuyingCatalogue.Domain.Entities;
using MediatR;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAll
{
	/// <summary>
	/// Defines the request handler for the <see cref="GetAllSolutionSummariesQuery"/>.
	/// </summary>
	public sealed class GetAllSolutionSummariesQueryHandler : IRequestHandler<GetAllSolutionSummariesQuery, GetAllSolutionSummariesQueryResult>
	{
		/// <summary>
		/// Access the persistence layer for the <see cref="Solution"/> entity.
		/// </summary>
		public ISolutionRepository SolutionsRepository { get; }

		/// <summary>
		/// Initialises a new instance of the <see cref="GetAllSolutionSummariesQueryHandler"/> class.
		/// </summary>
		public GetAllSolutionSummariesQueryHandler(ISolutionRepository solutionsRepository)
		{
			SolutionsRepository = solutionsRepository ?? throw new ArgumentNullException(nameof(solutionsRepository));
		}

		/// <summary>
		/// Gets the query result.
		/// </summary>
		/// <param name="request">The query parameters.</param>
		/// <param name="cancellationToken">Token to cancel the request.</param>
		/// <returns>The result of the query.</returns>
		public async Task<GetAllSolutionSummariesQueryResult> Handle(GetAllSolutionSummariesQuery request, CancellationToken cancellationToken)
		{
			IEnumerable<Solution> solutionList = await SolutionsRepository.ListSolutionSummaryAsync(cancellationToken).ConfigureAwait(false);

			return new GetAllSolutionSummariesQueryResult
			{
				Solutions = solutionList.Select(Map).Where(item => item != null)
			};
		}

		private SolutionSummaryViewModel Map(Solution item)
		{
			if (item is null)
			{
				throw new ArgumentNullException(nameof(item));
			}

			SolutionSummaryViewModel returnSolutionSummaryViewModel = null;

			OrganisationViewModel organisationViewModel = Map(item.Organisation);
			IEnumerable<CapabilityViewModel> capabilityViewModelList = Map(item.Capabilities);

			if (!(organisationViewModel is null) && capabilityViewModelList.Any())
			{
				returnSolutionSummaryViewModel = new SolutionSummaryViewModel
				{
					Id = item.Id,
					Name = item.Name,
					Summary = item.Summary,
					Organisation = organisationViewModel,
					Capabilities = capabilityViewModelList
				};
			}

			return returnSolutionSummaryViewModel;
		}

		private OrganisationViewModel Map(Organisation organisation)
		{
			OrganisationViewModel returnOrganisationViewModel = null;

			if (!(organisation is null))
			{
				returnOrganisationViewModel = new OrganisationViewModel
				{
					Id = organisation.Id,
					Name = organisation.Name
				};
			}

			return returnOrganisationViewModel;
		}

		private IEnumerable<CapabilityViewModel> Map(IEnumerable<Capability> capabilityList)
		{
			if (!(capabilityList is null) && capabilityList.Any())
			{
				foreach (Capability capability in capabilityList)
				{
					yield return new CapabilityViewModel
					{
						Id = capability.Id,
						Name = capability.Name
					};
				}
			}
		}
	}
}
