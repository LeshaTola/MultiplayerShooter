using System;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Localization.Localizers;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.RoomsViews
{
    public class RoomView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _serverNameText;
        [SerializeField] private TMPLocalizer _mapText;
        [SerializeField] private TextMeshProUGUI _modeText;
        [SerializeField] private TextMeshProUGUI _playersCountText;
        
        [SerializeField] private Image _closedImage;
        [SerializeField] private Button _joinButton;

        public void Initialize(ILocalizationSystem localizationSystem)
        {
            _mapText.Initialize(localizationSystem);
        }

        public void Cleanup()
        {
            _mapText.Cleanup();
        }
        
        public void Setup(RoomInfo roomInfo, Action onClick)
        {
            Default();
            _serverNameText.text = roomInfo.Name;

            _playersCountText.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
            _closedImage.gameObject.SetActive(roomInfo.CustomProperties.TryGetValue("Password", out _));
            _ = roomInfo.CustomProperties.TryGetValue("Map", out var map);
            _ = roomInfo.CustomProperties.TryGetValue("GameMode", out var gameMode);
            _modeText.text = gameMode as string;
            
            _mapText.Key = map as string;
            _mapText.Translate();
            
            _joinButton.onClick.AddListener(() =>
            {
                onClick?.Invoke();
            });
        }


        private void Default()
        {
            _joinButton.onClick.RemoveAllListeners();
        }
    }
}