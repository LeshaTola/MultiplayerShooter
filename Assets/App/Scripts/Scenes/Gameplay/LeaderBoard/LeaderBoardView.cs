using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.PlayerStats.Rank.Configs;
using App.Scripts.Features.UserStats.Rank.Configs;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.LeaderBoard
{
    public class LeaderBoardView : MonoBehaviour
    {
        [Inject]
        private LeaderBoardProvider _leaderBoardProvider;
        
        // [SerializeField] private 
        [SerializeField] private List<LeaderBoardElement> _elements;
        [SerializeField] private TextMeshProUGUI _playersCountText;
        [SerializeField] private RanksDatabase _ranksDatabase;
        [SerializeField] private TextMeshProUGUI _roomNameText;
        [SerializeField] private GlobalInventory _globalInventory;

        public void Show()
        {
            gameObject.SetActive(true);
            UpdateView();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void UpdateView()
        {
            _roomNameText.text = PhotonNetwork.CurrentRoom.Name;
            
            foreach (var element in _elements)
            {
                element.Hide();
            }

            int i = 0;
            foreach (var data in _leaderBoardProvider.GetTable())
            {
                var rank = _ranksDatabase.Ranks[data.RankId];
                data.RankSprite = rank.Sprite;
                var skinConfig = _globalInventory.SkinConfigs.FirstOrDefault(x => x.Id.Equals(data.SkinId));
                if (skinConfig != null)
                {
                    data.SkinSprite =skinConfig.Sprite; 
                    data.SkinColor = skinConfig.Material.color;
                }
                
                _elements[i].Setup(data);
                _elements[i].SetupColor(data.IsMine);
                _elements[i].Show();
                i++;
            }

            _playersCountText.text = i.ToString();
        }
    }
}