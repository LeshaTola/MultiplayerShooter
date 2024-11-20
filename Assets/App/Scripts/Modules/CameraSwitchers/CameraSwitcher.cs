using Cinemachine;

namespace App.Scripts.Modules.CameraSwitchers
{
    public class CameraSwitcher : ICameraSwitcher
    {
        public CamerasDatabase database;

        public CinemachineVirtualCamera CurrentCamera { get; private set; }
        public string CurrentCameraId { get; private set; }

        public CameraSwitcher(CamerasDatabase database)
        {
            this.database = database;
        }

        public void SwitchCamera(string id)
        {
            if (CurrentCamera != null)
            {
                CurrentCamera.gameObject.SetActive(false);
            }


            if (database.Cameras.ContainsKey(id))
            {
                CurrentCameraId = id;
                CurrentCamera = database.Cameras[id];
                CurrentCamera.gameObject.SetActive(true);
            }
        }
    }
}