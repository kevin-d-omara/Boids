using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        /// <summary>
        /// Radius for interacting with other Boids
        /// </summary>
        public static float radius = 5f;

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
            var steeringPressure = Vector2.zero;
            var flock = GetBoidsWithin(radius);

            // Determine new heading.
            steeringPressure += GetSeparationPressure(flock);
            steeringPressure += GetAlignmentPressure(flock);
            steeringPressure += GetCohesionPressure(flock);
            RotateByPressure(steeringPressure);

            // Move forward.
            rigidBody.velocity = constantSpeed * Heading;
        }



        /// <summary>
        /// Steer to avoid crowding local flockmates.
        /// </summary>
        /// <returns>Steering pressure from this behavior.</returns>
        private Vector2 GetSeparationPressure(IList<BoidController> flock)
        {
            // Sum inverse distance of flock-mates
            // Convert to steer angle pressure

            return Vector2.zero;
        }



        /// <summary>
        /// Steer toward the average heading of local flockmates.
        /// </summary>
        /// <returns>Steering pressure from this behavior.</returns>
        private Vector2 GetAlignmentPressure(IList<BoidController> flock)
        {
            var averageAlignment = GetFlockAlignment(flock);
            var deltaHeading = Vector2.Angle(Heading, averageAlignment);

            // Pressure increases linearly with distance from flock's average alignment.
            var steeringPressure = averageAlignment * deltaHeading / 10f;

            return steeringPressure;
        }

        /// <summary>
        /// Return the average alignment of flockmates.
        /// </summary>
        /// <param name="flock"></param>
        /// <returns></returns>
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
        /// <returns>Steering pressure from this behavior.</returns>
        private Vector2 GetCohesionPressure(IList<BoidController> flock)
        {
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

        private Vector2 GetAvoidance()
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
