﻿using System.Collections.Generic;
using App.Scripts.Scenes.Gameplay.Effectors.Strategy;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Effectors
{
    public class Effector : MonoBehaviourPun
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] [SerializeReference] private List<IEffectorStrategy> _strategies;

        public void Start()
        {
            foreach (var strategy in _strategies)
            {
                strategy.Initialize(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                if (player.photonView.IsMine)
                {
                    ApplyEffect(player);
                }
            }
        }

        public void ApplyEffect(Player.Player player)
        {
            foreach (IEffectorStrategy strategy in _strategies)
            {
                strategy.Apply(player);
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