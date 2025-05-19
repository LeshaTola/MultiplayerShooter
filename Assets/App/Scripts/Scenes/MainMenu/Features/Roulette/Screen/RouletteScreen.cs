using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Rewards.Configs;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Sounds;
using App.Scripts.Scenes.MainMenu.Features.Roulette.Configs;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Features.Roulette.Screen
{
    public class RouletteScreen : GameScreen
    {
        public event Action SpinButtonPressed;
        public event Action RefreshButtonPressed;

        [SerializeField] private Image _screenBlocker;
        [SerializeField] private Button _spinButton;
        [SerializeField] private Button _refreshButton;
        [SerializeField] private TextMeshProUGUI _ticketsText;

        [SerializeField] private AudioDatabase _audioDatabase;
        [field: SerializeField,ValueDropdown(@"GetAudioKeys")] public string ButtonSound { get; private set; }
        [field: SerializeField,ValueDropdown(@"GetAudioKeys")] public string RouletteStepSond { get; private set; }
        
        [Header("Sectors")]
        [SerializeField] private SectorView _sectorViewPrefab;
        [SerializeField] private RectTransform _sectorViewContainer;
        
        [Header("Win Item Conteiners")]
        [SerializeField] private WinItemContainerView _winItemContainerPrefab;
        [SerializeField] private RectTransform _winItemContainerContainer;

        private ILocalizationSystem _localizationSystem;
        private List<SectorView> _sectorViews = new();
        
        [Inject]
        public void Construct(ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
        }

        public override void Initialize()
        {
            _spinButton.onClick.AddListener(()=>SpinButtonPressed?.Invoke());
            _refreshButton.onClick.AddListener(()=>RefreshButtonPressed?.Invoke());
        }

        public override void Cleanup()
        {
            _spinButton.onClick.RemoveAllListeners();
            _refreshButton.onClick.RemoveAllListeners();
            DefaultSectors();
            DefaultWinItems();
        }

        public void SetupWinItems(Dictionary<SectorConfig, List<RewardConfig>> winItems)
        {
            DefaultWinItems();
    
            foreach (var kvp in winItems)
            {
                var sectorColor = kvp.Key.Color;
        
                foreach (var reward in kvp.Value)
                {
                    var container = Instantiate(_winItemContainerPrefab, _winItemContainerContainer);
                    container.Setup(sectorColor, reward);
                }
            }
        }

        public void SetupTicketsCount(int ticketsCount)
        {
            _ticketsText.text = ticketsCount.ToString();
        }

        public void SetupSectors(RouletteConfig rouletteConfig)
        {
            DefaultSectors();
            foreach (var sector in rouletteConfig.Sectors)
            {
                var sectorView = Instantiate(_sectorViewPrefab, _sectorViewContainer);
                sectorView.Initialize(_localizationSystem);
                sectorView.Setup(sector);
                _sectorViews.Add(sectorView);
            }
        }

        public void SetBlockSreen(bool isBlocked)
        {
            _screenBlocker.gameObject.SetActive(isBlocked);
        }

        private void DefaultSectors()
        {
            foreach (var sectorView in _sectorViews)
            {
                sectorView.Cleanup();
                Destroy(sectorView.gameObject);
            }
            _sectorViews.Clear();
        }

        private void DefaultWinItems()
        {
            foreach (Transform child in _winItemContainerContainer)
            {
                Destroy(child.gameObject);
            }
        }
        
                
        public List<string> GetAudioKeys()
        {
            if (_audioDatabase == null)
            {
                return null;
            }
            return _audioDatabase.Audios.Keys.ToList();
        }
    }
}