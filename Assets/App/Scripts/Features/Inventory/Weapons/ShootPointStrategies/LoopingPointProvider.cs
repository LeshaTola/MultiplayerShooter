using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootPointStrategies
{
    public class LoopingPointProvider : ShootPointStrategy
    {
        public override Vector3 NextShootPoint()
        {
            if (Points.Count == 0)
                return Vector3.zero; 

            CurrentIndex = (CurrentIndex + 1) % Points.Count; 

            return ShotPoint;
        }
    }
}