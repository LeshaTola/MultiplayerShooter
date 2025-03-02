using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootPointStrategies
{
    public class RandomPointProvider : ShootPointStrategy
    {
        private readonly System.Random random;

        public RandomPointProvider()
        {
            random = new System.Random();
        }

        public override Vector3 NextShootPoint()
        {
            if (Points.Count == 0)
                return Vector3.zero; // или выбрасывать исключение

            int randomIndex = random.Next(Points.Count);
            return ShotPoint;
        }
    }
}