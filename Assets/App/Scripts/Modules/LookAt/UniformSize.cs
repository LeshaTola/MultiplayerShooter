using UnityEngine;

namespace App.Scripts.Modules.LookAt
{
    public class UniformSize : MonoBehaviour
    {
        enum Mode
        {
            Normal,
            Uniform,
        }

        [SerializeField] Mode mode;
        [SerializeField] private float _distanceCoef = 25;
        [SerializeField] private float _minSize = 1;

        private void LateUpdate()
        {
            switch (mode)
            {
                case Mode.Normal:
                    break;
                case Mode.Uniform:
                    if(Camera.main == null) return;

                    float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
                    var scale = distance / _distanceCoef;
                    if (scale < _minSize)
                    {
                        scale = _minSize;
                    }
                    transform.localScale = Vector3.one * scale;
                    break;
            }
        }
    }
}