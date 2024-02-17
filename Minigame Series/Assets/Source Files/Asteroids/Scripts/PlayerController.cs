using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Controls")]

        Coroutine coroutine;
        [SerializeField] private Movement movement;
        [SerializeField] private Cannon cannon;
        [SerializeField] private GameObject explosion; //death/hit explosion animation.
        public PlayerSide playerSide; //only relevant if two player or more.
        int hit = 0; //how many times has player been hit.
        readonly float deathTick = 10f; //Time for animations before game start over.

        //set this to true to start the game for player.
        private void Start()
        {
            GameAssets.gamePlaying = true;
        }

        //Is player Collides with asteroid, this will allow player to spin out with no controls before game restart, more cinematic.
        public void Hit()
        {

            if (hit <= 0)
            {
                //player has no controls and is out for the count after one hit, health could be added.
                GameAssets.numberOfPlayers--;
            }

            //If there is still a player alive do not stop game, timer, or score.
            if (GameAssets.numberOfPlayers <= 0)
            {
                GameAssets.gamePlaying = false;

            }

            hit++;
            coroutine = StartCoroutine(DeathTimer());
            Instantiate(explosion, transform.position, Quaternion.identity); //blow up anim.

            //if hit more than 3 times blow up.
            if (hit >= 3)
            {
                Death();
            }
        }

        //This will restart the game.
        void Death()
        {
            if (coroutine != null)
            {
                //to stop error with death timer.
                StopCoroutine(coroutine);
            }
            Instantiate(explosion, transform.position, Quaternion.identity);

            Debug.Log($"Number of players before <= 0 check {GameAssets.numberOfPlayers}");
            if (GameAssets.numberOfPlayers <= 0)
            {
                RestartGame();
            }

            Destroy(gameObject);
        }

        //Restart Game.
        void RestartGame()
        {
            GameObject.Find("Game Manager").GetComponent<GameManager>().Restart();
        }

        //This will stop players drifting forever if they have not been hit 3 times in 3 seconds.
        IEnumerator DeathTimer()
        {
            //time after hit before game starts over automatically, gives time for animations.
            yield return new WaitForSeconds(deathTick);
            Death();
        }

        void LeftControls()
        {
            //Forward
            if (Input.GetKey(KeyCode.W))
            {
                movement.ForwardThrust();
            }
            else
            {
                movement.FireOff();
            }
            //Left
            if (Input.GetKey(KeyCode.A))
            {
                movement.TurnLeft();
            }
            //Right
            if (Input.GetKey(KeyCode.D))
            {
                movement.TurnRight();
            }
            //Shoot
            if (Input.GetKeyDown(KeyCode.Space))
            {
                cannon.FireCannon();
            }

        }
        void RightControls()
        {
            //Forward
            if (Input.GetKey(KeyCode.UpArrow))
            {
                movement.ForwardThrust();
            }
            else
            {
                //If singleplayer on then dont turn this off twice.
                if (!GameAssets.singlePlayer)
                {
                    movement.FireOff();
                }
            }
            //Left
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                movement.TurnLeft();
            }
            //Right
            if (Input.GetKey(KeyCode.RightArrow))
            {
                movement.TurnRight();
            }
            //Shoot
            if (Input.GetMouseButtonDown(0))
            {
                cannon.FireCannon();
            }
        }

        void Update()
        {
            if (hit <= 0)
            {
                if (GameAssets.singlePlayer) //If singleplayer mode then either controls.
                {
                    LeftControls();
                    RightControls();
                }
                else
                {
                    //Split controls in two for two players.
                    if (playerSide == PlayerSide.Left)
                    {
                        LeftControls();
                    }

                    if (playerSide == PlayerSide.Right)
                    {
                        RightControls();
                    }
                }
            }
            else
            {
                //Turns off booster rockets when player has been hit, as controls have been disabled.
                movement.FireOff();
            }
        }
    }
}