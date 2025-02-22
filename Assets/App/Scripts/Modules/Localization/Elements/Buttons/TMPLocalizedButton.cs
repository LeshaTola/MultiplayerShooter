using System;
using App.Scripts.Modules.Localization.Localizers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Scripts.Modules.Localization.Elements.Buttons
{
    public class TMPLocalizedButton : MonoBehaviour
    {
        [SerializeField] private TMPLocalizer text;
        [SerializeField] private Button button;

        private UnityAction buttonAction;

        private void OnValidate()
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }
            if (text == null)
            {
                text.GetComponentInChildren <TMPLocalizer>();
            }
        }

        public void Initialize(ILocalizationSystem localizationSystem)
        {
            text.Initialize(localizationSystem);
        }

        public void UpdateAction(Action action)
        {
            if (buttonAction != null)
            {
                button.onClick.RemoveListener(buttonAction);
            }

            buttonAction = new UnityAction(action);
            button.onClick.AddListener(buttonAction);
        }

        public void UpdateText(string key)
        {
            text.Key = key;
        }

        public void Translate()
        {
            text.Translate();
        }

        public void Cleanup()
        {
            text.Cleanup();
            button.onClick.RemoveAllListeners();
            buttonAction = null;
        }
    }
}