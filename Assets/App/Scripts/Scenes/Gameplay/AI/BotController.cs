using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.Extensions;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using Photon.Pun;
using UnityEngine;
using Zenject;
using IInitializable = App.Scripts.Modules.StateMachine.Services.InitializeService.IInitializable;

namespace App.Scripts.Scenes.Gameplay.AI
{
    public class BotController : MonoBehaviourPunCallbacks, IInitializable, ICleanupable
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
            RespawnDeadBots();
            SpawnMissingBots();
        }

        private void RespawnDeadBots()
        {
            foreach (var bot in _bots.Where(bot => bot.Health.Value <= 0))
            {
                bot.Health.RPCTakeHeal(bot.Health.MaxValue);
                bot.transform.position = _botFactory.GetSpawnPoint().position;
            }
        }
    
        private void SpawnMissingBots()
        {
            while (_bots.Count < _maxBots)
            {
                var bot = _botFactory.GetBot(_botConfig.Weapons.GetRandom());
                RegisterBot(bot.photonView.ViewID);
            }
        }
        
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