using System;
using System.Text;
using App.Scripts.Features;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.Shop.Sections.Tickets
{
    public class TicketsSectionView : MonoBehaviour
    {
        public event Action OnPlusButtonPressed;
        public event Action OnMinusButtonPressed;
        public event Action OnBuyButtonPressed;

        [Header("Change Amount")]
        [SerializeField] private Button _plusButton;

        [SerializeField] private Button _minusButton;
        [SerializeField] private TextMeshProUGUI _countText;

        [Header("Buy")]
        [SerializeField] private TextMeshProUGUI _costText;

        [SerializeField] private Button _buyButton;
        [SerializeField] private float _costAnimationDuration = 0.5f;

        private StringBuilder _sb = new StringBuilder(16);
        
        public void Initialise()
        {
            _plusButton.onClick.AddListener(() => OnPlusButtonPressed?.Invoke());
            _minusButton.onClick.AddListener(() => OnMinusButtonPressed?.Invoke());
            _buyButton.onClick.AddListener(() => OnBuyButtonPressed?.Invoke());
        }

        public void Cleanup()
        {
            _plusButton.onClick.RemoveAllListeners();
            _minusButton.onClick.RemoveAllListeners();
            _buyButton.onClick.RemoveAllListeners();
        }

        public void UpdateCost(int cost)
        {
            _costText.DOKill();

            int startValue = 0;
            int.TryParse(_costText.text.Replace(ConstStrings.R, ""), out startValue);

            DOTween.To(() => startValue, x =>
            {
                _sb.Clear();
                _sb.Append(x).Append(ConstStrings.R);
                _costText.text = _sb.ToString();
            }, cost, _costAnimationDuration).SetEase(Ease.OutQuad);
        }


        public void UpdateCount(int count)
        {
            _countText.text = count.ToString();
        }

        public void SetActiveChangeAmountButtons(bool active, bool isMinus = false)
        {
            if (isMinus)
            {
                _minusButton.interactable = active;
            }
            else
            {
                _plusButton.interactable = active;
            }
        }
    }
}