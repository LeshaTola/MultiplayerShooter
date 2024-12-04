using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.UI.LeaderBoard
{
    public class LeaderBoardElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nickNameText;
        [SerializeField] private TextMeshProUGUI _killsText;
        [SerializeField] private TextMeshProUGUI _deathText;
        [SerializeField] private TextMeshProUGUI _pingText;

        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _selectedColor = Color.yellow;
        
        public void Setup(string nickName, int kills, int death, int ping)
        {
            _nickNameText.text = nickName;
            _killsText.text = kills.ToString();
            _deathText.text = death.ToString();
            _pingText.text = ping.ToString();
        }

        public void SetupColor(bool isMine = false)
        {
            if (isMine)
            {
                _nickNameText.color = _selectedColor;
                _killsText.color = _selectedColor;
                _deathText.color = _selectedColor;
                _pingText.color = _selectedColor;
                return;
            }
            
            _nickNameText.color = _defaultColor;
            _killsText.color = _defaultColor;
            _deathText.color = _defaultColor;
            _pingText.color = _defaultColor;
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