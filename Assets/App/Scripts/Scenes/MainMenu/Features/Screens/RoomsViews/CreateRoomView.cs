using App.Scripts.Features.Match.Configs;
using App.Scripts.Features.Match.Maps;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Modules.PopupAndViews.Views;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.RoomsViews
{
    public class CreateRoomView : AnimatedView
    {
        public const int MIN_SERVER_NAME_LENGTH = 3;
        public const int MAX_SERVER_NAME_LENGTH = 16;
        public const int MIN_PASSWORD_LENGTH = 4;
        public const int MAX_PASSWORD_LENGTH = 6;
        public const int MAX_PLAYERS = 10;

        [SerializeField] private TMP_InputField _serverNameInputField;
        [SerializeField] private TMP_InputField _playersInputField;
        [SerializeField] private TMP_InputField _passwordInputField;

        [SerializeField] private TMP_Dropdown _modeDropdown;
        
        [Header("Map")]
        [SerializeField] private MapsConfig _mapsConfig;
        [SerializeField] private Image _mapImage;
        [SerializeField] private TextMeshProUGUI _mapNameText;
        
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _prevButton;
        
        private int _mapIndex = 0;
        
        [Header("Buttons")]
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _closeButton;
        
        private InfoPopupRouter _infoPopupRouter;
        private MapsProvider _mapsProvider;
        
        [Inject]
        public void Construct(InfoPopupRouter infoPopupRouter,
            MapsProvider mapsProvider)
        {
            _infoPopupRouter = infoPopupRouter;
            _mapsProvider = mapsProvider;
        }
        
        private void OnEnable()
        {
            _createButton.onClick.AddListener(CreateRoom);
            _closeButton.onClick.AddListener(HideYourself);

            _serverNameInputField.onValueChanged.AddListener(ValidateServerName);
            _passwordInputField.onValueChanged.AddListener(ValidatePassword);
            _playersInputField.onValueChanged.AddListener(ValidatePlayers);
            
            _nextButton.onClick.AddListener(NextMap);
            _prevButton.onClick.AddListener(PrevMap);

            _mapIndex = 0;
            UpdateMapUI();
            _serverNameInputField.text = $"Room_{Random.Range(0, 1000)}";
            _passwordInputField.text = "";
            _playersInputField.text = MAX_PLAYERS.ToString();
        }

        private async void HideYourself()
        {
            await Hide();
        }

        private void OnDisable()
        {
            _createButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();

            _nextButton.onClick.RemoveAllListeners();
            _prevButton.onClick.RemoveAllListeners();
            
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
                _playersInputField.text = "";
            }
        }

        private async void CreateRoom()
        {
            string serverName = _serverNameInputField.text;
            string password = _passwordInputField.text;
            string playersInput = _playersInputField.text;

            if (!await ValidateServerSettings(serverName, password, playersInput)) 
            {
                return;
            }

            TryParsePlayerCount(playersInput,out int maxPlayers);
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
            
            _mapsProvider.Map = _mapsConfig.Maps[_mapIndex].Prefab;
            PhotonNetwork.CreateRoom(serverName, options);
        }

        private async UniTask<bool> IsServerNameValid(string serverName)
        {
            if (string.IsNullOrWhiteSpace(serverName) || serverName.Length < MIN_SERVER_NAME_LENGTH || serverName.Length > MAX_SERVER_NAME_LENGTH)
            {
                await _infoPopupRouter.ShowPopup(
                    "Ошибка",
                    $"Длинна названия сервера должна быть между {MIN_SERVER_NAME_LENGTH} и {MAX_SERVER_NAME_LENGTH} символами.");
                return false;
            }
            return true;
        }

        private async UniTask<bool> IsPasswordValid(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return true;
            }
            
            if (password.Length < MIN_PASSWORD_LENGTH || password.Length > MAX_PASSWORD_LENGTH)
            {
                await _infoPopupRouter.ShowPopup(
                    "Ошибка",
                    $"Длинна пароля должна быть между {MIN_PASSWORD_LENGTH} и {MAX_PASSWORD_LENGTH} символами.");

                return false;
            }
            
            return true;
        }

        private bool TryParsePlayerCount(string playersInput, out int maxPlayers)
        {
            return int.TryParse(playersInput, out maxPlayers) && maxPlayers > 0 && maxPlayers <= MAX_PLAYERS;
        }

        private async UniTask<bool> IsPlayerCountValid(string playersInput)
        {
            if (!TryParsePlayerCount(playersInput, out _))
            {
                await _infoPopupRouter.ShowPopup(
                    "Ошибка",
                    $"Количество игроков должно быть между 1 и {MAX_PLAYERS}.");
                return false;
            }
            return true;
        }
        
        private async UniTask<bool> ValidateServerSettings(string serverName, string password, string playersInput)
        {
            if (!await IsServerNameValid(serverName)) return false;
            if (!await IsPasswordValid(password)) return false;

            var isPlayerCountValid= await IsPlayerCountValid(playersInput);
            if (!isPlayerCountValid) return false;

            return true;
        }
        
        

        private void NextMap()
        {
            if (_mapIndex < _mapsConfig.Maps.Count - 1)
            {
                _mapIndex++;
                UpdateMapUI();
            }
        }

        private void PrevMap()
        {
            if (_mapIndex > 0)
            {
                _mapIndex--;
                UpdateMapUI();
            }
        }

        private void UpdateMapUI()
        {
            var map = _mapsConfig.Maps[_mapIndex];

            _mapNameText.text = map.Name;
            _mapImage.sprite = map.Sprite;

            _prevButton.interactable = _mapIndex > 0;
            _nextButton.interactable = _mapIndex < _mapsConfig.Maps.Count - 1;
        }
    }
}
