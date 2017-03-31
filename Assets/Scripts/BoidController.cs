using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KevinDOMara.Boids2D
{
    /// <summary>
    /// A simple 2D Boid controller.
    /// 
    /// See http://www.red3d.com/cwr/boids/ for inspiration.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class BoidController : MonoBehaviour
    {
        [Header("Boid Characteristics")]
        public float flockRadius = 5f;
        public float constantSpeed = 5f;

        [Header("Steering Behavior")]
        public float separationWeight = 1.0f;
        public float alignmentWeight  = 1.0f;
        public float cohesionWeight   = 1.0f;

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
            var steeringPressure = Vector2.zero;
            var flock = GetBoidsWithin(flockRadius);

            // Determine new heading.
            steeringPressure += GetSeparationPressure(flock) * separationWeight;
            steeringPressure += GetAlignmentPressure(flock)  * alignmentWeight;
            steeringPressure += GetCohesionPressure(flock)   * cohesionWeight;
            RotateByPressure(steeringPressure);

            // Move forward.
            rigidBody.velocity = constantSpeed * Heading;
        }

        /// <summary>
        /// Steer to avoid crowding local flockmates.
        /// </summary>
        private Vector2 GetSeparationPressure(IList<BoidController> flock)
        {
            // Pressure increases with the inverse distance from flockmates.
            var separationPressure = GetInverseDistanceTo(flock);

            return separationPressure;
        }

        /// <summary>
        /// Return a summation of the inverse distance to each flockmate.
        /// </summary>
        private Vector3 GetInverseDistanceTo(IList<BoidController> flock)
        {
            var inverseDistance = Vector3.zero;
            foreach (BoidController boid in flock)
            {
                var deltaPosition = transform.position - boid.transform.position;
                if (deltaPosition.magnitude == 0) { deltaPosition = Random.onUnitSphere; }

                inverseDistance += deltaPosition / deltaPosition.magnitude;
            }

            return inverseDistance;
        }

        /// <summary>
        /// Steer toward the average heading of local flockmates.
        /// </summary>
        private Vector2 GetAlignmentPressure(IList<BoidController> flock)
        {
            if (flock.Count == 0) { return Vector2.zero; }

            var averageAlignment = GetFlockAlignment(flock);
            var deltaHeading = Vector2.Angle(Heading, averageAlignment);

            // Pressure increases linearly with distance from flock's average alignment.
            var alignmentPressure = averageAlignment * deltaHeading / 10f;

            return alignmentPressure;
        }

        /// <summary>
        /// Return the average alignment of flockmates.
        /// </summary>
        private Vector2 GetFlockAlignment(IList<BoidController> flock)
        {
            var averageAlignment = Vector2.zero;
            foreach (BoidController boid in flock)
            {
                averageAlignment += boid.Heading;
            }
            return averageAlignment.normalized;
        }

        /// <summary>
        /// Steer to move toward the average position of local flockmates.
        /// </summary>
        private Vector2 GetCohesionPressure(IList<BoidController> flock)
        {
            if (flock.Count == 0) { return Vector2.zero; }

            var averagePosition = GetFlockPosition(flock);
            var deltaPosition = averagePosition - transform.position;

            // Presure increases linearly with distance from the flock's average position.
            return deltaPosition;
        }

        /// <summary>
        /// Return the average position of the flock.
        /// </summary>
        private Vector3 GetFlockPosition(IList<BoidController> flock)
        {
            if (flock.Count == 0) { return Vector3.zero; }

            var averagePosition = Vector3.zero;
            foreach (BoidController boid in flock)
            {
                averagePosition += boid.transform.position;
            }
            averagePosition /= flock.Count;

            return averagePosition;
        }

        /// <summary>
        /// Rotate Boid proportional to the steering pressure.
        /// </summary>
        /// <param name="steeringPressure">Cumulative pressure from each steering behavior.</param>
        private void RotateByPressure(Vector2 steeringPressure)
        {
            // Prevent rotating too far.
            var deltaHeading = Vector2.Angle(Heading, steeringPressure);
            var rotateAngle = Mathf.Clamp(steeringPressure.magnitude, 0, deltaHeading);

            // Rotate clockwise or counter-clockwise.
            var rotationDirection = Heading.RotateTo(steeringPressure);
            rotateAngle *= rotationDirection;

            transform.Rotate(new Vector3(0f, 0f, rotateAngle));
        }

        /// <summary>
        /// Return all BoidController components within the radius.
        /// </summary>
        private IList<BoidController> GetBoidsWithin(float radius)
        {
            var neighbors = Geometry.FindComponentsInCircle<BoidController>(transform.position,
                radius);
            neighbors.Remove(this);

            return neighbors;
        }
    }
}
