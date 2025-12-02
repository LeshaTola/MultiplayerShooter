using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features;
using App.Scripts.Features.GameMods.Configs;
using App.Scripts.Features.GameMods.Providers;
using App.Scripts.Features.Match.Configs;
using App.Scripts.Features.Match.Maps;
using App.Scripts.Modules.Localization.Localizers;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Modules.PopupAndViews.Views;
using App.Scripts.Modules.Sounds;
using App.Scripts.Modules.Sounds.Providers;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
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

        [SerializeField] private TMP_InputField _serverNameInputField;
        [SerializeField] private TMP_InputField _playersInputField;
        [SerializeField] private TMP_InputField _passwordInputField;

        [SerializeField] private TMP_Dropdown _modeDropdown;

        [Header("Map")]
        [SerializeField] private Image _mapImage;
        [SerializeField] private TMPLocalizer _mapNameText;

        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _prevButton;

        private int _mapIndex = 0;
        private GameModConfig _currentGameModConfig;

        [Header("Buttons")]
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _closeButton;

                
        [Header("Audio")]
        [SerializeField] private AudioDatabase _audioDatabase;
        [field: SerializeField,ValueDropdown(@"GetAudioKeys")] public string ButtonSoundKey { get; private set; }
        
        private InfoPopupRouter _infoPopupRouter;
        private GameModProvider _gameModProvider;
        private MapsProvider _mapsProvider;
        private ISoundProvider _soundProvider;
        private int _playersMax;
        private int _playersMin;

        [Inject]
        public void Construct(InfoPopupRouter infoPopupRouter,
            MapsProvider mapsProvider,
            GameModProvider gameModProvider,
            ISoundProvider soundProvider)
        {
            _infoPopupRouter = infoPopupRouter;
            _mapsProvider = mapsProvider;
            _gameModProvider = gameModProvider;
            _soundProvider = soundProvider;
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
            
            SetupGameMods();

            _mapIndex = 0;
            UpdateMapUI();
            _serverNameInputField.text = $"Room_{Random.Range(0, 1000)}";
            _passwordInputField.text = "";
            _playersInputField.text = _playersMax.ToString();
        }

        private void OnDisable()
        {
            _createButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();

            _nextButton.onClick.RemoveAllListeners();
            _prevButton.onClick.RemoveAllListeners();

            _modeDropdown.onValueChanged.RemoveAllListeners();
            
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
                if (players > _playersMax)
                {
                    _playersInputField.text = _playersMax.ToString();
                }

                if (players < _playersMin)
                {
                    _playersInputField.text = _playersMin.ToString();
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

            _soundProvider.PlayOneShotSound(ButtonSoundKey);
            if (!await ValidateServerSettings(serverName, password, playersInput))
            {
                return;
            }

            TryParsePlayerCount(playersInput, out int maxPlayers);
            var options = SetupOptions(maxPlayers);
            AddPassword(password, options);
            
            _mapsProvider.Map = _mapsProvider.Config.Maps[_mapIndex].Prefab;
            PhotonNetwork.CreateRoom(serverName, options);
        }

        private RoomOptions SetupOptions(int maxPlayers)
        {
            var options = new RoomOptions
            {
                MaxPlayers = (byte) maxPlayers,
                IsOpen = true,
                IsVisible = true,
                CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
                {
                    {"Map", _mapsProvider.Config.Maps[_mapIndex].Name},
                    {"GameMode", _gameModProvider.CurrentGameMod.Name}
                },
                CustomRoomPropertiesForLobby = new[] {"Map", "GameMode"}
            };
            return options;
        }

        private void AddPassword(string password, RoomOptions options)
        {
            if (!string.IsNullOrEmpty(password))
            {
                options.CustomRoomProperties.Add("Password", password);
                options.CustomRoomPropertiesForLobby = options.CustomRoomPropertiesForLobby.Append("Password").ToArray();
            }
        }

        private async UniTask<bool> IsServerNameValid(string serverName)
        {
            if (string.IsNullOrWhiteSpace(serverName) || serverName.Length < MIN_SERVER_NAME_LENGTH ||
                serverName.Length > MAX_SERVER_NAME_LENGTH)
            {
                await _infoPopupRouter.ShowPopup(
                    ConstStrings.ERROR,
                    $"Длина названия сервера должна быть между {MIN_SERVER_NAME_LENGTH} и {MAX_SERVER_NAME_LENGTH} символами.");
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
                    ConstStrings.ERROR,
                    $"Длина пароля должна быть между {MIN_PASSWORD_LENGTH} и {MAX_PASSWORD_LENGTH} символами.");

                return false;
            }

            return true;
        }

        private bool TryParsePlayerCount(string playersInput, out int maxPlayers)
        {
            return int.TryParse(playersInput, out maxPlayers) && maxPlayers >= _playersMin && maxPlayers <= _playersMax;
        }

        private async UniTask<bool> ValidateServerSettings(string serverName, string password, string playersInput)
        {
            if (!await IsServerNameValid(serverName)) return false;
            if (!await IsPasswordValid(password)) return false;

            return true;
        }

        private void NextMap()
        {
            if (_mapIndex < _mapsProvider.Config.Maps.Count - 1)
            {
                _soundProvider.PlayOneShotSound(ButtonSoundKey);
                _mapIndex++;
                UpdateMapUI();
            }
        }

        private void PrevMap()
        {
            if (_mapIndex > 0)
            {
                _soundProvider.PlayOneShotSound(ButtonSoundKey);
                _mapIndex--;
                UpdateMapUI();
            }
        }

        private void UpdateMapUI()
        {
            var map = _mapsProvider.Config.Maps[_mapIndex];

            _mapNameText.Key = map.Name;
            _mapNameText.Translate();
            _mapImage.sprite = map.Sprite;

            _prevButton.interactable = _mapIndex > 0;
            _nextButton.interactable = _mapIndex < _mapsProvider.Config.Maps.Count - 1;
        }
        
        private List<string> GetAudioKeys()
        {
            if (_audioDatabase == null)
            {
                return null;
            }
            return _audioDatabase.Audios.Keys.ToList();
        }
        
        private void SetupGameMods()
        {
            _modeDropdown.ClearOptions();
            _modeDropdown.AddOptions(_gameModProvider.GameMods.Select(x => x.Name).ToList());
            _modeDropdown.onValueChanged.AddListener(ChangeGameMode);
            ChangeGameMode(0);
        }

        private void ChangeGameMode(int id)
        {
            _gameModProvider.SetGameMod(_gameModProvider.GameMods[id]);
            _mapIndex = 0;
            _playersMax = _gameModProvider.CurrentGameMod.Players.Max;
            _playersMin = _gameModProvider.CurrentGameMod.Players.Min;

            UpdateMapUI();
        }

        private async void HideYourself()
        {
            _soundProvider.PlayOneShotSound(ButtonSoundKey);
            await Hide();
        }
    }
}