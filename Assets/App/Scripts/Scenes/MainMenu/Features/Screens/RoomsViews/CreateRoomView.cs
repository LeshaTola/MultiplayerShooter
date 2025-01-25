using App.Scripts.Modules.PopupAndViews.Views;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.RoomsViews
{
    public class CreateRoomView : AnimatedView
    {
        [SerializeField] private TMP_InputField _serverNameInputField;
        [SerializeField] private TMP_InputField _playersInputField;
        [SerializeField] private TMP_InputField _passwordInputField;

        [SerializeField] private TMP_Dropdown _modeDropdown;
        [SerializeField] private Image _mapImage;

        [Header("Buttons")]
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _closeButton;

        public const int MIN_SERVER_NAME_LENGTH = 3;
        public const int MAX_SERVER_NAME_LENGTH = 16;
        public const int MAX_PASSWORD_LENGTH = 6;
        public const int MAX_PLAYERS = 10;

        private void OnEnable()
        {
            _createButton.onClick.AddListener(CreateRoom);
            _closeButton.onClick.AddListener(HideYourself);

            _serverNameInputField.onValueChanged.AddListener(ValidateServerName);
            _passwordInputField.onValueChanged.AddListener(ValidatePassword);
            _playersInputField.onValueChanged.AddListener(ValidatePlayers);
        }

        private async void HideYourself()
        {
            await Hide();
        }

        private void OnDisable()
        {
            _createButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();

            _serverNameInputField.onValueChanged.RemoveListener(ValidateServerName);
            _passwordInputField.onValueChanged.RemoveListener(ValidatePassword);
            _playersInputField.onValueChanged.RemoveListener(ValidatePlayers);
        }

        private void ValidateServerName(string input)
        {
            if (input.Length > MAX_SERVER_NAME_LENGTH)
            {
                _serverNameInputField.text = input.Substring(0, MAX_SERVER_NAME_LENGTH);
            }
        }

        private void ValidatePassword(string input)
        {
            if (input.Length > MAX_PASSWORD_LENGTH)
            {
                _passwordInputField.text = input.Substring(0, MAX_PASSWORD_LENGTH);
            }
        }

        private void ValidatePlayers(string input)
        {
            if (int.TryParse(input, out int players))
            {
                if (players > MAX_PLAYERS)
                {
                    _playersInputField.text = MAX_PLAYERS.ToString();
                }
            }
            else
            {
                _playersInputField.text = ""; // Очищаем поле, если введены нечисловые значения
            }
        }

        private void CreateRoom()
        {
            string serverName = _serverNameInputField.text;
            string password = _passwordInputField.text;
            string playersInput = _playersInputField.text;

            if (!IsServerNameValid(serverName) || !IsPasswordValid(password) ||
                !IsPlayerCountValid(playersInput, out int maxPlayers))
            {
                return;
            }

            var options = new RoomOptions
            {
                MaxPlayers = (byte)maxPlayers,
                IsOpen = true,
                IsVisible = true,
                CustomRoomProperties = string.IsNullOrEmpty(password)
                    ? new ExitGames.Client.Photon.Hashtable()
                    : new ExitGames.Client.Photon.Hashtable { { "Password", password } },
                CustomRoomPropertiesForLobby = string.IsNullOrEmpty(password)
                    ? new string[] { }
                    : new[] { "Password" }
            };

            PhotonNetwork.CreateRoom(serverName, options);
        }

        private bool IsServerNameValid(string serverName)
        {
            if (string.IsNullOrWhiteSpace(serverName) || serverName.Length < MIN_SERVER_NAME_LENGTH || serverName.Length > MAX_SERVER_NAME_LENGTH)
            {
                Debug.LogError($"Server name must be between {MIN_SERVER_NAME_LENGTH} and {MAX_SERVER_NAME_LENGTH} characters.");
                return false;
            }
            return true;
        }

        private bool IsPasswordValid(string password)
        {
            if (!string.IsNullOrEmpty(password) && password.Length < MAX_PASSWORD_LENGTH)
            {
                Debug.LogError($"Password must be at least {MAX_PASSWORD_LENGTH} characters long.");
                return false;
            }
            return true;
        }

        private bool IsPlayerCountValid(string playersInput, out int maxPlayers)
        {
            maxPlayers = 0;
            if (!int.TryParse(playersInput, out maxPlayers) || maxPlayers <= 0 || maxPlayers > MAX_PLAYERS)
            {
                Debug.LogError($"Player count must be a number between 1 and {MAX_PLAYERS}.");
                return false;
            }
            return true;
        }
    }
}
