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

        [SerializeField] private Chip chip;
        [SerializeField] private Button button;
        [SerializeField] private Image image;

        public Chip Chip => chip;

        public async Task InitAsync(Data data)
        {
            image.sprite = chip.Sprite;
            button.onClick.AddListener(() => data.clickAction?.Invoke());
        }

        public void SetSelected(bool isSelected)
        {
            float scale = isSelected ? 1.2f : 1.0f;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}