using App.Scripts.Features.Input;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.Gameplay.Controller;
using Cysharp.Threading.Tasks;
using Photon.Pun;

namespace App.Scripts.Scenes.Gameplay.Chat
{
    public class ChatViewPresenter : GameScreenPresenter, IInitializable , ICleanupable
    {
        private readonly ChatView _view;
        private readonly IGameInputProvider _gameInputProvider;
        private readonly PlayerController _playerController;
        private readonly ILocalizationSystem _localizationSystem;

        private bool _isActive;

        public ChatViewPresenter(ChatView view,
            IGameInputProvider gameInputProvider,
            PlayerController playerController,
            ILocalizationSystem localizationSystem)
        {
            _view = view;
            _gameInputProvider = gameInputProvider;
            _playerController = playerController;
            _localizationSystem = localizationSystem;
        }

        public override void Initialize()
        {
            _view.OnSendMessageButtonPressed += OnEnter;
            
            _view.Initialize(_localizationSystem);
            _gameInputProvider.OnPause += OnPause;
            _gameInputProvider.OnEnter += OnEnter;
        }

        public override void Cleanup()
        {
            _view.OnSendMessageButtonPressed -= OnEnter;

            _view.CleanUp();
            _gameInputProvider.OnPause -= OnPause;
            _gameInputProvider.OnEnter += OnEnter;
        }

        public override UniTask Show()
        {
            _view.ShowChatPanel();
            _playerController.IsBusy = true;
            return base.Show();
        }

        public override UniTask Hide()
        {
            _view.HideChatPanel();
            _playerController.IsBusy = false;
            return base.Hide();
        }
        
        public void SendJoinMessage(string player)
        {
            _view.SendJoinMessage(player);
        }
        
        public void SendLeaveMessage(string player)
        {
            _view.SendLeaveMessage(player);
        }
        
        public void SendMessage()
        {
            _view.RPCSendMessage();
            Hide();
        }
        
        private void OnPause()
        {
            if (_isActive)
            {
                Hide();
                _isActive = !_isActive;
            }
        }

        private void OnEnter()
        {
            if (!_isActive)
            {
                if (_playerController.IsBusy)
                {
                    return;
                }
                Show();
            }
            else
            {
                SendMessage();
            }
            _isActive = !_isActive;
        }
    }
}