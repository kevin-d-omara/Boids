using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinDOMara.Boids2D
{
    /// <summary>
    /// Extension methods for Unity's Vector2 class.
    /// </summary>
    public static class Vector2Extension
    {
        /// <summary>
        /// Return the rotation direction (clockwise, counter-clockwise, co-linear) between two
        /// vectors via the cross product.
        /// </summary>
        /// <param name="lhs">Left hand side of cross product.</param>
        /// <param name="rhs">Right hand side of cross product.</param>
        /// <returns>+1 if clockwise, -1 if counter-clockwise, 0 if co-linear.</returns>
        public static float RotateTo(this Vector2 lhs, Vector2 rhs)
        {
            var result = lhs.x * rhs.y - rhs.x * lhs.y;
            
            // Clockwise.
            if (result > 0)
            {
                return 1f;
            }
            // Counter-clockwise.
            else if (result < 0)
            {
                return -1f;
            }
            // Co-linear.
            else
            {
                return 0f;
            }
        }
    }
}
