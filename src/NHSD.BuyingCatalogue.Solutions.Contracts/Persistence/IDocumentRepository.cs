using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IDocumentRepository
    {
        Task<IDocumentResult> GetDocumentResultBySolutionIdAsync(string solutionId, CancellationToken cancellationToken);
    }
}
