using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.LeaderBoard
{
    public class LeaderBoard : MonoBehaviour
    {
        [SerializeField] private LeaderBoardProvider _leaderBoardProvider;
        [SerializeField] private List<LeaderBoardElement> _elements;

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
                _elements[i].Setup(tuple.Item1,tuple.Item2,tuple.Item3,tuple.Item4);
                _elements[i].SetupColor(tuple.Item5);
                _elements[i].Show();
                i++;
            }
            
        }
    }
}