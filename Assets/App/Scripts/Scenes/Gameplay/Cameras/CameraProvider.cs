using System.Collections.Generic;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using Cinemachine;

namespace App.Scripts.Scenes.Gameplay.Cameras
{
    public class CameraProvider
    {
        List<CinemachineVirtualCamera> _cameras = new List<CinemachineVirtualCamera>();

        /*public CameraProvider(PlayerProvider playerProvider)
        {
            /*if(playerProvider.Player != null)
            playerProvider.OnPlayerCreated += OnPlayerCreated;#1#
            RegisterCamera(playerProvider.Player.VirtualCamera);
        }
        */

        public CinemachineVirtualCamera GetPlayerCamera()
        {
            return _cameras[0];
        }

        public void RegisterCamera(CinemachineVirtualCamera camera)
        {
            _cameras.Add(camera);
        }

        private void OnPlayerCreated(Player.Player player)
        {
            RegisterCamera(player.VirtualCamera);
        }
    }
}