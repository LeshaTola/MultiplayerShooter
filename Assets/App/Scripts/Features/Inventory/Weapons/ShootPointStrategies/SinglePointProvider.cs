using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootPointStrategies
{
    public class SinglePointProvider : ShootPointStrategy
    {
        [SerializeField] private int _pointIndex = 0;

        public override void Initialize(List<Transform> points)
        {
            base.Initialize(points);
            _pointIndex = Mathf.Clamp(_pointIndex, 0, points.Count - 1);
        }

        public override Vector3 NextShootPoint()
        {
            if (Points.Count == 0)
                return Vector3.zero;
            return ShotPoint;
        }

        public override void Import(ShootPointStrategy strategy)
        {
            var concrete = (SinglePointProvider)strategy;
            _pointIndex = concrete._pointIndex;
        }
    }
}