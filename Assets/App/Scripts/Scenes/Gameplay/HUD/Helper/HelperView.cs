using System;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.Gameplay.HUD.Helper
{
    public class HelperView:MonoBehaviour
    {
        [SerializeField] private Button _closeButton;

        private void Awake()
        {
            _closeButton.onClick.AddListener(Close);
        }

        private void Close()
        {
            Destroy(gameObject);
        }
    }
}