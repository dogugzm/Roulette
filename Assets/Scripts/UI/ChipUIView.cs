using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ChipUIView : MonoBehaviour
    {
        public class Data
        {
            public Action clickAction;
        }

        [SerializeField] private ChipSO chipSo;
        [SerializeField] private Button button;
        [SerializeField] private Image image;

        private IAudioManager _audioManager;
        public ChipSO ChipSo => chipSo;

        public async Task InitAsync(Data data)
        {
            image.sprite = chipSo.Sprite;
            _audioManager = ServiceLocator.Get<IAudioManager>();
            button.onClick.AddListener(() =>
            {
                _audioManager.PlaySound(SFXConstants.Click);
                data.clickAction?.Invoke();
            });
        }

        public void SetSelected(bool isSelected)
        {
            float scale = isSelected ? 1.2f : 1.0f;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}