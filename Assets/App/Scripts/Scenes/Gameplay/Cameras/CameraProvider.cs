using System.Collections.Generic;
using Cinemachine;

namespace App.Scripts.Scenes.Gameplay.Cameras
{
    public class CameraProvider
    {
        List<CinemachineVirtualCamera> _cameras = new List<CinemachineVirtualCamera>();

        public CinemachineVirtualCamera GetPlayerCamera()
        {
            return _cameras[0];
        }

        public void RegisterCamera(CinemachineVirtualCamera camera)
        {
            _cameras.Add(camera);
        }
    }
}