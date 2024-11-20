using Cinemachine;

namespace App.Scripts.Modules.CameraSwitchers
{
    public interface ICameraSwitcher
    {
        CinemachineVirtualCamera CurrentCamera { get; }
        string CurrentCameraId { get; }

        void SwitchCamera(string id);
    }
}