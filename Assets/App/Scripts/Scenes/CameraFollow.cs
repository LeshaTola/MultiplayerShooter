using System;
using Cinemachine;
using UnityEngine;

namespace App.Scripts.Scenes
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        
        public static CameraFollow Instance { get; private set; }

        public CinemachineVirtualCamera VirtualCamera => _virtualCamera;

        private void Awake()
        {
            Instance = this;
        }

        public void SetFollow(Transform follow, Transform lookAt)
        {
            _virtualCamera.Follow = follow;
            _virtualCamera.LookAt = lookAt;
        }
        
        
    }
}