using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Managers
{
    public class PoolService<T> : IPoolService<T> where T : MonoBehaviour
    {
        private readonly Queue<T> _pool = new();
        private T _prefab;
        private Transform _parent;
        
        
        public async Task InitializeAsync(T prefab, int initialSize)
        {
            _prefab = prefab;
            _parent = new GameObject($"{typeof(T).Name}Pool").transform;

            for (int i = 0; i < initialSize; i++)
            {
                var obj = Object.Instantiate(_prefab, _parent);
                obj.gameObject.SetActive(false);
                _pool.Enqueue(obj);
            }

            await Task.CompletedTask;
        }

        public T Get()
        {
            if (_pool.Count > 0)
            {
                var obj = _pool.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            var newObj = Object.Instantiate(_prefab, _parent);
            return newObj;
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}