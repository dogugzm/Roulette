using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NumberUIView : MonoBehaviour
    {
        public int number;
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void Highlight()
        {
            StopAllCoroutines();
            StartCoroutine(FadeTo(0.5f, 0.2f));
        }

        public void Reset()
        {
            StopAllCoroutines();
            StartCoroutine(FadeTo(0f, 0.2f));
        }

        private IEnumerator FadeTo(float targetAlpha, float duration)
        {
            float startAlpha = _image.color.a;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                _image.color = new Color(1, 1, 1, alpha);
                yield return null;
            }

            _image.color = new Color(1, 1, 1, targetAlpha); // ensure final value
        }
    }
}