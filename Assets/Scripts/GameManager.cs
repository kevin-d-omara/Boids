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
            set { _flockingMode = value; }
        }
        [Header("Flock")]
        [SerializeField] private FlockMode _flockingMode;
        [Range(1, 250)]
        [SerializeField] private int flockSize;
        [SerializeField] private Vector2 boundary;

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
        private Transform _waypoint;
        [SerializeField] private List<Transform> Waypoints;

        [Header("Boid")]
        [SerializeField] private GameObject boidPrefab;
        [SerializeField] private Transform boidHolder;

        private int boidCount = 0;

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
        }

        private void Start()
        {
            // Create initial flock.
            for (int i = 0; i < flockSize; ++i)
            {
                var x = Random.Range(-boundary.x, boundary.x);
                var y = Random.Range(-boundary.y, boundary.y);
                CreateBoid(new Vector3(x, y, 0f));
            }
        }

        private void OnEnable()
        {
            PlayerController.OnRequestCreateBoid += CreateBoid;
        }

        private void OnDisable()
        {
            PlayerController.OnRequestCreateBoid -= CreateBoid;
        }

        private void CreateBoid(Vector3 position)
        {
            var instance = Instantiate(boidPrefab, position, Quaternion.identity) as GameObject;
            instance.transform.Rotate(new Vector3(0f, 0f, Random.value * 360f));
            instance.transform.SetParent(boidHolder);

            ++boidCount;
        }
    }
}
