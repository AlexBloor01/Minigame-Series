using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public class Goal : MonoBehaviour
    {
        [Header("Score Point if ball touches this")]
        public Owner goalOwner; //Who Owns the goal?

        // On Ball overlap with goal trigger the ball to score a point and reset
        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                other.gameObject.GetComponent<Ball>().ScorePoint(goalOwner);
            }
        }

    }

}