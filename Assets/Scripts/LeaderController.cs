using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinDOMara.Boids3D
{
    [RequireComponent(typeof(MeshRenderer))]
    public class LeaderController : MonoBehaviour
    {
        [Header("Flight Characteristics")]
        [Range(1f, 25f)] public float constantSpeed = 5f;
        [Range(0f, 5f)] public float directionalNoise = 0.25f;

        public FlockController flock;

        private Vector3 waypoint;
        private bool isWaiting = false;

        private Vector3 Heading { get { return transform.forward; } }
        private Rigidbody rigidBody;
        private MeshRenderer meshRenderer;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            waypoint = flock.GetRandomPositionInBounds();
        }

        private void OnEnable()
        {
            meshRenderer.enabled = true;
        }

        private void OnDisable()
        {
            meshRenderer.enabled = false;
            rigidBody.velocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            // Check if at waypoint.
            var relativePos = waypoint - transform.position;
            if (relativePos.sqrMagnitude < 0.1f)
            {
                waypoint = flock.GetRandomPositionInBounds();
            }

            // Rotate toward waypoint.
            var rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = rotation;

            // Move forward.
            rigidBody.velocity = constantSpeed * Heading;
        }
    }
}
