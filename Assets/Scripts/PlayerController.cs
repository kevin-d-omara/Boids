using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KevinDOMara.Boids2D
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject boidPrefab;
        [SerializeField] private Transform boidHolder;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var clickPosition = Input.mousePosition;
                clickPosition.z = 10f;
                clickPosition = Camera.main.ScreenToWorldPoint(clickPosition);

                // Create Boid!
                var instance = Instantiate(boidPrefab, clickPosition, Quaternion.identity) as GameObject;
                instance.transform.Rotate(new Vector3(0f, 0f, Random.value * 360f));
                instance.transform.SetParent(boidHolder);
            }
        }
    }
}
