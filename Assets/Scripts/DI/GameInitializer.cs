using DefaultNamespace;
using UnityEngine;

namespace DI
{
    public class GameInitializer : MonoBehaviour
    {
        private void Awake()
        {
            ServiceLocator.Register<ISaveLoadService>(new SaveLoadService());
        }
    }
}