using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KevinDOMara.Boids2D
{
    public class PlayerController : MonoBehaviour
    {
        public delegate void RequestCreateBoid(Vector3 position);
        public static event RequestCreateBoid OnRequestCreateBoid;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var clickPosition = Input.mousePosition;
                clickPosition.z = 10f;
                clickPosition = Camera.main.ScreenToWorldPoint(clickPosition);

                if (OnRequestCreateBoid != null)
                {
                    OnRequestCreateBoid(clickPosition);
                }
            }
        }
    }
}
