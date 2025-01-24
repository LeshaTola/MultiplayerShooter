using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Player
{
    public class PlayerVisual : MonoBehaviourPun
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private GlobalInventory _globalInventory;

        public void RPCSetImortal(bool imortable)
        {
            photonView.RPC(nameof(SetImortable), RpcTarget.AllBuffered, imortable);
        }

        [PunRPC]
        public void SetImortable(bool imortable)
        {
            var color = _meshRenderer.material.color;
            color.a = imortable ? 0.2f : 1;
            _meshRenderer.material.color = color;
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
            _meshRenderer.material = skinConfig.Material;
        }
    }
}