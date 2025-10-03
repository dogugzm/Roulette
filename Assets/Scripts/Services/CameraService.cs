using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    [Serializable]
    public class CameraTransformData
    {
        [field: SerializeField] public GameManager.GameState State { get; set; }
        [field: SerializeField] public Transform Transform { get; set; }
        [field: SerializeField] public float Duration { get; set; }
    }

    public class CameraService : ICameraService
    {
        private readonly Camera _mainCamera;
        private List<CameraTransformData> _cameraTransformDataList;

        public CameraService(List<CameraTransformData> cameraTransformDataList)
        {
            _mainCamera = Camera.main;
            _cameraTransformDataList = cameraTransformDataList;
        }

        public async Task MoveToTransformAsync(GameManager.GameState state, CancellationToken token = default)
        {
            CameraTransformData cameraTransformData =
                _cameraTransformDataList.FirstOrDefault(data => data.State == state);
            if (cameraTransformData == null)
                return;

            float time = 0;
            Transform targetTransform = cameraTransformData.Transform;
            float duration = cameraTransformData.Duration;

            Vector3 startPosition = _mainCamera.transform.position;
            Quaternion startRotation = _mainCamera.transform.rotation;

            while (time < duration)
            {
                if (token.IsCancellationRequested)
                    return;

                _mainCamera.transform.position = Vector3.Lerp(startPosition, targetTransform.position, time / duration);
                _mainCamera.transform.rotation =
                    Quaternion.Lerp(startRotation, targetTransform.rotation, time / duration);
                time += Time.deltaTime;
                await Task.Yield();
            }

            _mainCamera.transform.position = targetTransform.position;
            _mainCamera.transform.rotation = targetTransform.rotation;
        }
    }
}