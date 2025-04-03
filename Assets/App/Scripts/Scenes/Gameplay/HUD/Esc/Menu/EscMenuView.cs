using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.PopupAndViews.Views;
using App.Scripts.Modules.Sounds;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.Gameplay.Esc.Menu
{
    public class EscMenuView : AnimatedView
    {
        public event Action OnContinueButtonClicked;
        public event Action OnSettingsButtonClicked;
        public event Action OnExitButtonClicked;
        
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;
        
        [Header("Audio")]
        [SerializeField] private AudioDatabase _audioDatabase;
        [field: SerializeField,ValueDropdown(@"GetAudioKeys")] public string ButtonSound { get; private set; }
        
        public List<string> GetAudioKeys()
        {
            if (_audioDatabase == null)
            {
                return null;
            }
            return _audioDatabase.Audios.Keys.ToList();
        }
        
        public void Initialize()
        {
            _continueButton.onClick.AddListener(() => OnContinueButtonClicked?.Invoke());
            _settingsButton.onClick.AddListener(() => OnSettingsButtonClicked?.Invoke());
            _exitButton.onClick.AddListener(() => OnExitButtonClicked?.Invoke());
        }
        
        public void ShowWithoutAnimation()
        {
            gameObject.SetActive(true);
        }
        
        public void HideWithoutAnimation()
        {
            gameObject.SetActive(false);
        }
    }
}