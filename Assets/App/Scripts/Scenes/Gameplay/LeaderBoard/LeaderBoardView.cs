using System;
using System.Collections.Generic;
using App.Scripts.Features.PlayerStats.Rank.Configs;
using TMPro;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.LeaderBoard
{
    public class LeaderBoardView : MonoBehaviour
    {
        [Inject]
        private LeaderBoardProvider _leaderBoardProvider;
        
        [SerializeField] private List<LeaderBoardElement> _elements;
        [SerializeField] private TextMeshProUGUI _playersCountText;
        [SerializeField] private RanksDatabase _ranksDatabase;
        
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
            foreach (var element in _elements)
            {
                element.Hide();
            }

            int i = 0;
            foreach (var tuple in _leaderBoardProvider.GetTable())
            {
                var rank = _ranksDatabase.Ranks[tuple.Item1];
                _elements[i].Setup(rank.Sprite,tuple.Item2,tuple.Item3,tuple.Item4, tuple.Item5);
                _elements[i].SetupColor(tuple.Item6);
                _elements[i].Show();
                i++;
            }

            _playersCountText.text = i.ToString();
        }
    }
}