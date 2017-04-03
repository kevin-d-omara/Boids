using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinDOMara.Boids3D
{
    public class WaypointController : MonoBehaviour
    {
        /// <summary>
        /// Percent of boids which must be in the waypoint zone to be considered filled.
        /// </summary>
        [Range(0f, 1f)] public float triggerThreshold = 0.75f;

        /// <summary>
        /// Radius of waypoint zone.
        /// </summary>
        [Range(0f, 100f)] public float radius = 20f;

        [SerializeField] private LayerMask boidLayer;

        /// <summary>
        /// Return true if a percentage of the flock is within range.
        /// </summary>
        /// <param name="boidCount">Number of boids in the flock.</param>
        public bool IsFilled(int boidCount)
        {
            var hits = Physics.OverlapSphere(transform.position, radius, boidLayer);
            if (hits.Length >= boidCount * triggerThreshold)
            {
                return true;
            }
            return false;
        }
    }
}
