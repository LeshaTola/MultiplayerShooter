using System;
using App.Scripts.Features.Screens;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Screens.MainScreen
{
    public class MainScreen : GameScreen
    {
        public event Action PlayButtonAction;
        public event Action<string> OnPlayerNameChanged; 
        
        [SerializeField] private Button _playButton;

        public override void Initialize()
        {
            _playButton.onClick.AddListener(() => PlayButtonAction?.Invoke());
        }

        public override void Cleanup()
        {
            _playButton.onClick.RemoveAllListeners();
        }
    }
}