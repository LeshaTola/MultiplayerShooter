using Photon.Pun;
using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Player.Stats
{
    public class NickNameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nickName;

        public void Setup(string nickName, Color color)
        {
            _nickName.text = nickName;
            _nickName.color = color;
        }
    }
}