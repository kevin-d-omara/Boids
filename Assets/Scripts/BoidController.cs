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

            steeringPressure = new Vector3(0f, 1f, 0f);
            // Determine new heading.
            //steeringPressure += GetSeparationPressure(flock) * separationWeight;
            //steeringPressure += GetAlignmentPressure(flock) * alignmentWeight;
            //steeringPressure += GetCohesionPressure(flock) * cohesionWeight;
            //steeringPressure += GetWaypointPressure(waypoint) * waypointWeight;
            RotateByPressure(steeringPressure);

            // Move forward.
            rigidBody.velocity = constantSpeed * Heading;
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
