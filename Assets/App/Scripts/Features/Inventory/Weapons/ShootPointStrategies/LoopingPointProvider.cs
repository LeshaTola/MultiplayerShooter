using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootPointStrategies
{
    public class LoopingPointProvider : ShootPointStrategy
    {
        public override Vector3 GetShootPoint()
        {
            if (Points.Count == 0)
                return Vector3.zero; 

            ShotPoint = Points[CurrentIndex].position;
            CurrentIndex = (CurrentIndex + 1) % Points.Count; 

            return ShotPoint;
        }
    }
}