using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.KillChat
{
    public class KillChatView: MonoBehaviourPun
    {
        [SerializeField] private KillElementView _template;
        [SerializeField] private RectTransform _container;

        public void RPCSpawnKillElement(string killer, string victim)
        {
            photonView.RPC(nameof(SpawnKillElement), RpcTarget.All, killer, victim);
        }
        
        [PunRPC]
        public void SpawnKillElement(string killer, string victim)
        {
            var element = Instantiate(_template, _container);
            var killerType = PhotonNetwork.NickName == killer ? KillType.We : KillType.Them;
            var victimType = PhotonNetwork.NickName == victim ? KillType.We : KillType.Them;
            element.Setup(killer, victim, killerType, victimType);
            
        }
    }
}