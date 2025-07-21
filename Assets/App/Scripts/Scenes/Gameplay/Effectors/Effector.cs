using System.Collections.Generic;
using App.Scripts.Scenes.Gameplay.Effectors.Strategy;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Effectors
{
    public class Effector : MonoBehaviourPun
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] [SerializeReference] private List<IEffectorStrategy> _strategies;

        public IReadOnlyCollection<IEffectorStrategy> Strategies => _strategies;

        public void Start()
        {
            foreach (var strategy in Strategies)
            {
                strategy.Initialize(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IEntity effetable))
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    ApplyEffect(effetable);
                }
            }
        }

        public void ApplyEffect(IEntity iEntity)
        {
            foreach (IEffectorStrategy strategy in Strategies)
            {
                strategy.Apply(iEntity);
            }
        }

        public void RPCPlaySoud()
        {
            photonView.RPC(nameof(PlaySoud), RpcTarget.AllBuffered);
        }

        [PunRPC]
        public void PlaySoud()
        {
            _audioSource.Play();
        }
    }
}