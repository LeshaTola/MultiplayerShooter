using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons.Animations
{
    public class WeaponAnimator : MonoBehaviour
    {
        private Weapon _weapon;

        public void Initialize(Weapon weapon)
        {
            _weapon = weapon;
        }
        
        public void AttackAnimation()
        {
            AttackAnimation(_weapon.Config.AttackCooldown);
        }

        public void AttackAnimation(float duration)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(_weapon.transform.DOLocalRotate(new Vector3(-15, 0, 0),
                duration / 2));
            sequence.Append(_weapon.transform.DOLocalRotate(new Vector3(0, 0, 0),
                duration / 2));
        }

        public void ReloadAnimation()
        {
            _weapon.transform.DOLocalRotate(new Vector3(360, 0, 0),
                _weapon.Config.ReloadCooldown, RotateMode.FastBeyond360);
        }
    }
}