using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Weapons
{
    public class WeaponStat : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _statName;
        [SerializeField] private TextMeshProUGUI _statValue;

        public void Setup(string statName, string statValue)
        {
            _statName.text = statName;
            _statValue.text = statValue;
        }
    }
}