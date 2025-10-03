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
        [SerializeField] private GameObject highlightEffect;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void Highlight()
        {
            StopAllCoroutines();
            highlightEffect.SetActive(true);
        }

        public void Reset()
        {
            StopAllCoroutines();
            highlightEffect.SetActive(false);
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