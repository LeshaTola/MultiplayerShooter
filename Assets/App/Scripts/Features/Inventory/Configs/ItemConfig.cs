using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Features.Inventory.Configs
{
    public class ItemConfig: SerializedScriptableObject
    {
        [field: SerializeField] 
        [field: ReadOnly] 
        public string Id { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        
        [FormerlySerializedAs("_raritiesConfig")]
        [Space]
        [SerializeField] private RaritiesDatabase _raritiesDatabase;
        [field: SerializeField,ValueDropdown(nameof(GetRaries))] public string Rarity { get; private set; }
        
        
        private void OnValidate()
        {
            Id = name;
        }

        private List<string> GetRaries()
        {
            return _raritiesDatabase.Rarities.Select(x=>x.Value.Name).ToList();
        }
    }
}