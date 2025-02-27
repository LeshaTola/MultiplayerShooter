using System;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.Laser
{
    public class LaserRenderer : MonoBehaviourPun
    {
        [SerializeField] private float _maxLength;
        [SerializeField] private Vector3 _direction;
        
        [SerializeField] private LineRenderer _lineRenderer;
        private Vector3 _targetPosition;


        private void Start()
        {
            if (_maxLength > 0)
            {
                Vector3 worldDirection = transform.TransformDirection(_direction);
                _targetPosition = transform.position + worldDirection * _maxLength;
                if (Physics.Raycast(transform.position, worldDirection, out RaycastHit hit, _maxLength))
                {
                    _targetPosition = hit.point;
                }

                Initialize(transform.position , _targetPosition);
            }
        }

        public void RPCSetParent(PhotonView target)
        {
            photonView.RPC("SetParent", RpcTarget.AllBuffered, target.ViewID);
        }
        
        [PunRPC]
        void SetParent(int parentViewID)
        {
            PhotonView parentView = PhotonView.Find(parentViewID);
            if (parentView != null)
            {
                transform.SetParent(parentView.transform);
            }
        }
        
        public void SetLengthRPC(Vector3 endPoint)
        {
            photonView.RPC("SetLength", RpcTarget.All, endPoint);
        }
        public void InitializeRPC(Vector3 startPoint, Vector3 endPoint)
        {
            photonView.RPC("Initialize", RpcTarget.AllBuffered, startPoint, endPoint);
        }
        public void ShowRPC()
        {
            photonView.RPC("Show", RpcTarget.All);
        }
        public void HideRPC()
        {
            photonView.RPC("Hide", RpcTarget.All);
        }
        
        [PunRPC]
        public void SetLength(Vector3 endPoint)
        {
            Vector3 localEndPoint = transform.InverseTransformPoint(endPoint);
            _lineRenderer.SetPosition(1, localEndPoint);  
        }

        [PunRPC]
        public void Initialize(Vector3 startPoint, Vector3 endPoint)
        {
            Vector3 localStartPoint = transform.InverseTransformPoint(startPoint);
            Vector3 localEndPoint = transform.InverseTransformPoint(endPoint);
            _lineRenderer.SetPosition(0, localStartPoint);
            _lineRenderer.SetPosition(1, localEndPoint);
            
        }

        [PunRPC]
        public void Show()
        {
            gameObject.SetActive(true);
        }

        [PunRPC]
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}