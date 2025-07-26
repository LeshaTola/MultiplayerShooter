using System.Collections.Generic;
using App.Scripts.Scenes.Gameplay.AI;
using Photon.Pun;

namespace App.Scripts.Scenes.Gameplay.Weapons
{
    public class BotWeaponProvider : WeaponProviderBase
    {
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
            var botAI = PhotonView.Find(botId).GetComponent<BotAI>();
            var weaponTransform = weaponObject.transform;
            weaponTransform.SetParent(botAI.WeaponHolder);
        }
    }
}