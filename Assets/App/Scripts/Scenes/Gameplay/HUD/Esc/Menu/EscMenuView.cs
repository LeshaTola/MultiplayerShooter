using System;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.Gameplay.Esc.Menu
{
    public class EscMenuView : MonoBehaviour
    {
        public event Action OnContinueButtonClicked;
        public event Action OnSettingsButtonClicked;
        public event Action OnExitButtonClicked;
        
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;
        
        public void Initialize()
        {
            _continueButton.onClick.AddListener(() => OnContinueButtonClicked?.Invoke());
            _settingsButton.onClick.AddListener(() => OnSettingsButtonClicked?.Invoke());
            _exitButton.onClick.AddListener(() => OnExitButtonClicked?.Invoke());
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

    }
}