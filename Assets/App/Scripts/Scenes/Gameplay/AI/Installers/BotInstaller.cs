using App.Scripts.Modules.AI.Configs;
using App.Scripts.Modules.AI.Factories;
using App.Scripts.Modules.AI.Resolver;
using App.Scripts.Scenes.Gameplay.AI.Providers;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.AI.Installers
{
    public class BotInstaller : MonoInstaller
    {
        [SerializeField] private BotAI _botAI;
        [SerializeField] private BotEnemyProvider _botEnemyProvider;
        [SerializeField] private EffectorsProvider _effectorsProvider;
        [SerializeField] private ActionsDatabase _botActionsDatabase;
        
        public override void InstallBindings()
        {
            Container.Bind<BotAI>().FromInstance(_botAI).AsSingle();
            Container.Bind<EffectorsProvider>().FromInstance(_effectorsProvider).AsSingle();
            Container.Bind<BotEnemyProvider>().FromInstance(_botEnemyProvider).AsSingle();
            Container.BindInterfacesTo<ActionResolver>().AsSingle();
            Container.BindInterfacesTo<ActionsFactory>().AsSingle().WithArguments(_botActionsDatabase);
        }
    }
}