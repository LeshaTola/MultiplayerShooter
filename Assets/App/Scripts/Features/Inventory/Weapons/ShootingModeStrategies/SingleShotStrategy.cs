﻿using App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies.ShootStrategies;
using App.Scripts.Scenes.Gameplay.Weapons;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies
{
    public class SingleShotStrategy : ShootingMode
    {
        public override void PerformAttack()
        {
            ShootStrategy.Shoot();
            Weapon.Animator.AttackAnimation();
            Weapon.ChangeAmmoCount(-1);
            Weapon.StartAttackCooldown(Weapon.Config.AttackCooldown);
            CancelAttack();
        }
    }
}