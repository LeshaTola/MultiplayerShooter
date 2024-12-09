using System;
using App.Scripts.Scenes.Gameplay.Controller;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Stats
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