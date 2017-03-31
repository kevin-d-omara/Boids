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
        /// Constant propulsion force.
        /// </summary>
        public static float speed = 10f;

        /// <summary>
        /// Forward direction of Boid.
        /// </summary>
        public Vector2 Heading
        {
            get
            {
                return transform.right;
            }
        }

        private Rigidbody2D rigidBody;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var steerAngle = Vector2.zero;

            // Determine new heading.
            AvoidCollision();
            AlignWithFlock();
            MoveToFlock();

            // Rotate heading by steerAngle ("pressure").

            // Move forward.
            rigidBody.velocity = speed * Heading;
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

        private Vector2 GetAvoidance()
        {
            return Vector2.zero;
        }

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
