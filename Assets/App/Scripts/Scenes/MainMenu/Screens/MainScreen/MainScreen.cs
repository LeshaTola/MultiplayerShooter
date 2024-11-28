using System;
using App.Scripts.Features.Screens;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Screens.MainScreen
{
    public class MainScreen : GameScreen
    {
        public event Action ShopButtonAction;
        public event Action InventoryButtonAction;
        public event Action PlayButtonAction;

        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _playButton;

        public override void Initialize()
        {
            _shopButton.onClick.AddListener(() => ShopButtonAction?.Invoke());
            _inventoryButton.onClick.AddListener(() => InventoryButtonAction?.Invoke());
            _playButton.onClick.AddListener(() => PlayButtonAction?.Invoke());
        }

        public override void Cleanup()
        {
            _shopButton.onClick.RemoveAllListeners();
            _inventoryButton.onClick.RemoveAllListeners();
            _playButton.onClick.RemoveAllListeners();
        }
    }
}