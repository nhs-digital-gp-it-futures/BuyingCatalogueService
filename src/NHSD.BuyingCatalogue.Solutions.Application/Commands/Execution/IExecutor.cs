using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution
{
    internal interface IExecutor<T>
    {
        Task UpdateAsync(T request, CancellationToken cancellationToken);
    }
}
