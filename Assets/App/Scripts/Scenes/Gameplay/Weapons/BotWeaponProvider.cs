using System.Collections.Generic;
using App.Scripts.Scenes.Gameplay.AI;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons
{
    public class BotWeaponProvider : WeaponProviderBase
    {
        [field: SerializeField] public Transform WeaponHolder { get; private set; }

        public void Initialize(List<Weapon> weapons)
        {
            Weapons = weapons;
            WeaponIndex = 0;
            CurrentWeapon = Weapons[WeaponIndex];
            foreach (var weapon in Weapons)
            {
                RPCConnectWeapon(weapon);
            }
        }

        private void RPCConnectWeapon(Weapon weapon)
        {
            photonView.RPC(nameof(ConnectWeapon), 
                RpcTarget.AllBuffered,
                photonView.ViewID,
                weapon.photonView.ViewID);
        }

        [PunRPC]
        public void ConnectWeapon(int botId, int weaponId)
        {
            var weaponObject = PhotonView.Find(weaponId).gameObject;
            var weaponProvider = PhotonView.Find(botId).GetComponent<BotWeaponProvider>();
            var weaponTransform = weaponObject.transform;
            weaponTransform.SetParent(weaponProvider.WeaponHolder);
        }
    }
}