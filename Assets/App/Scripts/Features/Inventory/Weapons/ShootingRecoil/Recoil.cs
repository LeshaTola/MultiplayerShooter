using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.Scripts.Features.Inventory.Weapons.ShootingRecoil
{
    [Serializable]
    public class Recoil
    {
        [field: SerializeField] public float Radius { get; private set; }
        [field: SerializeField] public float Decrement { get; private set; }
        [field: SerializeField, Range(0,1)] public float Increment { get; private set; }
        [field: SerializeField, Range(0,1)] public float MinValue { get; private set; }
        
        [field: SerializeField, ReadOnly, Space] public float Value { get; private set; }
        [field: SerializeField, ReadOnly] public bool IsShooting { get; set; }

        // private UniTask _recoilReloadTask;

        public void Initialize()
        {
            Value = MinValue;
        }

        public Quaternion GetRecoilRotation(Transform cameraTransform)
        {
            var recoil = GetRecoil();
            var localRecoil = cameraTransform.InverseTransformDirection(recoil);
            var recoilRotation = Quaternion.Euler(localRecoil);
            return recoilRotation;
        }

        public Vector3 GetRecoil()
        {
            var offset = Random.insideUnitCircle * Radius * Value;
            return new Vector3(offset.x, offset.y, 0f); 
        }

        public void Add()
        {
            Value += Increment;
            Value = Mathf.Clamp(Value, MinValue, 1);
        }
        
        public void Update()
        {
            if (IsShooting)
            {
                return;
            }
            
            Value -= Decrement * Time.deltaTime;
            Value = Mathf.Clamp(Value, MinValue, 1);
        }

        public void Import(Recoil recoil)
        {
            Radius = recoil.Radius;
            Decrement = recoil.Decrement;
            Increment = Mathf.Clamp(recoil.Increment, 0, 1);
            MinValue = Mathf.Clamp(recoil.MinValue, 0, 1);
        }
       
    }
}