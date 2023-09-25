using characters;
using UnityEngine;

namespace projectile
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float projectileSpeed = 5f;
        [SerializeField] private bool isHomingProjectile = true;
        [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] private float maxLifeTime = 10f;
        [SerializeField] private float lifeAfterImpact = 0.5f;
        
        private BadGuy _target;
        private CapsuleCollider _targetCapsule;

        private bool _isTargetNull = true; // avoid costly null checks in Update()
        private bool _isTargetCapsuleNull = true; // avoid costly null checks in Update()

        private void Start()
        {
            if (_isTargetNull)
                return;

            transform.LookAt(GetShoulderHeightWorldPosition());
        }

        private void Update()
        {
            if (_isTargetNull)
                return;

            if (isHomingProjectile)
            {
                transform.LookAt(GetShoulderHeightWorldPosition());
            }

            transform.Translate(Vector3.forward * (projectileSpeed * Time.deltaTime));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<BadGuy>() != _target)
                return;
            
            projectileSpeed = 0;

            if (hitEffectPrefab != null)
            {
                GameObject hitEffectInstance = Instantiate(hitEffectPrefab, GetShoulderHeightWorldPosition(), transform.rotation);
                Destroy(hitEffectInstance, 0.5f);
            }

            Destroy(gameObject, lifeAfterImpact);
            _target.Kill();
        }

        public void SetTarget(BadGuy target)
        {
            _target = target;
            
            _isTargetNull = _target == null;
            if (_isTargetNull)
                return;

            _targetCapsule = _target.GetComponent<CapsuleCollider>();
            _isTargetCapsuleNull = _targetCapsule == null;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetShoulderHeightWorldPosition()
        {
            if (_isTargetCapsuleNull)
                return _target.transform.position + Vector3.up;

            return _target.transform.position + Vector3.up * _targetCapsule.height / 2;
        }
    }
}