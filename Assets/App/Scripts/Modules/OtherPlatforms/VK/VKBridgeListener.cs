using UnityEngine;

namespace DefaultNamespace
{
    public class VKBridgeListener : MonoBehaviour
    {
        public void ReceiveValue(string json)
        {
            Debug.Log("[VK Listener] JSON received: " + json);
            VKSaveManager.Instance?.OnDataReceived(json);
        }
    }
}