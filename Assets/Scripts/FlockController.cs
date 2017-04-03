using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace KevinDOMara.Boids3D
{
    public enum FlockMode { LazyFlight, Waypoint, FollowTheLeader }

    public class FlockController : MonoBehaviour
    {
        [Header("Flock")]
        public FlockMode flockMode = FlockMode.LazyFlight;
        public Dropdown modeSelector;
        [Range(1, 250)] public int flockSize = 10;
        public GameObject boidPrefab;

        private List<GameObject> flock = new List<GameObject>();

        [Header("Waypoints")]
        public Vector3 boundary = Vector3.one * 10f;
        public WaypointController waypoint;
        public List<Transform> orderedWaypointLocations;

        private Queue<Transform> orderedWaypoints = new Queue<Transform>();

        private void OnEnable()
        {
            modeSelector.onValueChanged.AddListener(delegate {
                SetFlockMode((FlockMode)modeSelector.value); });
        }

        private void OnDisable()
        {
            modeSelector.onValueChanged.RemoveAllListeners();
        }

        private void Start()
        {
            // Create set of ordered waypoints.
            for (int i = 0; i < orderedWaypointLocations.Count; ++i)
            {
                orderedWaypoints.Enqueue(orderedWaypointLocations[i]);
            }

            SetFlockMode(flockMode);

            // Create initial flock.
            for (int i = 0; i < flockSize; ++i)
            {
                CreateBoid(GetRandomPositionInBounds());
            }

            // Populate flock mode dropdown.
            var modes = new List<FlockMode>((FlockMode[])FlockMode.GetValues(typeof(FlockMode)));
            foreach (FlockMode mode in modes)
            {
                modeSelector.AddOptions(new List<string>() { mode.ToString() });
            }
            modeSelector.value = (int)flockMode;
        }

        private void FixedUpdate()
        {
            if (flockMode != FlockMode.FollowTheLeader && waypoint.IsFilled(flock.Count))
            {
                SetFlockMode(flockMode);
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

        /// <summary>
        /// Return a point within the bounds of the flock.
        /// </summary>
        public Vector3 GetRandomPositionInBounds()
        {
            var x = Random.Range(-boundary.x, boundary.x);
            var y = Random.Range(-boundary.y, boundary.y);
            var z = Random.Range(-boundary.z, boundary.z);
            return new Vector3(x, y, z) + transform.position;
        }

        private void TurnWaypointMovementOn()
        {
            var leader = waypoint.GetComponent<LeaderController>();
            leader.enabled = true;
        }

        private void TurnWaypointMovementOff()
        {
            var leader = waypoint.GetComponent<LeaderController>();
            leader.enabled = false;
        }

        private void SetFlockMode(FlockMode mode)
        {
            flockMode = mode;
            switch (mode)
            {
                case FlockMode.LazyFlight:
                    TurnWaypointMovementOff();
                    waypoint.transform.position = GetRandomPositionInBounds();
                    break;
                case FlockMode.Waypoint:
                    TurnWaypointMovementOff();
                    var nextWaypoint = orderedWaypoints.Dequeue();
                    waypoint.transform.position = nextWaypoint.position;
                    orderedWaypoints.Enqueue(nextWaypoint);
                    break;
                case FlockMode.FollowTheLeader:
                    TurnWaypointMovementOn();
                    break;
                default:
                    throw new System.ArgumentException("Flocking Mode not implemented.");
            }
        }
    }
}
