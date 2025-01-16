using System.Linq;
using App.Scripts.Features.Inventory;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.Gameplay.KillChat
{
    public class KillChatView: MonoBehaviourPun
    {
        [SerializeField] private KillElementView _template;
        [SerializeField] private RectTransform _container;
        [SerializeField] private GlobalInventory _inventory;
        [SerializeField] private Sprite _nullSprite;

        public void RPCSpawnKillElement(string weaponId, string killer, string victim)
        {
            photonView.RPC(nameof(SpawnKillElement), RpcTarget.All,weaponId, killer, victim);
        }
        
        [PunRPC]
        public void SpawnKillElement(string weaponId, string killer, string victim)
        {
            var element = Instantiate(_template, _container);
            var killerType = PhotonNetwork.NickName == killer ? KillType.We : KillType.Them;
            var victimType = PhotonNetwork.NickName == victim ? KillType.We : KillType.Them;
            var weaponConfig = _inventory.Weapons.FirstOrDefault(x => x.Id.Equals(weaponId));
            var weaponSprite  = weaponConfig ==null ? _nullSprite : weaponConfig.Sprite;
            element.Setup(weaponSprite, killer, victim, killerType, victimType);
            
        }
    }
}