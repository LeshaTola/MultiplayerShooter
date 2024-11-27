using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Screens
{
    public class MainScreen : MonoBehaviour
    {
        [SerializeField] private Button _playButton;

        private void OnEnable()
        {
            _playButton.onClick.AddListener(ShowRooms);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveAllListeners();
        }

        private void ShowRooms()
        {
            ConnectionProvider.Instance.ViewProvider.Initialize();
            ConnectionProvider.Instance.ViewProvider.Show();
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}