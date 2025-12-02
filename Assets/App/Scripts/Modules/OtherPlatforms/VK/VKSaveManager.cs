using System;
using System.Runtime.InteropServices;

using Newtonsoft.Json;

using UnityEngine;
using YG;

namespace DefaultNamespace
{
    public class VKSaveManager : MonoBehaviour
    {
        public static VKSaveManager Instance;
        private Action<SavesYG> onLoadCallback;

        // [SerializeField] private MenuView _menuView;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // Debug.Log("Зашло в загрузку");
            // LoadData(loaded =>
            // {
            //     YG2.saves = loaded;
            // });
        }

        public void SaveData(SavesYG data)
        {
#if UNITY_WEBGL && !UNITY_EDITOR && VK
        string json = JsonUtility.ToJson(data);
        SaveVKValue("player_data", json);
#else
            Debug.Log("[VK Save] Running outside WebGL, skipped");
#endif
        }
        
        public void MyStart(Action callback)
        {
            Debug.Log("Зашло в загрузку");
            LoadData(loaded =>
            {
                YG2.saves = loaded;
                Debug.Log("Загрузило данные – " + JsonConvert.SerializeObject(loaded));
                callback?.Invoke();
            });
        }

        public void LoadData(Action<SavesYG> callback)
        {
            onLoadCallback = callback;
#if UNITY_WEBGL && !UNITY_EDITOR && VK
        GetVKValue("player_data");
#else
            Debug.Log("[VK Load] Running outside WebGL, returning default");
            callback?.Invoke(YG2.saves);
#endif
        }

        public void OnDataReceived(string json)
        {
            Debug.Log("[VK Load] Received JSON: " + json);
            if (string.IsNullOrEmpty(json))
                onLoadCallback?.Invoke(YG2.saves);
            else
                onLoadCallback?.Invoke(JsonUtility.FromJson<SavesYG>(json));
        }

        // JS bridge
#if VK
    [DllImport("__Internal")]
    private static extern void SaveVKValue(string key, string value);

    [DllImport("__Internal")]
    private static extern void GetVKValue(string key);
#endif
    }
}