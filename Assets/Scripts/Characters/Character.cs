using UnityEngine;

namespace Characters
{
    /// <summary>
    /// Abstract class to handle character movement. 
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Character : MonoBehaviour
    {
        /// <summary>
        /// The radius from Target to Character, at which the character movement stops.
        /// </summary>
        [SerializeField] private float stopRadius;

        /// <summary>
        /// The radius from Target to Character, at which the character starts slowing down.
        /// </summary>
        [SerializeField] private float slowRadius;

        /// <summary>
        /// Maximum acceleration of the character.
        /// </summary>
        [SerializeField] private float maxAcceleration;

        /// <summary>
        /// Maximum speed of the character.
        /// </summary>
        [SerializeField] private float maxSpeed;

        /// <summary>
        /// Time taken to reach to the target.
        /// </summary>
        [SerializeField, Range(0f, 1f)] private float timeToTarget = 0.1f;

        /// <summary>
        /// Target's position, duh!!
        /// </summary>
        protected abstract Vector2 TargetPosition { get; }

        /// <summary>
        /// Velocity of the character.
        /// </summary>
        private Vector2 _currentVelocity;

        /// <summary>
        /// Rigidbody component of the character.
        /// </summary>
        private Rigidbody2D _characterRigidbody;

        private void Start()
        {
            // Try getting Rigidbody2D.
            _characterRigidbody = GetComponent<Rigidbody2D>();
            // If not found?
            if (!_characterRigidbody)
            {
                // Destroy game object, and return.
                DestroyImmediate(gameObject);
                return;
            }

            // Set the velocity to data type's default value.
            _currentVelocity = default;
        }

        private void Update()
        {
            // Call the custom move towards function.
            MoveTowardsTarget();
        }

        /// <summary>
        /// Custom move towards function.
        /// </summary>
        private void MoveTowardsTarget()
        {
            // Get the direction in which the character is.
            var direction = TargetPosition - (Vector2)transform.position;
            // Get the distance between the target and the character.
            var distance = direction.magnitude;
            // Get Normalised directions.
            direction = direction.normalized;

            // If distance is less than stop radius.
            if (distance <= stopRadius)
            {
                // Stop the character.
                _currentVelocity = Vector2.zero;
            }
            else
            {
                // Else calculate the velocity and acceleration according to the distance.
                // Get target velocity.
                var targetVelocity = direction // the direction in which the object should move
                                     * (maxSpeed * Mathf.Clamp01(distance /
                                                                 slowRadius)); // the speed with which it show be moving, slowing down if distance is less than slow radius.
                // Calculate the acceleration.
                var acceleration = Vector2.ClampMagnitude((targetVelocity - _characterRigidbody.velocity) / timeToTarget, maxAcceleration);

                // Add the velocity change for the frame.
                _currentVelocity += acceleration * Time.deltaTime;
            }

            // Face the target direction.
            LookAtTarget();
            // Apply the calculated velocity to the character.
            _characterRigidbody.velocity = _currentVelocity;
        }

        /// <summary>
        /// Function to make the character look at the target.
        /// </summary>
        private void LookAtTarget()
        {
            // Check if the character is moving.
            if (_currentVelocity.magnitude <= 0)
                return;

            // Get the normalized direction of the character.
            var direction = _currentVelocity.normalized;
            // Apply the rotation of the object.
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        }
    }
}