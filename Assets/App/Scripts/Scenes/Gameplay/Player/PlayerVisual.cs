using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Scenes.Gameplay.Weapons.Animations;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Scenes.Gameplay.Player
{
    public class PlayerVisual : MonoBehaviourPun
    {
        private const string WALK_X = "WalkingX";
        private const string WALK_Y = "WalkingY";
        private const string JUMP_TRIGGER = "Jump";
        private const string LAND_TRIGGER = "Landing";
        private const string SHOOT_BOOL = "IsShooting";
        private const string SHOOT_TRIGGER = "Shoot";
        private const string FIRE_SPEED = "FireSpeed";

        [SerializeField] private List<MeshRenderer> _meshRenderer;
        [SerializeField] private MeshRenderer _eyeMeshRenderer;
        [SerializeField] private GlobalInventory _globalInventory;
        [SerializeField] private Animator _animator;

        private Vector3 _animationDirection;

        public void Hide()
        {
            foreach (MeshRenderer renderer in _meshRenderer)
            {
                renderer.enabled = false;
            }

            _eyeMeshRenderer.enabled = false;
        }

        public void RPCSetImortal(bool imortable)
        {
            photonView.RPC(nameof(SetImortable), RpcTarget.AllBuffered, imortable);
        }

        [PunRPC]
        public void SetImortable(bool imortable)
        {
            foreach (MeshRenderer renderer in _meshRenderer)
            {
                Material
                    material = renderer.material;
                SetMaterialTransparent(material, imortable);

                var color = material.color;
                color.a = imortable ? 0.2f : 1;
                material.color = color;
            }
            SetEye(imortable);
        }

        public void RPCSetSkin(string skinId)
        {
            photonView.RPC(nameof(SetSkin), RpcTarget.AllBuffered, skinId);
        }

        [PunRPC]
        public void SetSkin(string skinId)
        {
            var skinConfig = _globalInventory.SkinConfigs.FirstOrDefault(x => x.Id.Equals(skinId));
            if (!skinConfig)
            {
                return;
            }

            foreach (MeshRenderer renderer in _meshRenderer)
            {
                renderer.material = skinConfig.Material;
            }

            _eyeMeshRenderer.material = skinConfig.EyeMaterial;
        }

        public void MoveAnimation(Vector3 direction)
        {
            if (_animator == null)
            {
                return;
            }
            
            _animationDirection = Vector3.Lerp(_animationDirection, direction, Time.deltaTime * 5);
            _animator.SetFloat(WALK_X, Mathf.Clamp(_animationDirection.x, -1, 1));
            _animator.SetFloat(WALK_Y, Mathf.Clamp(_animationDirection.z, -1, 1));
        }

        public void JumpAnimation()
        {
            if (_animator == null)
            {
                return;
            }
            _animator.SetTrigger(JUMP_TRIGGER);
        }

        public void LandAnimation()
        {
            if (_animator == null)
            {
                return;
            }
            _animator.SetTrigger(LAND_TRIGGER);
        }

        public void ShootAnimation(bool isShooting, float fireRate = 1)
        {
            if (_animator == null)
            {
                return;
            }
            _animator.SetFloat(FIRE_SPEED, 1 / fireRate);
            _animator.SetBool(SHOOT_BOOL, isShooting);
        }

        public void ShootAnimation(float fireRate = 1)
        {
            if (_animator == null)
            {
                return;
            }
            _animator.SetFloat(FIRE_SPEED, 1 / fireRate);
            _animator.SetTrigger(SHOOT_TRIGGER);
        }

        private void SetEye(bool imortable)
        {
            Material material = _eyeMeshRenderer.material;
            SetMaterialTransparent(material, imortable);

            var color = material.color;
            color.a = imortable ? 0.2f : 1;
            material.color = color;
        }

        private void SetMaterialTransparent(Material material, bool isTransparent)
        {
            if (isTransparent)
            {
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0); // Отключаем запись в Z-buffer
                material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
                material.EnableKeyword("_ALPHABLEND_ON"); // Включаем альфа-смешивание
            }
            else
            {
                material.SetOverrideTag("RenderType", "Opaque");
                material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1); // Включаем запись в Z-buffer
                material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Geometry;
                material.DisableKeyword("_ALPHABLEND_ON"); // Выключаем альфа-смешивание
            }
        }
    }
}