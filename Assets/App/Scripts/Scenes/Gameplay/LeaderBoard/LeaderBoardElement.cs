using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.Gameplay.LeaderBoard
{
    public class LeaderBoardElement : MonoBehaviour
    {
        [SerializeField] private Image _rankImage;
        [SerializeField] private TextMeshProUGUI _nickNameText;
        [SerializeField] private TextMeshProUGUI _killsText;
        [SerializeField] private TextMeshProUGUI _deathText;
        [SerializeField] private TextMeshProUGUI _pingText;

        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _selectedColor = Color.yellow;
        
        public void Setup(Sprite rankSprite, string nickName, int kills, int death, int ping)
        {
            _rankImage.sprite = rankSprite;
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
            _rankImage.gameObject.SetActive(true);
            _nickNameText.gameObject.SetActive(true);
            _killsText.gameObject.SetActive(true);
            _deathText.gameObject.SetActive(true);
            _pingText.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _rankImage.gameObject.SetActive(false);
            _nickNameText.gameObject.SetActive(false);
            _killsText.gameObject.SetActive(false);
            _deathText.gameObject.SetActive(false);
            _pingText.gameObject.SetActive(false);
        }
    }
}