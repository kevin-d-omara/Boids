using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KevinDOMara.Boids3D
{
    public enum FlockMode { LazyFlight, Waypoint, FollowTheLeader }

    public class FlockController : MonoBehaviour
    {
        [Header("Flock")]
        public FlockMode flockMode = FlockMode.LazyFlight;
        [Range(1, 250)] public int flockSize = 10;
        public GameObject boidPrefab;

        private List<GameObject> flock = new List<GameObject>();

        [Header("Waypoints")]
        public Vector3 boundary = Vector3.one * 10f;
        public GameObject waypointPrefab;
        public List<Transform> orderedWaypointLocations;

        private WaypointController waypoint;
        private Queue<Transform> orderedWaypoints = new Queue<Transform>();

        private void Start()
        {
            // Create set of ordered waypoints.
            for (int i = 0; i < orderedWaypointLocations.Count; ++i)
            {
                orderedWaypoints.Enqueue(orderedWaypointLocations[i]);
            }

            // Create waypoint.
            var instance = Instantiate(waypointPrefab, transform.position, Quaternion.identity)
                as GameObject;
            instance.transform.SetParent(transform);
            instance.GetComponent<MeshRenderer>().enabled = false;
            instance.GetComponent<BoxCollider>().enabled = false;
            waypoint = instance.GetComponent<WaypointController>();

            // Set waypoint position
            switch (flockMode)
            {
                case FlockMode.LazyFlight:
                    waypoint.transform.position = GetRandomPositionInBounds(boundary);
                    break;
                case FlockMode.Waypoint:
                    var nextWaypoint = orderedWaypoints.Dequeue();
                    waypoint.transform.position = nextWaypoint.position;
                    orderedWaypoints.Enqueue(nextWaypoint);
                    break;
                case FlockMode.FollowTheLeader:
                    throw new System.NotImplementedException();
                    break;
                default:
                    throw new System.ArgumentException("Flocking Mode not implemented.");
            }

            // Create initial flock.
            for (int i = 0; i < flockSize; ++i)
            {
                CreateBoid(GetRandomPositionInBounds(boundary));
            }
        }

        private void FixedUpdate()
        {
            if (flockMode != FlockMode.FollowTheLeader && waypoint.IsFilled(flock.Count))
            {
                switch (flockMode)
                {
                    case FlockMode.LazyFlight:
                        waypoint.transform.position = GetRandomPositionInBounds(boundary);
                        break;
                    case FlockMode.Waypoint:
                        var nextWaypoint = orderedWaypoints.Dequeue();
                        waypoint.transform.position = nextWaypoint.position;
                        orderedWaypoints.Enqueue(nextWaypoint);
                        break;
                    case FlockMode.FollowTheLeader:
                        throw new System.InvalidOperationException("This line should never be reached");
                    default:
                        throw new System.ArgumentException("Flocking Mode not implemented.");
                }
            }
        }

        private void CreateBoid(Vector3 position)
        {
            var instance = Instantiate(boidPrefab, position, Quaternion.identity) as GameObject;
            instance.transform.Rotate(Random.insideUnitSphere * 360f);
            instance.transform.SetParent(transform);
            flock.Add(instance);

            instance.GetComponent<BoidController>().waypoint = waypoint.transform;
        }

        private Vector3 GetRandomPositionInBounds(Vector3 boundary)
        {
            var x = Random.Range(-boundary.x, boundary.x);
            var y = Random.Range(-boundary.y, boundary.y);
            var z = Random.Range(-boundary.z, boundary.z);
            return new Vector3(x, y, z) + transform.position;
        }
    }
}
