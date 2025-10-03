using System.Collections.Generic;
using ScriptableObject;
using Services;
using Services.Interfaces;
using UnityEngine;
using Wheel;

namespace DI
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private List<CameraTransformData> cameraPositions;
        [SerializeField] private WheelController wheelController;
        [Header("AUDIO")] [SerializeField] private SoundDatabaseSO soundDatabase;

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource fxSource;

        [Header("EFFECTS")] [SerializeField] private GameObject winnerEffect;
        [SerializeField] private GameObject loseEffect;
        [SerializeField] private ChipDatabaseSO chipDatabase;

        private void Awake()
        {
            ServiceLocator.Register<IWheelController>(wheelController);

            ServiceLocator.Register<ISaveLoadService>(new SaveLoadService());

            var bettingManager = new BettingManager(1000);
            ServiceLocator.Register<IBettingManager>(bettingManager);

            var statisticService = new StatisticService();
            ServiceLocator.Register<IStatisticService>(statisticService);

            var audioService = new AudioManager(soundDatabase, musicSource, fxSource);
            ServiceLocator.Register<IAudioManager>(audioService);

            var payoutManager =
                new PayoutManager(bettingManager, statisticService, winnerEffect, audioService, loseEffect);
            ServiceLocator.Register<IPayoutManager>(payoutManager);

            var chipManager = new ChipManager(bettingManager, audioService, chipDatabase);
            ServiceLocator.Register<IChipManager>(chipManager);


            var cameraService = new CameraService(cameraPositions);
            ServiceLocator.Register<ICameraService>(cameraService);
        }
    }
}