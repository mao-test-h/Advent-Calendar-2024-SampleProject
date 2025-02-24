using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    public sealed class SliderAndLabel : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Text text;

        private string _label;

        public float Value => slider.value;

        private void Awake()
        {
            _label = name;
            text.text = $"{_label} : {slider.value:F1}";
            slider.onValueChanged.AddListener(value =>
            {
                text.text = $"{_label} : {value:F1}";
            });
        }
    }
}
