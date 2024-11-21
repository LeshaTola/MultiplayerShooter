using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkView : MonoBehaviour
{
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;
    [SerializeField] private NetworkManager _networkManager;

    private void Awake()
    {
        _hostButton.onClick.AddListener(StartHost);
        _clientButton.onClick.AddListener(StartClient);
    }

    private void StartClient()
    {
        _networkManager.StartClient();
    }

    private void StartHost()
    {
        _networkManager.StartHost();
    }
    
}
