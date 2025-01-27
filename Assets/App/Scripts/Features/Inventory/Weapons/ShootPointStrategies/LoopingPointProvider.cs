using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootPointStrategies
{
    public class LoopingPointProvider : ShootPointStrategy
    {
        public override Vector3 GetShootPoint()
        {
            if (Points.Count == 0)
                return Vector3.zero; 

            Vector3 shootPoint = Points[CurrentIndex].position;
            CurrentIndex = (CurrentIndex + 1) % Points.Count; 

            return shootPoint;
        }
    }
}