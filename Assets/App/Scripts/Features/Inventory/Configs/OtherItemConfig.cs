using UnityEngine;

namespace App.Scripts.Features.Inventory.Configs
{
    [CreateAssetMenu(menuName = "Configs/Inventory/Other", fileName = "OtherItemConfig")]
    public class OtherItemConfig : ItemConfig
    {
        [field: SerializeField] public OtherItemType ItemType { get; private set; }
    }
}