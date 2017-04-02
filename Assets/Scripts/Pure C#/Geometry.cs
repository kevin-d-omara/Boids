using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinDOMara.Boids3D
{
    public class Geometry : MonoBehaviour
    {
        /// <summary>
        /// Return all GameObjects within a sphere at the position. Discludes objects on the
        /// IgnoreRaycastLayer.
        /// </summary>
        public static GameObject[] FindObjectsInSphere(Vector3 point, float radius)
        {
            var hits = Physics.OverlapSphere(point, radius);

            GameObject[] objects = new GameObject[hits.Length];
            for (int i = 0; i < hits.Length; ++i)
            {
                objects[i] = hits[i].gameObject;
            }
            return objects;
        }

        /// <summary>
        /// Return all components (objects) of type T within a sphera at the position. Discludes
        /// objects on the IgnoreRayCastLayer.
        /// </summary>
        public static IList<T> FindComponentsInSphere<T>(Vector3 point, float radius)
        {
            var components = new List<T>();

            var gameObjects = FindObjectsInSphere(point, radius);
            foreach (GameObject gameObject in gameObjects)
            {
                var component = gameObject.GetComponent<T>();
                if (component != null)
                {
                    components.Add(component);
                }
            }

            return components;
        }
    }
}
