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