using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public class Boundary : MonoBehaviour
    {

        //Checks if ball has collided with top or bottom wall to reverse the y axis
        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Ball")
            {
                other.gameObject.GetComponent<Ball>().BoundaryBounce();
            }
        }
    }
}
