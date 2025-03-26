using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.AI
{
    public class BotController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private BotConfig _botConfig;
        [SerializeField] private int _maxBots = 5;
        
        private BotFactory _botFactory;
        
        private readonly List<BotAI> _bots = new List<BotAI>();
    
        [Inject]
        public void Construct(BotFactory botFactory)
        {
            _botFactory = botFactory;
        }

        private void Start()
        {
            Setup();
        }

        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            Setup();
        }

        private void Setup()
        {
            
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            
            foreach (var bot in _bots)
            {
                ConnectToBotsDeath(bot);
            }
            
            UpdateBots();
        }

        private void UpdateBots()
        {
            RemoveDeadBots();
            SpawnMissingBots();
        }

        private void RemoveDeadBots()
        {
            _bots.RemoveAll(bot => bot == null);
        }
    
        private void SpawnMissingBots()
        {
            while (_bots.Count < _maxBots)
            {
                var bot = _botFactory.GetBot(_botConfig);
                RegisterBot(bot.photonView.ViewID);
            }
        }

            
        [PunRPC]
        public void RegisterBot(int botId)
        {
            var bot = PhotonView.Find(botId).GetComponent<BotAI>();
            if (bot != null && !_bots.Contains(bot))
            {
                _bots.Add(bot);
            }

            if (PhotonNetwork.IsMasterClient)
            {
                ConnectToBotsDeath(bot);
            }
        }

        private void ConnectToBotsDeath(BotAI bot)
        {
            bot.Health.OnDied += UpdateBots;
        }
    }
}