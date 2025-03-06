using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.VFX.SpeedEffect
{
    public class SpeedLinesEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _speedThreshold = 10f;
        [SerializeField] private float _fadeSpeed = 5f;

        private ParticleSystem.EmissionModule _emissionModule;
        private float _currentEmissionRate;

        private void Start()
        {
            _emissionModule = _particleSystem.emission;
            _currentEmissionRate = 0f;
            _emissionModule.rateOverTime = 0f;
        }

        private void Update()
        {
            var speed = _characterController.velocity.magnitude;
            var targetEmissionRate = speed > _speedThreshold ? speed * 2 : 0f;

            _currentEmissionRate = Mathf.Lerp(_currentEmissionRate, targetEmissionRate, _fadeSpeed * Time.deltaTime);
            _emissionModule.rateOverTime = _currentEmissionRate;
        }
    }
}