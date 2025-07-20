using System;
using System.Collections.Generic;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using Photon.Pun;
using UnityEngine;
using Zenject;
using IInitializable = App.Scripts.Modules.StateMachine.Services.InitializeService.IInitializable;

namespace App.Scripts.Scenes.Gameplay.AI
{
    public class BotController : MonoBehaviourPunCallbacks, IInitializable, ICleanupable
    {
        // [SerializeField] private BotConfig _botConfig;
        [SerializeField] private int _maxBots = 5;
        
        private BotFactory _botFactory;
        
        private readonly List<BotAI> _bots = new List<BotAI>();
    
        [Inject]
        public void Construct(BotFactory botFactory)
        {
            _botFactory = botFactory;
        }

        public void Initialize()
        {
            Setup();
        }

        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            Setup();
        }
        
        public void Cleanup()
        {
            foreach (var bot in _bots)
            {
                bot.Health.OnDied -= UpdateBots;
                PhotonNetwork.Destroy(bot.gameObject);
            }
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
                var bot = _botFactory.GetBot();
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