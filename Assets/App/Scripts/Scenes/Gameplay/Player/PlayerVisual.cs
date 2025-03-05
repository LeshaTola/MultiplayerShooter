using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Player
{
    public class PlayerVisual : MonoBehaviourPun
    {
        private const string WALK_X = "WalkingX";
        private const string WALK_Y = "WalkingY";
        private const string JUMP_TRIGGER = "Jump";
        private const string LAND_TRIGGER = "Landing";
        private const string SHOOT_TRIGGER = "Shot";
        
        [SerializeField] private List<MeshRenderer> _meshRenderer;
        [SerializeField] private MeshRenderer _eyeEeshRenderer;
        [SerializeField] private GlobalInventory _globalInventory;
        [SerializeField] private Animator _animator;
        private Vector3 _animationDirection;

        public void RPCSetImortal(bool imortable)
        {
            photonView.RPC(nameof(SetImortable), RpcTarget.AllBuffered, imortable);
        }

        [PunRPC]
        public void SetImortable(bool imortable)
        {
            foreach (MeshRenderer renderer in _meshRenderer)
            {
                var color = renderer.material.color;
                color.a = imortable ? 0.2f : 1;
            
                renderer.material.color = color;
            }

            SetEye(imortable);
        }

        private void SetEye(bool imortable)
        {
            var color = _eyeEeshRenderer.material.color;
            color.a = imortable ? 0.2f : 1;
            
            _eyeEeshRenderer.material.color = color;
        }

        public void RPCSetSkin(string skinId)
        {
            photonView.RPC(nameof(SetSkin), RpcTarget.AllBuffered, skinId);
        }

        [PunRPC]
        public void SetSkin(string skinId)
        {
            var skinConfig = _globalInventory.SkinConfigs.FirstOrDefault(x=>x.Id.Equals( skinId));
            if (!skinConfig)
            {
                return;
            }

            foreach (MeshRenderer renderer in _meshRenderer)
            {
                renderer.material = skinConfig.Material;
            }
            _eyeEeshRenderer.material = skinConfig.EyeMaterial;
        }

        public void MoveAnimation(Vector3 direction)
        {
            _animationDirection = Vector3.Lerp(_animationDirection, direction, Time.deltaTime * 5);
            _animator.SetFloat(WALK_X, Mathf.Clamp(_animationDirection.x,-1, 1));
            _animator.SetFloat(WALK_Y, Mathf.Clamp(_animationDirection.z,-1, 1));
        }

        public void JumpAnimation()
        {
            _animator.SetTrigger(JUMP_TRIGGER);
        }
        public void LandAnimation()
        {
            _animator.SetTrigger(LAND_TRIGGER);
        }

        public void ShootAnimation()
        {
            _animator.SetTrigger(SHOOT_TRIGGER);
        }
    }
}