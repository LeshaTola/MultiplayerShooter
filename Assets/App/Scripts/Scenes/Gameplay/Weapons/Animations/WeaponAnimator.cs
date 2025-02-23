using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons.Animations
{
    public class WeaponAnimator : MonoBehaviour
    {
        [SerializeField, SerializeReference] private WeaponAnimation _animation;
        [SerializeField, SerializeReference] private WeaponViewProvider _viewProvider;
        
        private Weapon _weapon;

        public void Initialize(Weapon weapon)
        {
            _weapon = weapon;
            _viewProvider.Initialize(weapon);
        }

        public void AttackAnimation(float duration)
        {
            _animation.PlayAnimation(_viewProvider.GetWeapon(), duration);
        }

        public void ReloadAnimation()
        {
            _weapon.transform.DOLocalRotate(new Vector3(360, 0, 0),
                _weapon.Config.ReloadCooldown, RotateMode.FastBeyond360);
        }
    }

    public abstract class WeaponViewProvider
    {
        protected Weapon Weapon;

        public virtual void Initialize(Weapon weapon)
        {
            Weapon = weapon;
        }
        
        public abstract Transform GetWeapon();
    }

    public class ConstWeaponViewProvider : WeaponViewProvider
    {
        [SerializeField] private Transform _weaponView;
        public override Transform GetWeapon()
        {
            return _weaponView;
        }
    }

    public class PoinToWeaponViewProvider : WeaponViewProvider
    {
        [SerializeField] private List<PoinToWeapon> _transformToWeapon;
        
        private Dictionary<Vector3, Transform> _pointToWeapon;
        
        public override void Initialize(Weapon weapon)
        {
            base.Initialize(weapon);
            /*_pointToWeapon = new Dictionary<Vector3, Transform>();
            foreach (var transform in _transformToWeapon)
            {
                _pointToWeapon.Add(transform.Point.localPosition, transform.Weapon);
            }*/
        }

        public override Transform GetWeapon()
        {
            foreach (var poinToWeapon in _transformToWeapon)
            {
                if (poinToWeapon.Point.position.Equals(Weapon.ShootPointProvider.ShotPoint))
                {
                    return poinToWeapon.Weapon;
                }
            }
            return _transformToWeapon[0].Weapon;
        }

        [Serializable]
        private class PoinToWeapon
        {
            public Transform Point;
            public Transform Weapon;
        }
    }

    public abstract class WeaponAnimation
    {
        public float _speedMultiplier = 1.0f;
        public abstract void PlayAnimation(Transform transform, float duration);
    }

    public class NoAnimation : WeaponAnimation
    {
        public override void PlayAnimation(Transform transform, float duration)
        {
            
        }
    }

    public class SlideAnimation : WeaponAnimation
    {
        [SerializeField] private float _slideOffset = 0.15f;
        
        public override void PlayAnimation(Transform transform, float duration)
        {
            duration = duration * _speedMultiplier;
            Sequence sequence = DOTween.Sequence();
            var startPos = transform.localPosition.z;
            if (duration <= 0.2f)
            {
                sequence.Append(transform.DOLocalMoveZ(startPos -_slideOffset, duration / 2));
                sequence.Append(transform.DOLocalMoveZ(startPos, duration / 2));
            }
            else
            {
                sequence.Append(transform.DOLocalMoveZ(startPos-_slideOffset, 0.1f));
                sequence.Append(transform.DOLocalMoveZ(startPos, duration -0.1f));
            }
        }
    }
}