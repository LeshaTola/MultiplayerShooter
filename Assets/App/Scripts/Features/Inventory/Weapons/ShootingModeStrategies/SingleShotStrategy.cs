using App.Scripts.Scenes.Gameplay.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies
{
    public class SingleShotStrategy : ShootingMode
    {
        public override void PerformAttack()
        {
            base.PerformAttack();
            Weapon.Owner.PlayerVisual.ShootAnimation(AttackCooldown);
            ShootStrategy.Shoot();
            Weapon.Animator.AttackAnimation(AttackCooldown);
            Weapon.Owner.PlayerAudioProvider.RPCPlayWeaponSound();
            Weapon.ChangeAmmoCount(-1);
            Weapon.StartAttackCooldown(AttackCooldown);
            CancelAttack();
        }
    }
}