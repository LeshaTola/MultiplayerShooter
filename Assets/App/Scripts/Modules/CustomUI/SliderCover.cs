using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Modules.CustomToggles
{
    public class SliderCover:MonoBehaviour
    {
        [field: SerializeField] public Slider Slider { get; private set; }
        [SerializeField] private TextMeshProUGUI _valueText;

        private void Awake()
        {
            Slider.onValueChanged.AddListener(SetHandle);
        }

        private void Start()
        {
            SetHandle(Slider.value);
        }

        private void SetHandle(float value)
        {
            _valueText.text = ((int) (value * 100)).ToString();
        }
    }
}