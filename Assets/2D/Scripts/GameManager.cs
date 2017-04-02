using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KevinDOMara.Boids2D
{
    public enum FlockMode { LazyFlight, Waypoint, FollowTheLeader }

    public class GameManager : MonoBehaviour
    {
        public delegate void WaypointChanged(Transform waypoint);
        public static event WaypointChanged OnWaypointChanged;

        public static GameManager Instance { get; private set; }

        public FlockMode FlockingMode
        {
            get { return _flockingMode; }
            set
            {
                _flockingMode = value;
            }
        }
        [Header("Flock")]
        [SerializeField] private FlockMode _flockingMode;
        [Range(1, 250)]
        [SerializeField] private int flockSize;
        [SerializeField] private Vector2 boundary;
        private List<GameObject> flock = new List<GameObject>();

        public Transform Waypoint
        {
            get { return _waypoint; }
            set
            {
                _waypoint = value;
                if (OnWaypointChanged != null)
                {
                    OnWaypointChanged(_waypoint);
                }
            }
        }
        [Header("Waypoints")]
        private Transform _waypoint;
        [SerializeField] private List<Transform> Waypoints;
        private int waypointCounter = 0;
        [SerializeField] private Transform LazyWaypoint;
        [SerializeField] private Transform Leader;

        [Header("Boid")]
        [SerializeField] private GameObject boidPrefab;
        [SerializeField] private Transform boidHolder;

        public int BoidCount { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            BoidCount = 0;

            // Set lazy waypoint.
            LazyWaypoint.position = GetRandomPositionInBounds(boundary);

            // Set waypoint.
            switch (FlockingMode)
            {
                case FlockMode.LazyFlight:
                    Waypoint = LazyWaypoint;
                    break;
                case FlockMode.Waypoint:
                    Waypoint = Waypoints[0];
                    break;
                case FlockMode.FollowTheLeader:
                    Leader.GetComponent<SpriteRenderer>().enabled = true;
                    Waypoint = Leader;
                    break;
                default:
                    throw new System.ArgumentException("Flocking Mode not implemented.");
            }
            Waypoint.gameObject.SetActive(true);
        }

        private void Start()
        {
            // Create initial flock.
            for (int i = 0; i < flockSize; ++i)
            {
                CreateBoid(GetRandomPositionInBounds(boundary));
            }
        }

        private void OnEnable()
        {
            PlayerController.OnRequestCreateBoid += CreateBoid;
            WaypointController.OnWaypointFilled += OnWaypointFilled;
        }

        private void OnDisable()
        {
            PlayerController.OnRequestCreateBoid -= CreateBoid;
            WaypointController.OnWaypointFilled -= OnWaypointFilled;
        }

        private void CreateBoid(Vector3 position)
        {
            var instance = Instantiate(boidPrefab, position, Quaternion.identity) as GameObject;
            instance.transform.Rotate(new Vector3(0f, 0f, Random.value * 360f));
            instance.transform.SetParent(boidHolder);
            flock.Add(instance);

            ++BoidCount;
        }

        private Vector3 GetRandomPositionInBounds(Vector2 boundary)
        {
            var x = Random.Range(-boundary.x, boundary.x);
            var y = Random.Range(-boundary.y, boundary.y);
            return new Vector3(x, y, 0f);
        }

        private void OnWaypointFilled(GameObject waypoint)
        {
            waypoint.SetActive(false);

            // Set waypoint.
            switch (FlockingMode)
            {
                case FlockMode.LazyFlight:
                    LazyWaypoint.gameObject.SetActive(true);
                    LazyWaypoint.position = GetRandomPositionInBounds(boundary);
                    Waypoint = LazyWaypoint;
                    break;
                case FlockMode.Waypoint:
                    waypointCounter = (waypointCounter + 1) % Waypoints.Count;
                    Waypoints[waypointCounter].gameObject.SetActive(true);
                    Waypoint = Waypoints[waypointCounter];
                    break;
                case FlockMode.FollowTheLeader:
                    Leader.GetComponent<SpriteRenderer>().enabled = true;
                    Waypoint = Leader;
                    break;
                default:
                    throw new System.ArgumentException("Flocking Mode not implemented.");
            }
            Waypoint.gameObject.SetActive(true);
        }
    }
}
