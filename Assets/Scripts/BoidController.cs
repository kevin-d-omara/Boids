using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinDOMara.Boids3D
{
    [RequireComponent(typeof(Rigidbody))]
    public class BoidController : MonoBehaviour
    {
        [Header("Boid Characteristics")]
        public float flockRadius = 5f;
        public float constantSpeed = 5f;

        [Header("Steering Behavior")]
        public float separationWeight = 1.0f;
        public float alignmentWeight = 1.0f;
        public float cohesionWeight = 1.0f;
        public float waypointWeight = 1.0f;

        /// <summary>
        /// Forward direction of the Boid.
        /// </summary>
        public Vector3 Heading { get { return transform.forward; } }

        private Rigidbody rigidBody;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var steeringPressure = Vector3.zero;
            var flock = GetBoidsWithin(flockRadius);

            //steeringPressure = new Vector3(0f, 1f, 0f);
            // Determine new heading.
            //steeringPressure += GetSeparationPressure(flock) * separationWeight;
            steeringPressure += GetAlignmentPressure(flock) * alignmentWeight;
            steeringPressure += GetCohesionPressure(flock) * cohesionWeight;
            //steeringPressure += GetWaypointPressure(waypoint) * waypointWeight;
            RotateByPressure(steeringPressure);

            // Move forward.
            rigidBody.velocity = constantSpeed * Heading;

            Debug.DrawRay(transform.position, Heading * 2f);
        }

        /// <summary>
        /// Return all BoidController components within the radius.
        /// </summary>
        private IList<BoidController> GetBoidsWithin(float radius)
        {
            var neighbors = Geometry.FindComponentsInSphere<BoidController>(transform.position,
                radius);
            neighbors.Remove(this);

            return neighbors;
        }

        /// <summary>
        /// Steer toward the average heading of local flockmates.
        /// </summary>
        private Vector3 GetAlignmentPressure(IList<BoidController> flock)
        {
            if (flock.Count == 0) { return Vector3.zero; }

            // Get average flock alignment.
            var averageAlignment = Vector3.zero;
            foreach (BoidController boid in flock)
            {
                averageAlignment += boid.Heading;
            }
            averageAlignment = averageAlignment.normalized;

            // Pressure linearly proportionaly to distance from average alignment.
            var deltaHeading = Vector3.Angle(Heading, averageAlignment);
            return averageAlignment * deltaHeading / 20f;
        }

        /// <summary>
        /// Steer to move toward the average position of local flockmates.
        /// </summary>
        private Vector3 GetCohesionPressure(IList<BoidController> flock)
        {
            if (flock.Count == 0) { return Vector3.zero; }

            // Get average flock position.
            var averagePosition = Vector3.zero;
            foreach (BoidController boid in flock)
            {
                averagePosition += boid.transform.position;
            }
            averagePosition /= flock.Count;

            // Pressure linearly proportional to distance from average position.
            return averagePosition - transform.position;
        }

        /// <summary>
        /// Rotate Boid proportional to the steering pressure.
        /// </summary>
        /// <param name="steeringPressure">Cumulative pressure from each steering behavior.</param>
        private void RotateByPressure(Vector3 steeringPressure)
        {
            if (steeringPressure == Vector3.zero) { return; }

            var stepSize = steeringPressure.magnitude * Mathf.Deg2Rad;
            var finalHeading = Vector3.RotateTowards(Heading, steeringPressure, stepSize, 0.0f);

            var rotation = Quaternion.LookRotation(finalHeading);
            transform.rotation = rotation;
        }
    }
}
