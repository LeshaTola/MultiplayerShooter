﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootPointStrategies
{
    public abstract class ShootPointStrategy
    {
        [field: SerializeField] public bool ReloadReset {get; set;}

        protected List<Transform> Points;
        protected int CurrentIndex;

        public Vector3 ShotPoint => Points[CurrentIndex].position;

        public virtual void Initialize(List<Transform> points)
        {
            Points = points;
        }
        
        public abstract Vector3 NextShootPoint();

        public virtual void Reset()
        {
            CurrentIndex = 0;
        }

        public virtual void Import(ShootPointStrategy strategy)
        {
            
        }
    }
}