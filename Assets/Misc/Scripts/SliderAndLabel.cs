using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    public sealed class SliderAndLabel : MonoBehaviour
    {
        [SerializeField] private string label;
        [SerializeField] private Slider slider;
        [SerializeField] private Text text;

        public float Value => slider.value;

        private void Awake()
        {
            text.text = $"{label} : {slider.value:F1}";
            slider.onValueChanged.AddListener(value =>
            {
                text.text = $"{label} : {value:F1}";
            });
        }
    }
}
