using System.Collections.Generic;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Modules.CameraSwitchers
{
    public class CamerasDatabase : SerializedMonoBehaviour
    {
        [field: SerializeField] public Dictionary<string, CinemachineVirtualCamera> Cameras { get; private set; }
    }
}