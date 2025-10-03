using System;
using System.Threading.Tasks;
using DI;
using Services;
using Services.Interfaces;
using UnityEngine;

namespace Wheel
{
    public class WheelController : MonoBehaviour, IWheelController
    {
        [Header("References")] [SerializeField]
        private Transform wheelCenter;

        [SerializeField] private WheelPocketManager pocketManager;

        [Header("Settings")] [SerializeField] private float orbitRadius = 1f;
        [SerializeField] private float orbitSpeed = 50f;
        [SerializeField] private float startDelay = 3f;
        [SerializeField] private float jumpHeight = 0.3f;
        [SerializeField] private float jumpDuration = 0.5f;
        [SerializeField] private int randomJumps = 3;

        public event Action<int> OnSpinComplete;

        private bool isSpinning = false;
        private IAudioManager _audioManager;

        private void Start()
        {
            _audioManager = ServiceLocator.Get<IAudioManager>();
        }


        public void Spin()
        {
            if (isSpinning)
            {
                Debug.LogWarning("Wheel is already spinning.");
                return;
            }

            RunBallRoutineAsync(1);
        }

        private async void RunBallRoutineAsync(int? deterministicNumber = null)
        {
            _audioManager.PlaySound(SFXConstants.BallSpin, 0.3f);
            isSpinning = true;
            float elapsed = 0f;
            float angle = 0f;

            while (elapsed < startDelay)
            {
                angle += orbitSpeed * Time.deltaTime;
                float rad = angle * Mathf.Deg2Rad;
                transform.position =
                    wheelCenter.position + new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * orbitRadius;
                elapsed += Time.deltaTime;
                await Task.Yield();
            }

            var selectedNumber = deterministicNumber ?? pocketManager.GetRandomPocketNumber();
            Transform targetPocket = pocketManager.GetPocketTransform(selectedNumber);

            if (targetPocket == null)
            {
                Debug.LogError($"Target pocket for number {selectedNumber} not found!");
                isSpinning = false;
                return;
            }

            Debug.Log("Targeting pocket: " + selectedNumber);

            float orbitTargetDistance = 4f;
            while (Vector3.Distance(transform.position, targetPocket.position) > orbitTargetDistance)
            {
                angle += orbitSpeed * Time.deltaTime;
                float rad = angle * Mathf.Deg2Rad;
                transform.position =
                    wheelCenter.position + new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * orbitRadius;
                await Task.Yield();
            }

            _ = _audioManager.StopSoundFadedAsync(SFXConstants.BallSpin, 0.5f);

            for (int i = 0; i < randomJumps; i++)
            {
                await JumpToDynamicTargetAsync(targetPocket, 0.6f);
            }

            await JumpToDynamicTargetAsync(targetPocket, 0f);

            transform.SetParent(targetPocket, true);

            OnSpinComplete?.Invoke(selectedNumber);

            isSpinning = false;
        }


        private async Task JumpToDynamicTargetAsync(Transform target, float offsetRange)
        {
            Vector3 startPos = transform.position;
            float elapsed = 0f;

            Vector3 randomOffset = offsetRange > 0f
                ? new Vector3(UnityEngine.Random.Range(-offsetRange, offsetRange), 0f,
                    UnityEngine.Random.Range(-offsetRange, offsetRange))
                : Vector3.zero;

            while (elapsed < jumpDuration)
            {
                float t = elapsed / jumpDuration;
                Vector3 currentTarget = target.position + randomOffset;
                float yOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
                transform.position = Vector3.Lerp(startPos, currentTarget, t) + Vector3.up * yOffset;
                elapsed += Time.deltaTime;
                await Task.Yield();
            }

            _audioManager.PlaySound(SFXConstants.BallDrop);
            transform.position = target.position + randomOffset;
        }
    }
}