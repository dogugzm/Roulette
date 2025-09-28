using DefaultNamespace;
using UnityEngine;

namespace DI
{
    public class GameInitializer : MonoBehaviour
    {
        private void Awake()
        {
            ServiceLocator.Register<ISaveLoadService>(new SaveLoadService());

            var bettingManager = new BettingManager(1000);
            ServiceLocator.Register<IBettingManager>(bettingManager);

            var payoutManager = new PayoutManager(bettingManager);
            ServiceLocator.Register<IPayoutManager>(payoutManager);

            var chipManager = new UI.ChipManager(bettingManager);
            ServiceLocator.Register<UI.IChipManager>(chipManager);

            IWheelController wheelController = FindObjectOfType<WheelController>();
            ServiceLocator.Register<IWheelController>(wheelController);
        }
    }
}