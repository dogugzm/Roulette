using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DeterministicUI : MonoBehaviour
    {
        [SerializeField] private Toggle deterministicToggle;
        [SerializeField] private TMP_InputField deterministicInputField;

        public int? SelectedDeterministicValue =>
            deterministicToggle.isOn ? int.Parse(deterministicInputField.text) : null;

        private void Start()
        {
            if (deterministicToggle != null)
            {
                deterministicToggle.onValueChanged.AddListener(OnDeterministicToggleChanged);
            }

            if (deterministicInputField != null)
            {
                deterministicInputField.onEndEdit.AddListener(OnDeterministicInputFieldChanged);
                deterministicInputField.interactable = deterministicToggle.isOn;
            }
        }

        private void OnDeterministicInputFieldChanged(string data)
        {
            if (int.TryParse(data, out int result))
            {
                result = Mathf.Clamp(result, 0, 36);
                deterministicInputField.text = result.ToString();
            }
            else
            {
                deterministicInputField.text = "0";
            }
        }

        private void OnDeterministicToggleChanged(bool data)
        {
            if (deterministicInputField != null)
            {
                deterministicInputField.interactable = data;
            }
        }
    }
}