using System;
using System.Collections.Generic;
using App.Scripts.Features.Inventory.Weapons.ShootingRecoil;
using App.Scripts.Features.Inventory.Weapons.WeaponEffects;
using App.Scripts.Scenes.Gameplay.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies
{
    public interface IShootStrategy
    {
        public event Action<Vector3, float, bool> OnPlayerHit;
        
        public Recoil Recoil { get; }        
        public List<IWeaponEffect> WeaponEffects { get;  set; }

        public void Initialize(Weapon weapon);
        public void Shoot();

        public void Import(IShootStrategy original);
    }

    public abstract class ShootStrategy: IShootStrategy
    {
        public virtual event Action<Vector3, float, bool> OnPlayerHit;

        [SerializeField] public Recoil Recoil { get; private set; } = new();
        [SerializeField] public List<IWeaponEffect> WeaponEffects { get;  set;}
        
        [SerializeField] protected float _maxDistance = 50f;
        protected Weapon Weapon;
        protected Camera Camera;

        protected ShootStrategy(Camera camera)
        {
            Camera = camera;
        }

        public virtual void Initialize(Weapon weapon)
        {
            foreach (var weaponEffect in WeaponEffects)
            {
                weaponEffect.Initialize(weapon);
            }
            Weapon = weapon;
        }

        public virtual void Shoot()
        {
            Recoil.Add();
        }

        public virtual void Import(IShootStrategy original)
        {
            Recoil.Import(original.Recoil);

            for (int i = 0; i < WeaponEffects.Count; i++)
            {
                WeaponEffects[i].OnPlayerHit += (point, damage, killed)=>OnPlayerHit?.Invoke(point, damage, killed);
                WeaponEffects[i].Import(original.WeaponEffects[i]);
            }
        }

        public (RaycastHit, Vector3) GetRaycastHit()
        {
            var recoilRotation = Recoil.GetRecoilRotation(Camera.transform);
            var direction = recoilRotation * Camera.transform.forward;

            Vector3 endPoint = Camera.transform.position + direction * _maxDistance;
            if (Physics.Raycast(Camera.transform.position, direction, out var hit, _maxDistance, Physics.DefaultRaycastLayers,
                    QueryTriggerInteraction.Ignore))
            {
                endPoint = hit.point;
            }

            return (hit, endPoint);
        }
    }
}