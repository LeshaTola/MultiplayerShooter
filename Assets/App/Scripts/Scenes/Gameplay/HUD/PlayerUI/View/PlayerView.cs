using App.Scripts.Scenes.Gameplay.Player.Stats;
using App.Scripts.Scenes.Gameplay.Weapons;
using App.Scripts.Scenes.MainMenu.Features.Inventory.GameInventory;
using UnityEngine;
namespace App.Scripts.Scenes.Gameplay.HUD.PlayerUI.View
{
    public class PlayerView : MonoBehaviour
    {
        [field: SerializeField] public HealthBarUI HealthBar { get; private set; }
        [field: SerializeField] public WeaponView WeaponView { get;  private set;}

        [field: Header("Inventory")] 
        [field: SerializeField] public GameInventoryView InventoryView { get; private set; }
        
    }
}