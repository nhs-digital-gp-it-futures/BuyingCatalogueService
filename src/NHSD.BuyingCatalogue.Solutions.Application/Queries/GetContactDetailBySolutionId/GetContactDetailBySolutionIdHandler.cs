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
        private readonly ContactDetailsReader _contactDetailsReader;
        private readonly SolutionVerifier _solutionVerifier;
        private readonly IMapper _mapper;

        public GetContactDetailBySolutionIdHandler(ContactDetailsReader contactDetailsReader, SolutionVerifier solutionVerifier, IMapper mapper)
        {
            _contactDetailsReader = contactDetailsReader;
            _solutionVerifier = solutionVerifier;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IContact>> Handle(GetContactDetailBySolutionIdQuery request, CancellationToken cancellationToken)
        {
            await _solutionVerifier.ThrowWhenMissingAsync(request.Id, cancellationToken).ConfigureAwait(false);
            return _mapper.Map<IEnumerable<IContact>>(await _contactDetailsReader.ByIdAsync(request.Id, cancellationToken).ConfigureAwait(false));
        }
    }
}
