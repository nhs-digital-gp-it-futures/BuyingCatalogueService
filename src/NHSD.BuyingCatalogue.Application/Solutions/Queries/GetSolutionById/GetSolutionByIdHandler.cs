using AutoMapper;
using MediatR;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById
{
    public sealed class GetSolutionByIdHandler : IRequestHandler<GetSolutionByIdQuery, GetSolutionByIdResult>
    {
        private ISolutionRepository _solutionsRepository { get; }
        private IMapper _mapper { get; }

        public GetSolutionByIdHandler(ISolutionRepository solutionsRepository, IMapper mapper)
        {
            _solutionsRepository = solutionsRepository;
            _mapper = mapper;
        }

        public async Task<GetSolutionByIdResult> Handle(GetSolutionByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _solutionsRepository.ByIdAsync(request.Id, cancellationToken);

            return new GetSolutionByIdResult
            {
                Solution = _mapper.Map<SolutionByIdViewModel>(result)
            };
        }
    }
}
