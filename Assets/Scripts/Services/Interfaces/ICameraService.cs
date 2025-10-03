using System.Threading;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICameraService
    {
        Task MoveToTransformAsync(GameManager.GameState state,
            CancellationToken token);
    }
}