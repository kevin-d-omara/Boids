using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinDOMara.Boids2D
{
    public class WaypointController : MonoBehaviour
    {
        public delegate void WaypointFilled(GameObject waypoint);
        public static event WaypointFilled OnWaypointFilled;

        /// <summary>
        /// Percent of boids which must be in the waypoint before it triggers.
        /// </summary>
        [Range(0f, 1f)] public float triggerThreshold = 0.75f;

        /// <summary>
        /// Radius of waypoint trigger region.
        /// </summary>
        [Range(0f, 100f)] public float radius = 20f;

        [SerializeField] private LayerMask boidLayer;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, radius, boidLayer);
            if (hits.Length >= GameManager.Instance.BoidCount * triggerThreshold)
            {
                if (OnWaypointFilled != null)
                {
                    OnWaypointFilled(gameObject);
                }
            }
        }
    }
}
