using System;
using App.Scripts.Features.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Inventory.Screen
{
    public class InventoryScreeen : GameScreen
    {
        public event Action CloseAction;
        
        [SerializeField] private Button _closeButton;

        public override void Initialize()
        {
            _closeButton.onClick.AddListener(() => CloseAction?.Invoke());
        }

        public override void Cleanup()
        {
            _closeButton.onClick.RemoveAllListeners();
        }
    }
}