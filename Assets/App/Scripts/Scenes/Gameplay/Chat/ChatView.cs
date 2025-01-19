using System;
using System.Collections.Generic;
using ModestTree.Util;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.Gameplay.Chat
{
    public class ChatView : MonoBehaviourPun
    {
        public Action OnSendMessageButtonPressed;
        
        [SerializeField] private GameObject _chatPanel;
        [SerializeField] private GameObject _historyPanel;
        [SerializeField] private Button _sendButton;
        [SerializeField] private TMP_InputField _inputField;

        [SerializeField] private Message _messagePrefab;
        [SerializeField] private RectTransform _container;
        
        [Header("History")]
        [SerializeField] private int _historySize = 10;
        [SerializeField] private RectTransform _historyContainer;
        
        private List<Message> _history;
        private int _historyIndex = 0;
        
        public void Initialize()
        {
            _sendButton.onClick.AddListener(()=>OnSendMessageButtonPressed?.Invoke());
            _history = new List<Message>(_historySize);
            for (int i = 0; i < _historySize; i++)
            {
                var message = Instantiate(_messagePrefab, _historyContainer);
                _history.Add(message);
            }
        }
        public void CleanUp()
        {
            _sendButton.onClick.RemoveAllListeners();
        }
        
        public void ShowChatPanel()
        {
            _chatPanel.SetActive(false);
            _historyPanel.SetActive(true);
            _inputField.text = "";
            _inputField.Select();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void HideChatPanel()
        {
            _chatPanel.SetActive(true);
            _historyPanel.SetActive(false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void RPCSendMessage()
        {
            HideChatPanel();
            var message = _inputField.text;
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            photonView.RPC(nameof(SendMessage), RpcTarget.All, PhotonNetwork.NickName, message);
        }

        [PunRPC]
        public void SendMessage(string player, string message)
        {
           var messageObject = Instantiate(_messagePrefab, _container);
           messageObject.SetupText(player, message);
           messageObject.Fade();

           var historyMessage = _history[_historyIndex];
           _historyIndex = (_historyIndex + 1) % _history.Count;
           historyMessage.SetupText(player, message);
           historyMessage.SetAsLastMessage();
           
        }
        
    }
}