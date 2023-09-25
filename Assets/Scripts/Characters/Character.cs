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
            direction = direction.normalized;

            if (distance <= stopRadius)
            {
                _currentVelocity = Vector2.zero;
            }
            else
            {
                var targetVelocity = direction // the direction in which the object should move
                                     * (maxSpeed * Mathf.Clamp01(distance / slowRadius)); // the speed with which it show be moving
                var acceleration = Vector2.ClampMagnitude((targetVelocity - _characterRigidbody.velocity) / timeToTarget, maxAcceleration);

                _currentVelocity += acceleration * Time.deltaTime;
            }

            FaceTarget(direction);
            _characterRigidbody.velocity = _currentVelocity;
        }

        private void FaceTarget(Vector2 direction)
        {
            if (direction.magnitude <= 0)
                return;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        }
    }
}