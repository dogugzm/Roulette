using UnityEngine;

namespace Wheel
{
    public class PocketNumber : MonoBehaviour
    {
        public int number;
        public Transform centerPoint;

        private void Reset()
        {
            if (centerPoint == null)
            {
                var go = new GameObject("CenterPoint");
                go.transform.SetParent(transform, false);
                go.transform.localPosition = Vector3.zero;
                centerPoint = go.transform;
            }
        }
    }
}