using Photon.Pun;
using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Player.Stats
{
    public class NickNameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nickName;

        [PunRPC]
        public void Setup(string nickName)
        {
            _nickName.text = nickName;
        }
    }
}