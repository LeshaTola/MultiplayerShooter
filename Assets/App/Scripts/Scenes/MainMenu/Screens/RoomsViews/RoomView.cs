using System;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Screens.RoomsViews
{
    public class RoomView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _serverNameText;
        [SerializeField] private TextMeshProUGUI _mapText;
        [SerializeField] private TextMeshProUGUI _modeText;
        [SerializeField] private TextMeshProUGUI _playersCountText;
        
        [SerializeField] private Image _closedImage;
        [SerializeField] private Button _joinButton;

        public void Setup(RoomInfo roomInfo,string mode, Action onClick)
        {
            Cleanup();
            _serverNameText.text = roomInfo.Name;

            _playersCountText.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
            _closedImage.gameObject.SetActive(roomInfo.CustomProperties.TryGetValue("Password", out _));
            _modeText.text = mode;
            
            _joinButton.onClick.AddListener(() =>
            {
                onClick?.Invoke();
            });
        }


        private void Cleanup()
        {
            _joinButton.onClick.RemoveAllListeners();
        }
    }
}