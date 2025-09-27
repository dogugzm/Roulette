using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Wheel
{
    public class RouletteWheel : MonoBehaviour
    {
        [SerializeField] private Transform wheelPivot;
        [SerializeField] private float angularSpeed;

        private CancellationTokenSource _cts;

        private void Start()
        {
            StartSpinning();
        }

        public void StartSpinning()
        {
            if (wheelPivot == null)
            {
                Debug.LogWarning("Wheel pivot is not assigned!");
                return;
            }

            StopSpinning();

            _cts = new CancellationTokenSource();
            _ = ContinuousSpin(_cts.Token);
        }

        private void StopSpinning()
        {
            if (_cts is { IsCancellationRequested: false })
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }

        public void SetSpeed(float speed)
        {
            angularSpeed = speed;
        }

        private async Task ContinuousSpin(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (wheelPivot != null)
                {
                    wheelPivot.Rotate(0f, angularSpeed * Time.deltaTime, 0f);
                }

                await Task.Yield();
            }
        }

        private void OnDestroy()
        {
            StopSpinning();
        }
    }
}