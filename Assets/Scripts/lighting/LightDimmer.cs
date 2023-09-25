using UnityEngine;

namespace lighting
{
    public class LightDimmer : MonoBehaviour
    {
        [SerializeField] private Light _light;

        [SerializeField] private float _minIntensity = 3f; 
        [SerializeField] private float _maxIntensity = 6f;
        
        [SerializeField] private float _cycleDurationSpeed = 3f;
        private bool _intensitiyGoingDown = true;
        
        private void Start()
        {
            _light.intensity = _maxIntensity;
        }

        private void Update()
        {
            if (_intensitiyGoingDown)
            {
                _light.intensity -= Time.deltaTime * _cycleDurationSpeed;
                if (_light.intensity < _minIntensity)
                {
                    _intensitiyGoingDown = false;
                }
                return;
            }

            _light.intensity += Time.deltaTime * _cycleDurationSpeed;
            if (_light.intensity > _maxIntensity)
            {
                _intensitiyGoingDown = true;
            }

        }
    }
}