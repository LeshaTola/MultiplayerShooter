using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using App.Scripts.Scenes.Gameplay.Weapons;
using App.Scripts.Scenes.MainMenu.Features.Inventory.GameInventory;

namespace App.Scripts.Scenes.Gameplay.Inventory
{
    public class InventoryController: IInitializable, ICleanupable
    {
        private readonly GameInventoryViewPresenter _inventoryPresenter;
        private readonly PlayerProvider _playerProvider;

        public InventoryController(GameInventoryViewPresenter inventoryPresenter, PlayerProvider playerProvider)
        {
            _inventoryPresenter = inventoryPresenter;
            _playerProvider = playerProvider;
        }

        public void Initialize()
        {
            _playerProvider.Player.WeaponProvider.OnWeaponIndexChanged += OnWeaponChanged;
            // OnWeaponChanged(0);
        }

        public void Cleanup()
        {
            _playerProvider.Player.WeaponProvider.OnWeaponIndexChanged -= OnWeaponChanged;
        }

        private void OnWeaponChanged(int index)
        {
            _inventoryPresenter.SelectSlot(index);
        }
    }
}