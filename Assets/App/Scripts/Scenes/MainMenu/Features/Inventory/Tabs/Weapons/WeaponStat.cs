using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Localization.Localizers;
using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Weapons
{
    public class WeaponStat : MonoBehaviour
    {
        [SerializeField] private TMPLocalizer _statName;
        [SerializeField] private TMPLocalizer _statValue;

        public void Initialize(ILocalizationSystem localizationSystem)
        {
            _statName.Initialize(localizationSystem);
            _statValue.Initialize(localizationSystem);
        }
        
        public void Setup(string statName, string statValue)
        {
            _statName.Key = statName;
            _statValue.Key = statValue;
            
            _statName.Translate();
            _statValue.Translate();
        }

        public void Cleanup()
        {
            _statName.Cleanup();
            _statValue.Cleanup();
        }
    }
}