using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Asteroids
{
    public class GameManager : MonoBehaviour
    {
        [Header("Spawns Game Assets")]
        [SerializeField] private GameObject player; //Player Controller Asset.
        [SerializeField] private GameObject asteroidGenerator; //Asteroid Spawner Asset.
        private List<GameObject> gameplayObjects = new List<GameObject>(); //Get game objects that are in play to remove on game restart.

        private void Start()
        {
            StartVariables();
        }

        //Singleplayer is set to true just in case, to stop confusion with new player.
        void StartVariables()
        {
            GameAssets.singlePlayer = true; //Automatically assign to singleplayer at very start just in case.
            Restart();
        }

        //Resets the whole game to set parameters.
        public void Restart()
        {
            Debug.Log("Restart Game");

            //Reset Required variables
            ResetGameVariables();

            //Destroy previous game assets
            DestroyGameAssets();

            //Spawn assets needed in order
            InstantiateRequiredItems();
        }

        //resets variables that will change depending on game settings that need resetting each game.
        void ResetGameVariables()
        {
            GameAssets.score = 0; //reset score.
            GameAssets.inGameTimer = 0; //timer needs to be reset for new game.
            GameAssets.numberOfPlayers = 0; //reset number of players, waiting to see how many.
        }

        //Destroys game objects set from previous game
        void DestroyGameAssets()
        {
            //Destroy previous game assets.
            if (gameplayObjects.Count > 0)
            {
                foreach (GameObject obj in gameplayObjects)
                {
                    Destroy(obj);
                }
                //Clear List for new objects to spawn.
                gameplayObjects.Clear();
            }
        }

        //Spawns all required objects (Player and Asteroid Generator) in order.
        void InstantiateRequiredItems()
        {
            //Spawn players depending on settings.
            //Check if singleplayer or not.
            if (GameAssets.singlePlayer)
            {
                InstantiateItem(player, PlayerSide.Left);
            }
            else
            {
                InstantiateItem(player, PlayerSide.Left);
                InstantiateItem(player, PlayerSide.Right);
            }

            //Spawn Asteroid Generator.
            InstantiateItem(asteroidGenerator, PlayerSide.Left);

        }

        // Spawn an item (player or asteroid generator) in the game world.
        // PlayerSide is needed for multiplayer only.
        void InstantiateItem(GameObject obj, PlayerSide side)
        {
            //Instantiate Item and set its name
            GameObject newobj = Instantiate(obj, new Vector3(), Quaternion.identity);
            newobj.name = newobj.name;

            //Set this as parent to keep the scene neat.
            newobj.transform.parent = transform;

            //Add to a list so it can be deleted later.
            gameplayObjects.Add(newobj);

            // For multiplayer, controller settings.
            if (obj.tag == player.tag && GameAssets.singlePlayer == false)
            {
                //Increment number of players, only needed with multiplayer.
                GameAssets.numberOfPlayers++;

                //assign a control scheme to each player
                newobj.GetComponent<PlayerController>().playerSide = side;

                //create one to the left and one to the right of center screen.
                Vector2 newPos = new Vector2(0f, 0f);
                if (side == PlayerSide.Left)
                {
                    newPos.x += -1f;
                }
                else
                {
                    newPos.x += 1f;
                }

                //assign new position.
                newobj.transform.position = newPos;
            }
        }

    }
}