using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace App.Scripts.Modules.CustomToggles
{
    public class SliderCover:MonoBehaviour
    { 
        [field: SerializeField] public Slider Slider { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ValueText { get; private set; }

        [SerializeField] private bool _isAutoHandle = true;
        
        private Action<float> _handleAction;
        
        private void Awake()
        {
            if (_isAutoHandle)
            {
                SetCastomHandleMethod((value)=> ValueText.text = ((int) (value * 100)).ToString());
            }
            Slider.onValueChanged.AddListener(SetHandle);
        }

        private void Start()
        {
            if (_isAutoHandle)
            {
                SetHandle(Slider.value);
            }
        }

        private void SetHandle(float value)
        {
            _handleAction?.Invoke(value);
        }

        public void SetCastomHandleMethod(Action<float> action)
        {
            _handleAction = action;
        }
    }
}