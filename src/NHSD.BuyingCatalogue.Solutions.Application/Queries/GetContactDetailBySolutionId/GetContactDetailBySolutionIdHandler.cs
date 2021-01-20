using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetContactDetailBySolutionId
{
    internal sealed class GetContactDetailBySolutionIdHandler : IRequestHandler<GetContactDetailBySolutionIdQuery, IEnumerable<IContact>>
    {
        private readonly ContactDetailsReader contactDetailsReader;
        private readonly SolutionVerifier solutionVerifier;
        private readonly IMapper mapper;

        public GetContactDetailBySolutionIdHandler(
            ContactDetailsReader contactDetailsReader,
            SolutionVerifier solutionVerifier,
            IMapper mapper)
        {
            this.contactDetailsReader = contactDetailsReader;
            this.solutionVerifier = solutionVerifier;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<IContact>> Handle(
            GetContactDetailBySolutionIdQuery request,
            CancellationToken cancellationToken)
        {
            await solutionVerifier.ThrowWhenMissingAsync(request.Id, cancellationToken);
            return mapper.Map<IEnumerable<IContact>>(await contactDetailsReader.ByIdAsync(request.Id, cancellationToken));
        }
    }
}
