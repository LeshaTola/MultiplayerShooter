using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Sounds;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Inventory.Screen
{
    public class InventoryScreeen : GameScreen
    {
        [SerializeField] private AudioDatabase _audioDatabase;
        [field: SerializeField,ValueDropdown(@"GetAudioKeys")] public string SelectWeaponSound { get; private set; }
        [field: SerializeField,ValueDropdown(@"GetAudioKeys")] public string ToggleSond { get; private set; }
        
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