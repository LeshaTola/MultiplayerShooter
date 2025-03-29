using System;
using App.Scripts.Features.Input;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.Gameplay.HUD.PlayerUI.View;
using UnityEngine;
using YG;

namespace App.Scripts.Scenes.Gameplay.HUD.PlayerUI.Provider
{
    public class PlayerUIProvider : MonoBehaviour, IInitializable, ICleanupable
    {
        public event Action<PlayerView> OnPlayerViewCreated;
        
        [SerializeField] private PlayerView _playerUI;
        [SerializeField] private PlayerView _mobilePlayerUI;
        [SerializeField] private MobileInputView _mobileInputView;
        [SerializeField] private GameObject _helper;

        [SerializeField] private Transform _container;

        public PlayerView PlayerView { get; private set; }

        public void Initialize()
        {
            SpawnUI();
        }

        public void Cleanup()
        {
            Destroy(PlayerView.gameObject);
        }

        private void SpawnUI()
        {
            if (YG2.envir.device == YG2.Device.Desktop)
            {
                SpawnDesktopUI();
            }
            else
            {
                SpawnMobileUI();
                _mobileInputView.gameObject.SetActive(true);
                _helper.SetActive(false);
            }
            
            OnPlayerViewCreated?.Invoke(PlayerView);
        }


        private void SpawnMobileUI()
        {
            PlayerView = Instantiate(_mobilePlayerUI, _container);
        }

        private void SpawnDesktopUI()
        {
            PlayerView = Instantiate(_playerUI, _container);
        }
    }
}