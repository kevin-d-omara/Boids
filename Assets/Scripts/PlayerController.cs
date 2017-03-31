using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                instance.transform.SetParent(boidHolder);
            }
        }
    }
}
