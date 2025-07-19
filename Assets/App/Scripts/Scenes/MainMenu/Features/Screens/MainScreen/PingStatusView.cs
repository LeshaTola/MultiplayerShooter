using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PingStatusView : MonoBehaviourPunCallbacks
{
    [SerializeField] private Image _statusImage;
    [SerializeField] private TextMeshProUGUI _pingText;

    [SerializeField] private Color _goodStatusColor = Color.green;   // например, пинг < 80
    [SerializeField] private Color _normStatusColor = Color.yellow;  // например, пинг 80–150
    [SerializeField] private Color _badStatusColor = Color.red;    
    
    private void UpdateStatusColor()
    {
        int ping = PhotonNetwork.GetPing();
        _pingText.text = ping.ToString();
        
        if (ping < 80)
            _statusImage.color = _goodStatusColor;
        else if (ping < 150)
            _statusImage.color = _normStatusColor;
        else
            _statusImage.color = _badStatusColor;
    }

    public override void OnConnected()
    {
        UpdateStatusColor();
    }
    
    public override void OnConnectedToMaster()
    {
        UpdateStatusColor();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _statusImage.color = _badStatusColor;
    }

    private void Update()
    {
        if (PhotonNetwork.IsConnectedAndReady && Time.frameCount % 60 == 0)
        {
            UpdateStatusColor();
        }
    }
}