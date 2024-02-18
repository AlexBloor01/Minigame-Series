using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public class PaddleCollision : MonoBehaviour
    {
        [Header("Checks if Ball is Colliding with Player Paddle")]

        public float yPlus = 0f;

        //Checks on collision with ball to reverse the x axis
        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                //Transfers bounce information to Ball so it can change direction based on current player paddle movement
                other.gameObject.GetComponent<Ball>().PaddleBounce(yPlus * 10);
            }
        }
    }
}