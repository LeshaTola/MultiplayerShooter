using System;
using UnityEngine;

namespace App.Scripts.Modules.LookAt
{
    public class LookAt : MonoBehaviour
    {
        public enum Mode
        {
            LookAt,
            LookAtInverted,
            CameraForward,
            CameraForwardInverted,
            YAxisOnly,
            YAxisOnlyInverted
        }

        [SerializeField] Mode mode = Mode.LookAtInverted;
        
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            if (_camera == null) return;

            switch (mode)
            {
                case Mode.LookAt:
                    transform.LookAt(_camera.transform);
                    break;

                case Mode.LookAtInverted:
                    Vector3 dirFromCamera = transform.position - _camera.transform.position;
                    transform.LookAt(transform.position + dirFromCamera);
                    break;

                case Mode.CameraForward:
                    transform.forward = _camera.transform.forward;
                    break;

                case Mode.CameraForwardInverted:
                    transform.forward = -_camera.transform.forward;
                    break;

                case Mode.YAxisOnly:
                    Vector3 lookPos = _camera.transform.position;
                    lookPos.y = transform.position.y; 
                    transform.LookAt(lookPos);
                    break;
                case Mode.YAxisOnlyInverted:
                    Vector3 invertedLookPos = 2 * transform.position - _camera.transform.position;
                    invertedLookPos.y = transform.position.y;
                    transform.LookAt(invertedLookPos);
                    break;
                    
            }
        }
    }
}