using System.Collections;
using System.Collections.Generic;
// using Unity.Mathematics;
using UnityEngine;
// using UnityEditor;

namespace Asteroids
{
    public class AsteroidGenerator : MonoBehaviour
    {
        [Header("Spawn Asteroids at almost Random Locations and Speeds")]
        [SerializeField] private GameObject asteroidObj; //asteroid asset
        public float spawnForce = 30f; //speed of asteroid rigidbody2d force.
        float extraSpace = 5f; //adds extra Instantiate space for outside the map.
        float spawnTime = 5f; //Time between Asteroid Spawns.
        float standardSize = 1f; //1f is standard size of Asteroids, can be increased or decreased
        [SerializeField] private Sprite[] asteroidVisuals;


        private void Start()
        {
            //Start spawning asteroids as soon as spawned in by game manager.
            StartCoroutine(SpawnRoutine());
        }

        //Recursion loop for spawning the asteroids.
        IEnumerator SpawnRoutine()
        {
            //Instantiate new Asteroid.
            NewAsteroid();

            //Check what time in the game it is to ramp up the difficulty.
            if (GameAssets.inGameTimer >= 0)
            {
                spawnTime = 5f;
            }
            if (GameAssets.inGameTimer >= 10f)
            {
                spawnTime = 3f;
            }
            if (GameAssets.inGameTimer >= 20f)
            {
                spawnTime = 1.5f;
            }

            //wait allotted time.
            yield return new WaitForSeconds(spawnTime);

            //Continue Recursion.
            StartCoroutine(SpawnRoutine());
        }

        //Get a Vector2 position either inside or outside playarea.
        Vector2 Vector2Position(float times)
        {
            //Get the lowest possible value of the screen space and heighest.
            float x = UnityEngine.Random.Range(GameAssets.edgeOfScreenX * times * -1, GameAssets.edgeOfScreenX * times);
            float y = UnityEngine.Random.Range(GameAssets.edgeOfScreenY * times * -1, GameAssets.edgeOfScreenY * times);

            return new Vector2(x, y);
        }

        //Create a new Asteroid.
        //Assign spawn position (outside playarea) and direction (inside playarea).
        void NewAsteroid()
        {
            //Get a spawn position, this will rarely return exactly outside.
            Vector2 spawnPoint = Vector2Position(1.2f);
            //Flip a coin on which side will be outside the playarea and apply extraSpace outside the map.
            spawnPoint = FlipCoinForVector2(spawnPoint);

            //Get the spawn position and take it away from a position inside the playarea to get the direction.
            Vector2 forceDirection = Vector2Position(0.8f) - spawnPoint;

            //Allow randomisation of asteroid size.
            float randomSize = UnityEngine.Random.Range(standardSize, standardSize + 0.3f);

            //Instantaite Asteroid using said variables.
            SpawnAsteroid(forceDirection, spawnPoint, randomSize, false);
        }

        //Instantaite Asteroid.
        //direction of force for rigidbody and position to Instantiate.
        //Set the size of Asteroid 1f is standard.
        //entered: if entered playarea already.
        //if the asteroid is not freshly spawned though this script entered set to false.
        public void SpawnAsteroid(Vector2 direction, Vector2 spawnPosition, float size, bool entered)
        {
            //randomise the force speed.
            float newForce = UnityEngine.Random.Range(spawnForce, spawnForce + 0.5f);

            //Instantiate new Asteroid.
            GameObject newAsteroid = Instantiate(asteroidObj, spawnPosition, Quaternion.identity);

            newAsteroid.GetComponent<SpriteRenderer>().sprite = asteroidVisuals[(int)UnityEngine.Random.Range(0, asteroidVisuals.Length)];

            //Add relative force to to its current location.
            newAsteroid.GetComponent<Rigidbody2D>().AddRelativeForce((direction * newForce) * Time.deltaTime, ForceMode2D.Impulse);

            //Set parent to Game Manager to keep scene tidy.
            newAsteroid.transform.parent = transform;

            //Get newAsteroids Script
            Asteroid newAsteroidScript = newAsteroid.GetComponent<Asteroid>();

            //Assign Reference to this script to the new asteroid.
            //Set Size and Assign entered playarea.
            newAsteroidScript.asteroidGenerator = this;
            newAsteroidScript.currentSize = size;
            newAsteroidScript.enteredPlayArea = entered;
        }

        //Flips a coin for which side of the map it will be spawning from.
        //More efficient than using a while loop with the same result.
        Vector2 FlipCoinForVector2(Vector2 pos)
        {
            //Flip coins between 0 and 1 then convert to int.
            int randomValueA = (int)UnityEngine.Random.Range(0, 2); //first random coin toss.
            int randomValueB = (int)UnityEngine.Random.Range(0, 2); //second random coin toss.

            //Check if coin is 0 or 1.
            //Assign to either x, y, -x, or -y for sides.
            if (randomValueA == 0)
            {
                if (randomValueB == 0)
                {
                    pos.y = GameAssets.edgeOfScreenY + extraSpace;
                }
                else if (randomValueB == 1)
                {
                    pos.y = -GameAssets.edgeOfScreenY - extraSpace;
                }
            }
            else if (randomValueA == 1)
            {
                if (randomValueB == 0)
                {
                    pos.x = GameAssets.edgeOfScreenX + extraSpace;
                }
                else if (randomValueB == 1)
                {
                    pos.x = -GameAssets.edgeOfScreenX - extraSpace;
                }
            }

            //return changed Vector2
            return pos;
        }

    }
}
