using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootPointStrategies
{
    public class RandomPointProvider : ShootPointStrategy
    {
        private System.Random random;

        public RandomPointProvider()
        {
            random = new System.Random();
        }

        public override Vector3 GetShootPoint()
        {
            if (Points.Count == 0)
                return Vector3.zero; // или выбрасывать исключение

            int randomIndex = random.Next(Points.Count);
            return Points[randomIndex].position;
        }
    }
}