using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.HitVisualProvider
{
    public class DamageView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _damageText;

        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _headShotColor = Color.red;

        public void Setup(string damage, bool isHeadshot = false)
        {
            _damageText.text = damage;
            _damageText.color = isHeadshot ? _headShotColor : _defaultColor;
        }
    }
}