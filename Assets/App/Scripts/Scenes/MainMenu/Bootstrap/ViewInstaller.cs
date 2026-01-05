using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Features.Yandex.Saves;
using App.Scripts.Modules.Saves;
using App.Scripts.Scenes.Gameplay.Esc.Settings;
using App.Scripts.Scenes.MainMenu.Features.Inventory.GameInventory;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Screen;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Slot.SelectionProviders;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Skins;
using App.Scripts.Scenes.MainMenu.Features.Inventory.Tabs.Weapons;
using App.Scripts.Scenes.MainMenu.Features.PromoCodes.Providers;
using App.Scripts.Scenes.MainMenu.Features.Roulette;
using App.Scripts.Scenes.MainMenu.Features.Roulette.Configs;
using App.Scripts.Scenes.MainMenu.Features.Roulette.Screen;
using App.Scripts.Scenes.MainMenu.Features.Screens.BattlePass;
using App.Scripts.Scenes.MainMenu.Features.Screens.MainScreen;
using App.Scripts.Scenes.MainMenu.Features.Screens.MainScreen.DailyTasks;
using App.Scripts.Scenes.MainMenu.Features.Screens.RoomsViews;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Currency;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Market;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Offers;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.PromoCodes;
using App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Tickets;
using App.Scripts.Scenes.MainMenu.Features.Screens.TopViews;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace App.Scripts.Scenes.MainMenu.Bootstrap
{
    public class ViewInstaller : MonoInstaller
    {
        [FoldoutGroup("Main")]
        [SerializeField] private MainScreen _mainScreen;

        [FoldoutGroup("Main")]
        [SerializeField] private UserStatsView _userStatsView;
        
        [FoldoutGroup("Main")]
        [SerializeField] private DailyTasksView _dailyTasksView;
        
        [FoldoutGroup("Rooms")]
        [SerializeField] private RoomsView _roomsView;

        [SerializeField]
        [FoldoutGroup("Inventory")]
        private InventoryScreeen _inventoryScreeen;

        [SerializeField]
        [FoldoutGroup("Inventory")]
        private GameInventoryView _gameInventoryView;

        [SerializeField]
        [FoldoutGroup("Inventory")]
        private TabSwitcher _tabSwitcher;

        [SerializeField]
        [FoldoutGroup("Inventory")]
        private RectTransform _overlayContainer;

        [Space]
        [SerializeField]
        [FoldoutGroup("Inventory")]
        private InventoryTab _weaponTab;

        [FormerlySerializedAs("_weaponStatsView")]
        [SerializeField]
        [FoldoutGroup("Inventory")]
        private MarketView _marketView;

        [Space]
        [FoldoutGroup("Inventory")]
        [SerializeField]
        private InventoryTab _skinTab;

        [FoldoutGroup("Inventory")]
        [SerializeField]
        private InventorySlot _skinSlot;

        [SerializeField]
        [FoldoutGroup("Shop")]
        private ShopScreen _shopScreen;

        [SerializeField]
        [FoldoutGroup("Shop")]
        private TicketsSectionView _ticketsSectionView;

        [SerializeField]
        [FoldoutGroup("Shop")]
        private TicketsSectionConfig _ticketsSectionConfig;

        [SerializeField]
        [FoldoutGroup("Shop")]
        private PromoCodesView _promoCodesView;
        
        [SerializeField]
        [FoldoutGroup("Shop")]
        private OffersView _offersView;
        
        [SerializeField]
        [FoldoutGroup("Shop")]
        private MarketSectionView _marketSectionView;

        [SerializeField]
        [FoldoutGroup("Shop")]
        private GlobalInventory _globalInventory;

        [SerializeField]
        [FoldoutGroup("Shop")]
        private RaritiesDatabase _raritiesDatabase;

        [SerializeField]
        [FoldoutGroup("Shop")]
        private CostsDatabase _costsDatabase;

        [FoldoutGroup("Roulette")]
        [SerializeField] private RouletteConfig _rouletteConfig;

        [FoldoutGroup("Roulette")]
        [SerializeField] private RouletteScreen _rouletteScreen;

        [FoldoutGroup("Roulette")]
        [SerializeField] private RouletteView _rouletteView;

        [FoldoutGroup("BattlePass")]
        [SerializeField] private BattlePassScreen _battlePassScreen;


        [FoldoutGroup("TopScreen")]
        [SerializeField] private TopView _topView;

        [FoldoutGroup("TopScreen")]
        [SerializeField] private SettingsView _settingsView;

        [FoldoutGroup("TopScreen")]
        [SerializeField] private TutorialConfig _tutorialConfig;


        public override void InstallBindings()
        {
            Container.BindInstance(_userStatsView).AsSingle();
            
            Container.BindInstance(_dailyTasksView).AsSingle();
            Container.BindInterfacesAndSelfTo<DailyTaskViewPresenter>().AsSingle();
            
            Container.Bind<SettingsView>().FromInstance(_settingsView).AsSingle();

            Container.BindInstance(_raritiesDatabase).AsSingle();
            Container.BindInstance(_costsDatabase).AsSingle();

            BindTopView();
            BindMainScreen();
            BindRoomsScreen();
            BindInventoryScreen();
            BindBattlePassScreen();
            BindRouletteScreen();
            BindShopScreen();
        }

        private void BindMainScreen()
        {
            Container.BindInstance(_mainScreen).AsSingle();
            Container.BindInterfacesAndSelfTo<MainScreenPresenter>().AsSingle().WithArguments(_tutorialConfig);
        }

        private void BindRoomsScreen()
        {
            Container.BindInstance(_roomsView).AsSingle();
            Container.BindInterfacesAndSelfTo<RoomsViewElementPresenter>().AsSingle();
        }

        private void BindTopView()
        {
            Container.Bind<TopView>().FromInstance(_topView).AsSingle();
            Container.BindInterfacesAndSelfTo<TopViewPresenter>().AsSingle();
        }

        private void BindBattlePassScreen()
        {
            Container.BindInstance(_battlePassScreen).AsSingle();
            Container.BindInterfacesAndSelfTo<BattlePassScreenPrezenter>().AsSingle();
        }

        private void BindShopScreen()
        {
            Container.BindInstance(_ticketsSectionView).AsSingle();
            Container.BindInterfacesAndSelfTo<TicketsSectionViewPrezenter>().AsSingle()
                .WithArguments(_ticketsSectionConfig);

            Container.BindInstance(_promoCodesView).AsSingle();
            Container.BindInterfacesAndSelfTo<PromoCodesViewPresenter>().AsSingle();
            
            Container.BindInstance(_offersView).AsSingle();
            Container.BindInterfacesAndSelfTo<OffersViewPresenter>().AsSingle();
            
            Container.BindInstance(_marketSectionView).AsSingle();
            Container.Bind<MarketSectionPrezenter>().AsSingle();

            Container.BindInterfacesAndSelfTo<MarketService>().AsSingle().WithArguments(_globalInventory).NonLazy();
#if YandexGamesPlatform_yg
            Container.Bind<IDataProvider<MarketSavesData>>().To<YandexMarketSavesDataProvider>().AsSingle();
#else
            Container.Bind<IDataProvider<MarketSavesData>>().
                To<DataProvider<MarketSavesData>>().
                AsSingle().
                WithArguments("MarketSavesKey");
#endif

            Container.BindInstance(_shopScreen).AsSingle();
            Container.BindInterfacesAndSelfTo<ShopScreenElementPrezenter>().AsSingle();
        }

        private void BindInventoryScreen()
        {
            Container.Bind<MarketViewPresenter>().AsSingle().WithArguments(_marketView);
            Container.BindInterfacesAndSelfTo<InventoryScreenPresenter>().AsSingle();
            Container.Bind<GameInventoryViewPresenter>().AsSingle().WithArguments(_overlayContainer);

            Container.Bind<SelectionProvider>().AsSingle();
            Container.Bind<InventoryTabPresenter>().To<WeaponTabPresenter>().AsSingle()
                .WithArguments(_overlayContainer, _weaponTab);

            Container.Bind<InventoryTabPresenter>().To<SkinsTabPresenter>().AsSingle()
                .WithArguments(_overlayContainer, _skinTab, _skinSlot);
            Container.BindInstance(_inventoryScreeen).AsSingle();
            Container.BindInstance(_gameInventoryView).AsSingle();
            Container.BindInstance(_tabSwitcher).AsSingle();
        }

        private void BindRouletteScreen()
        {
            Container.BindInstance(_rouletteScreen).AsSingle();
            Container.BindInstance(_rouletteView).AsSingle();
            Container.Bind<Roulette>().AsSingle().WithArguments(_rouletteConfig);
            Container.BindInterfacesAndSelfTo<RouletteScreenPresentrer>().AsSingle().WithArguments(_rouletteConfig);
        }
    }
}