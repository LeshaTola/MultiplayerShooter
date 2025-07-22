using System.Collections.Generic;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using Cinemachine;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Cameras
{
    public class CameraProvider
    {
        public Camera Camera { get; }

        List<CinemachineVirtualCamera> _cameras = new List<CinemachineVirtualCamera>();

        public CameraProvider(Camera camera)
        {
            Camera = camera;
        }

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
            RegisterCamera(player.PlayerMovement.VirtualCamera);
        }
    }
}