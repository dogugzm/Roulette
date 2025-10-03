using System.Threading.Tasks;
using UnityEngine;

namespace Managers
{
    public interface IPoolService<T> where T : MonoBehaviour
    {
        Task InitializeAsync(T prefab, int initialSize);
        T Get();
        void Return(T obj);
    }
}