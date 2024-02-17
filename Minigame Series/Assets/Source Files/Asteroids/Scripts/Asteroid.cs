using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Asteroids
{
    public class Asteroid : MonoBehaviour
    {
        [Header("Enemy Asteroid, Beware!")]
        public AsteroidGenerator asteroidGenerator; //Astroid Instantiator, use this as the command ship of the asteroids
        [SerializeField] private Rigidbody2D rb; //this rigidbody
        public bool enteredPlayArea = false; //has asteroid entered playarea
        public float currentSize = 1f; //Current Size of object, this controls division
        private float spinSpeed = 7.5f;//Torque speed added
        int scorePlus = 50; //Most points after destorying asteroid with current size at 1f (currentSize)

        //When Spawned set Spin and Size.
        private void Start()
        {
            SetTorque();
            SetSize();
        }

        void SetSize()
        {
            //Set the size to currentSize.
            transform.localScale = new Vector2(currentSize, currentSize);
        }

        //Assign a spin factor to the asteroid for visual effect.
        void SetTorque()
        {

            float torque = UnityEngine.Random.Range(-spinSpeed, spinSpeed);
            rb.AddTorque(torque, ForceMode2D.Impulse);
        }

        //When shot halve the current size of the next two and assign velocity to rigidbody.
        public void BreakApart()
        {
            //Increase score for smaller asteroids due to size difficult.
            scorePlus *= 1 + (int)currentSize;
            GameAssets.score += scorePlus;

            //Reduce size to Check if asteroid is large enough for dividing.
            currentSize /= 2;

            if (currentSize > 0.2f) //with the largest size at 1f this should give 7 asteroids.
            {
                //slow down the speed of the new asteroids by timesing by current size.
                float splitSpeed = asteroidGenerator.spawnForce * currentSize;

                //use this to alter the position of spawned asteroid, this should be small.
                float posAlter = 0.1f;

                //split into two asteroids half the size, with the same velocity directions slightly sped up.
                //They usually collide to create dispersal.
                asteroidGenerator.SpawnAsteroid(new Vector2(rb.velocity.x * splitSpeed, rb.velocity.y * splitSpeed), new Vector2(transform.position.x - posAlter, transform.position.y + posAlter), currentSize, true);
                asteroidGenerator.SpawnAsteroid(new Vector2(rb.velocity.y * splitSpeed, rb.velocity.y * splitSpeed), new Vector2(transform.position.x + posAlter, transform.position.y - posAlter), currentSize, true);
            }
            Destroy(gameObject);
        }

        //If touches player end game.
        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerController>().Hit();
            }
        }

        private void LateUpdate()
        {
            if (enteredPlayArea)
            {
                //check if the object is inside the containing area and teleport when needed.
                GameAssets.ContainingArea(gameObject);
            }
            else
            {
                //This allows asteroids to smoothly fly in, rather than appear, ignoring the containing barrier.
                if (GameAssets.WithinArea(gameObject.transform.position))
                {
                    enteredPlayArea = true;
                }
            }

        }
    }
}
