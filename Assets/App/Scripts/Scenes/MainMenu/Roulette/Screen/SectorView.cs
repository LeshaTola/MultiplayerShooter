using App.Scripts.Scenes.MainMenu.Roulette.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Roulette.Screen
{
    public class SectorView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _percentText;

        public void Setup(SectorConfig config)
        {
            _image.color = config.Color;
            _nameText.text = config.Name;
            _percentText.text = $"{Mathf.RoundToInt(config.Percent*100)}%";
        }
    }
}