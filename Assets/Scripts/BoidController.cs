using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinDOMara.Boids2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BoidController : MonoBehaviour
    {
        /// <summary>
        /// Radius for interacting with other Boids
        /// </summary>
        public static float radius = 2f;

        /// <summary>
        /// Constant speed the Boid moves with.
        /// </summary>
        public static float constantSpeed = 10f;

        /// <summary>
        /// Forward direction of the Boid.
        /// </summary>
        public Vector2 Heading { get { return transform.right; } }

        private Rigidbody2D rigidBody;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var steerPressure = Vector2.zero;
            var flock = GetBoidsWithin(radius);

            // Determine new heading.
            AvoidCollision();
            AlignWithFlock();
            MoveToFlock();
            steerPressure = new Vector2(0f, -5f);

            RotateByPressure(steerPressure);

            // Move forward.
            rigidBody.velocity = constantSpeed * Heading;
        }

        private void AvoidCollision()
        {
            // Sum inverse distance of flock-mates
            // Convert to steer angle pressure
        }

        private void AlignWithFlock()
        {
            // GetFlockAlignment()
            // Calculate delta w/ self
            // Convert to steer angle pressure
        }

        private void MoveToFlock()
        {
            // GetFlockPosition()
            // Calculate delta w/ self
            // Convert to steer angle pressure
        }

        /// <summary>
        /// Rotate Boid proportional to the steering pressure.
        /// </summary>
        /// <param name="steeringPressure">Cumulative pressure from each steering behavior.</param>
        private void RotateByPressure(Vector2 steeringPressure)
        {
            // Prevent rotating too far.
            var rotationDirection = Heading.RotateTo(steeringPressure);
            var deltaHeading = Vector2.Angle(Heading, steeringPressure);

            // Rotate clockwise or counter-clockwise.
            var rotateAngle = Mathf.Clamp(steeringPressure.magnitude, 0, deltaHeading);
            rotateAngle *= rotationDirection;

            transform.Rotate(new Vector3(0f, 0f, rotateAngle));
        }

        private Vector2 GetAvoidance()
        {
            return Vector2.zero;
        }

        /// <summary>
        /// Return the average alignment of the flock.
        /// </summary>
        private Vector2 GetFlockAlignment()
        {
            return Vector2.zero;
        }

        private Vector2 GetFlockPosition()
        {
            return Vector2.zero;
        }



        /// <summary>
        /// Return all BoidController components within the radius.
        /// </summary>
        private IList<BoidController> GetBoidsWithin(float radius)
        {
            var neighbors = Geometry.FindComponentsInCircle<BoidController>(transform.position, radius);
            neighbors.Remove(this);

            return neighbors;
        }
    }
}
