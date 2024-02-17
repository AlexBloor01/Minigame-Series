using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids
{
    public class Movement : MonoBehaviour
    {
        [Header("Controls Movement")]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private SpriteRenderer fire;
        private float thrustSpeed = 1f;
        private float rotationSpeed = 220f;

        //Pushes player in direction of player object.
        public void ForwardThrust()
        {
            fire.enabled = true; //Turn on rocket booster visuals.
            rb.AddRelativeForce(new Vector2(0f, 1f) * thrustSpeed * Time.deltaTime, ForceMode2D.Impulse);

            //Early Experiment to try and move without Rigidbody.
            #region Experiment
            //Experiment gone wrong, oh well :D
            // float x = 0;
            // float y = 0;
            // float percentage = rb.rotation / 360;

            // Map x and y based on the rotation percentage
            // x = Mathf.Sin(percentage * 2 * Mathf.PI); // oscillates between -1 and 1
            // y = Mathf.Cos(percentage * 2 * Mathf.PI); // oscillates between -1 and 1

            // Vector2 direction = new Vector2(x, y);
            // rb.velocity = direction;
            #endregion
        }

        //Turn off rocket booster visuals.
        public void FireOff()
        {
            fire.enabled = false;
        }

        //Rotates Player left.
        public void TurnLeft()
        {
            rb.rotation += rotationSpeed * Time.deltaTime;
        }

        //Rotates Player Right.
        public void TurnRight()
        {
            rb.rotation -= rotationSpeed * Time.deltaTime;
        }

        //Keeps player inside play area.
        private void Update()
        {
            //Keeps player inside play area.
            GameAssets.ContainingArea(gameObject);
        }

    }
}
