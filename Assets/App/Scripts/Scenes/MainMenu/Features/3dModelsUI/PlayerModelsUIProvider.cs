using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Scenes.Gameplay.Player;

namespace App.Scripts.Scenes.MainMenu.Features._3dModelsUI
{
    public class PlayerModelsUIProvider
    {
        private readonly PlayerVisual _playerVisual;

        public PlayerModelsUIProvider(PlayerVisual playerVisual)
        {
            _playerVisual = playerVisual;
        }

        public void Setup(string skinConfigId)
        {
            _playerVisual.SetSkin(skinConfigId);
            _playerVisual.gameObject.SetActive(true);
        }

        public void Cleanup()
        {
            _playerVisual.gameObject.SetActive(false);
        }
    }
}