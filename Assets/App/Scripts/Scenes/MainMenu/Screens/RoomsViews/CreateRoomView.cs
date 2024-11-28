using App.Scripts.Modules.PopupAndViews.Views;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Screens.RoomsViews
{
    public class CreateRoomView : AnimatedView
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _closeButton;

        private void OnEnable()
        {
            _createButton.onClick.AddListener(CreateRoom);
            _closeButton.onClick.AddListener(HideYourself);
        }

        private async void HideYourself()
        {
            await Hide();
        }

        private void OnDisable()
        {
            _createButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
        }

        private void CreateRoom()
        {
            var options = new RoomOptions()
            {
                MaxPlayers = 10,
                IsVisible = true,
                IsOpen = true
            };

            PhotonNetwork.CreateRoom(_inputField.text, options);
        }
    }
}