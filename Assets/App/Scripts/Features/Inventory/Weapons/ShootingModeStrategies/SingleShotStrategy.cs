using App.Scripts.Scenes.Gameplay.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies
{
    public class SingleShotStrategy : ShootingMode
    {
        public override void PerformAttack()
        {
            base.PerformAttack();
            if (!IsMissCounted && _isMiss)
            {
                return;
            }
            Weapon.Owner.Visual.ShootAnimation(AttackCooldown);
            ShootStrategy.Shoot();
            Weapon.Animator.AttackAnimation(AttackCooldown);
            Weapon.Owner.AudioProvider.RPCPlayWeaponSound();
            Weapon.ChangeAmmoCount(-1);
            Weapon.StartAttackCooldown(AttackCooldown);
            CancelAttack();
        }
    }
}