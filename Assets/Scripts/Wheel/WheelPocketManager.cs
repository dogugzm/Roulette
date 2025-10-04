using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Wheel
{
    [ExecuteAlways]
    public class WheelPocketManager : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform wheelCenter;

        [SerializeField] private Transform pocketsRoot;
        [SerializeField] private GameObject pocketPrefab;

        [Header("Layout")] public int[] wheelNumbers;
        [SerializeField] private float pocketRadius = 0.9f;
        [SerializeField] private float pocketHeight = 0.02f;
        [SerializeField] private float angleOffsetDeg = 0f;

        // runtime caches
        private readonly List<Transform> pockets = new List<Transform>();
        private readonly Dictionary<int, Transform> pocketsByNumber = new Dictionary<int, Transform>();

        private void Start()
        {
            GeneratePockets();
        }

        [ContextMenu("Generate Pockets")]
        public void GeneratePockets()
        {
            if (wheelCenter == null || pocketsRoot == null || pocketPrefab == null || wheelNumbers == null ||
                wheelNumbers.Length == 0)
            {
                Debug.LogWarning("WheelPocketManager: missing references or wheelNumbers.");
                return;
            }

            // clear old pockets (editor-safe)
            for (int i = pocketsRoot.childCount - 1; i >= 0; i--)
            {
                var ch = pocketsRoot.GetChild(i);
#if UNITY_EDITOR
                if (!Application.isPlaying) DestroyImmediate(ch.gameObject);
                else Destroy(ch.gameObject);
#else
                Destroy(ch.gameObject);
#endif
            }

            pockets.Clear();
            pocketsByNumber.Clear();

            int count = wheelNumbers.Length;
            for (int i = 0; i < count; i++)
            {
                float angleDeg = angleOffsetDeg + i * (360f / count);
                float rad = angleDeg * Mathf.Deg2Rad;
                Vector3 pos = wheelCenter.position + new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * pocketRadius;
                pos.y = wheelCenter.position.y + pocketHeight;

                var go = Instantiate(pocketPrefab, pos, Quaternion.identity, pocketsRoot);
                go.name = $"Pocket_{wheelNumbers[i]}";
                go.transform.LookAt(
                    new Vector3(wheelCenter.position.x, go.transform.position.y, wheelCenter.position.z));

                var pn = go.GetComponent<PocketNumber>();
                if (pn == null) pn = go.AddComponent<PocketNumber>();
                pn.number = wheelNumbers[i];

                if (pn.centerPoint == null)
                {
                    var center = new GameObject("CenterPoint");
                    center.transform.SetParent(go.transform, false);
                    center.transform.localPosition = Vector3.zero;
                    pn.centerPoint = center.transform;
                }

                pockets.Add(go.transform);
                pocketsByNumber[pn.number] = go.transform;
            }

            Debug.Log($"WheelPocketManager: generated {pockets.Count} pockets.");
        }

        public Transform GetPocketTransform(int number)
        {
            pocketsByNumber.TryGetValue(number, out var t);
            return t;
        }

        public int GetRandomPocketNumber()
        {
            if (wheelNumbers == null || wheelNumbers.Length == 0) return -1;
            return wheelNumbers[Random.Range(0, wheelNumbers.Length)];
        }
    }
}