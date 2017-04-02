using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinDOMara.Boids2D
{
    public static class Geometry
    {
        /// <summary>
        /// Return all GameObjects within a circle at the position. Discludes objects on the
        /// IgnoreRaycastLayer.
        /// </summary>
        /// <returns>Array of all GameObjects within a circle at the position.</returns>
        public static GameObject[] FindObjectsInCircle(Vector2 point, float radius)
        {
            var hits = Physics2D.OverlapCircleAll(point, radius);

            GameObject[] objects = new GameObject[hits.Length];
            for (int i = 0; i < hits.Length; ++i)
            {
                objects[i] = hits[i].gameObject;
            }
            return objects;
        }

        /// <summary>
        /// Return all components (objects) of type T within a circle at the position. Discludes
        /// objects on the IgnoreRayCastLayer.
        /// </summary>
        /// <returns>List of all Components within a circle at the position.</returns>
        public static IList<T> FindComponentsInCircle<T>(Vector2 point, float radius)
        {
            var components = new List<T>();

            var gameObjects = FindObjectsInCircle(point, radius);
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
