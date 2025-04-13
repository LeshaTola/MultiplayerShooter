using App.Scripts.Scenes.Gameplay.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies
{
    public class AutoShootingStrategy: ShootingMode
    {
        public override void PerformAttack()
        {
            base.PerformAttack();
            
            if (!IsMissCounted && _isMiss)
            {
                return;
            }
            
            ShootStrategy.Shoot();
            Weapon.Animator.AttackAnimation(AttackCooldown);
            Weapon.Owner.PlayerAudioProvider.RPCPlayWeaponSound();
            Weapon.ChangeAmmoCount(-1);
            Weapon.StartAttackCooldown(AttackCooldown);
        }
    }
}