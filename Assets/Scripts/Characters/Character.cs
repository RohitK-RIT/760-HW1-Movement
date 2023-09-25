using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] private float stopRadius;
        [SerializeField] private float slowRadius;

        [SerializeField] private float maxAcceleration;
        [SerializeField] private float maxSpeed;

        [SerializeField, Range(0f, 1f)] private float timeToTarget = 0.1f;

        protected abstract Vector2 TargetPosition { get; }

        private Vector2 _currentVelocity;
        private Rigidbody2D _characterRigidbody;

        private void Start()
        {
            _characterRigidbody = GetComponent<Rigidbody2D>();
            if (!_characterRigidbody)
            {
                DestroyImmediate(gameObject);
                return;
            }

            _currentVelocity = default;
        }

        private void Update()
        {
            MoveTowardsTarget();
        }

        private void MoveTowardsTarget()
        {
            var direction = TargetPosition - (Vector2)transform.position;
            var distance = direction.magnitude;

            if (distance <= stopRadius)
            {
                _characterRigidbody.velocity = Vector2.zero;
                return;
            }

            float targetSpeed = 0;
            Vector2 targetVelocity = default;

            if (distance > slowRadius)
            {
                targetSpeed = maxSpeed;
            }
            else
            {
                targetSpeed = maxSpeed * (distance / slowRadius);
            }

            targetVelocity = direction.normalized * targetSpeed;

            var acceleration = targetVelocity - _characterRigidbody.velocity;
            acceleration /= timeToTarget;

            if (acceleration.magnitude > maxAcceleration)
            {
                acceleration = acceleration.normalized * maxAcceleration;
            }

            _currentVelocity = new Vector2(
                _currentVelocity.x + acceleration.x * Time.deltaTime,
                _currentVelocity.y + acceleration.y * Time.deltaTime);

            FaceTarget(direction.normalized);

            _characterRigidbody.velocity = _currentVelocity;
        }

        private void FaceTarget(Vector2 velocity)
        {
            if (velocity.magnitude <= 0)
                return;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg));
        }
    }
}