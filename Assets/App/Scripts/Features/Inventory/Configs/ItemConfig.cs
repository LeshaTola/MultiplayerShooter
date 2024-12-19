using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons
{
    public class ItemConfig: SerializedScriptableObject
    {
        [field: SerializeField] 
        [field: ReadOnly] 
        public string Id { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        
        private void OnValidate()
        {
            Id = name;
        }
    }
}