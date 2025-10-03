using System.Threading.Tasks;
using UnityEngine;

namespace Services.Interfaces
{
    public interface IPoolService<T> where T : MonoBehaviour
    {
        Task InitializeAsync(T prefab, int initialSize);
        T Get();
        void Return(T obj);
    }
}