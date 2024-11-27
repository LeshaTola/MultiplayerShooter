using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Screens
{
    public class RoomView : MonoBehaviour
    {
        [SerializeField] private Button _selectButton;

        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _playersCountText;
        
        [Header("Button")]
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _selectedColor = Color.white;
        
        [Header("Text")]
        [SerializeField] private Color _correctColor = Color.green;
        [SerializeField] private Color _incorrectColor = Color.red;

        public void Setup(string name, int currentPlayers, int maxPlayers, Action onClick)
        {
            Cleanup();
            _nameText.text = name;

            _playersCountText.text = $"{currentPlayers}/{maxPlayers}";
            _playersCountText.color = currentPlayers >= maxPlayers ? _incorrectColor : _correctColor;

            _selectButton.onClick.AddListener(() =>
            {
                onClick?.Invoke();
                Select();
            });
        }

        private void Select()
        {
            _selectButton.image.color = _selectedColor;
        }

        private void Unselect()
        {
            _selectButton.image.color = _defaultColor;
        }


        private void Cleanup()
        {
            Unselect();
            _selectButton.onClick.RemoveAllListeners();
        }
    }
}