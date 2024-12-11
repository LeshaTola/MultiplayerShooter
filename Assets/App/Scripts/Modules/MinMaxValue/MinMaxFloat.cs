using System;
using UnityEngine;

namespace App.Scripts.Modules.MinMaxValue
{
    [Serializable]
    public struct MinMaxFloat
    {
        public float Min;
        public float Max;

        public bool IsValid(float value)
        {
            return (value >= Min) && (value <= Max);
        }

        public float GetRandom()
        {
            return UnityEngine.Random.Range(Min, Max);
        }

        public float Clamp(float value)
        {
            return Mathf.Clamp(value, Min, Max);
        }

        public float Lerp(float amount)
        {
            amount = Mathf.Clamp01(amount);
            return Mathf.Lerp(Min, Max, amount);
        }
        
        public float InverseLerp(float value)
        {
            value = Mathf.Clamp(value, Min, Max);
            return (value - Min) / (Max - Min);
        }
    }
}