using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons
{
    public abstract class Weapon : MonoBehaviourPun
    {
        public virtual event Action<int, int> OnAmmoChanged;

        [SerializeField] protected Transform _shootPoint;
        [SerializeField] protected int _maxAmmoCount;
        [SerializeField] protected float _reloadCooldown;
        [SerializeField] protected float _attackCooldown;

        public bool IsReady { get; protected set; } = true;
        public int CurrentAmmoCount { get; set; }

        public abstract void Attack();
        public abstract void Reload();
        
        protected void StartAttackCooldown()
        {
            StartCoroutine(AttackCooldown());
        }

        protected void StartReloadCooldown()
        {
            StartCoroutine(ReloadCooldown());
        }

        private IEnumerator AttackCooldown()
        {
            IsReady = false;
            yield return new WaitForSeconds(_attackCooldown);
            IsReady = true;
        }

        private IEnumerator ReloadCooldown()
        {
            IsReady = false;
            yield return new WaitForSeconds(_reloadCooldown);
            IsReady = true;
        }
    }
}