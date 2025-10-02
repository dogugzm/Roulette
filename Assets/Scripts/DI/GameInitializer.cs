using System.Collections.Generic;
using Services;
using UnityEngine;

namespace DI
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private List<CameraTransformData> cameraPositions;
        [SerializeField] private SoundDatabaseSO soundDatabase;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource fxSource;

        private void Awake()
        {
            ServiceLocator.Register<ISaveLoadService>(new SaveLoadService());

            var bettingManager = new BettingManager(1000);
            ServiceLocator.Register<IBettingManager>(bettingManager);

            var statisticService = new StatisticService();
            ServiceLocator.Register<IStatisticService>(statisticService);

            var payoutManager = new PayoutManager(bettingManager, statisticService);
            ServiceLocator.Register<IPayoutManager>(payoutManager);

            var audioService = new SfxManager(soundDatabase, musicSource, fxSource);
            ServiceLocator.Register<ISfxManager>(audioService);

            var chipManager = new UI.ChipManager(bettingManager, audioService);
            ServiceLocator.Register<UI.IChipManager>(chipManager);

            IWheelController wheelController = FindObjectOfType<WheelController>();
            ServiceLocator.Register<IWheelController>(wheelController);

            var cameraService = new CameraService(cameraPositions);
            ServiceLocator.Register<ICameraService>(cameraService);
        }
    }
}