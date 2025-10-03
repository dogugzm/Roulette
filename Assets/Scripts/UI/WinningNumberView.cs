using Helper;
using TMPro;
using UnityEngine;

namespace UI
{
    public class WinningNumberView : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        public void Init(int number)
        {
            text.text = number.ToString();
            text.color = BetRules.GetNumberColor(number);
        }
    }
}