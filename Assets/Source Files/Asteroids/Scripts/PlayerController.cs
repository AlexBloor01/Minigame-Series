using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Controls")]

        public GameManager gameManager;
        Coroutine coroutine;
        [SerializeField] private Movement movement;
        [SerializeField] private Cannon cannon;
        [SerializeField] private GameObject playerNameObj; //Player Name Object, reference to destory on death.
        [SerializeField] private GameObject explosion; //Death/Hit explosion animation.
        public PlayerSide playerSide; //Only relevant if two player or more.
        int hit = 0; //How many times has player been hit.
        int hitsBeforeDeath = 3; //How many hits before death.
        readonly float deathTick = 10f; //Time for animations before game start over.

        //Keys For Right 
        readonly private KeyCode upKey = KeyCode.UpArrow;
        readonly private KeyCode leftKey = KeyCode.LeftArrow;
        readonly private KeyCode rightKey = KeyCode.RightArrow;
        readonly private KeyCode leftClickKey = KeyCode.Mouse0; //Attack Key

        //Keys for Left
        readonly private KeyCode wKey = KeyCode.W;
        readonly private KeyCode aKey = KeyCode.A;
        readonly private KeyCode dKey = KeyCode.D;
        readonly private KeyCode spaceKey = KeyCode.Space; //Attack Key


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
                HidePlayerName();
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
            if (hit >= hitsBeforeDeath)
            {
                Death();
            }
        }

        //Hiding the Player name outside of script stops the next Instantiate being false.
        void HidePlayerName()
        {
            playerNameObj.SetActive(false);
        }

        //This will restart the game.
        void Death()
        {
            if (coroutine != null)
            {
                //to stop error with death timer.
                StopCoroutine(coroutine);
            }

            //Instantiate explosion Particle System.
            Instantiate(explosion, transform.position, Quaternion.identity);

            if (GameAssets.numberOfPlayers <= 0)
            {
                RestartGame();
            }

            Destroy(gameObject);
        }

        //Restart Game.
        void RestartGame()
        {
            if (gameManager != null)
            {
                gameManager.Restart();
            }
            else
            {
                GameObject.Find("Game Manager").GetComponent<GameManager>().Restart();
            }
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
            //Split controls in two for two players and dont for singleplayer.
            if (playerSide == PlayerSide.Left || GameAssets.singlePlayer)
            {
                //Forward
                if (Input.GetKey(wKey))
                {
                    movement.ForwardThrust();
                }
                else
                {
                    movement.FireOff();
                }
                //Left
                if (Input.GetKey(aKey))
                {
                    movement.TurnLeft();
                }
                //Right
                if (Input.GetKey(dKey))
                {
                    movement.TurnRight();
                }
                //Shoot
                if (Input.GetKeyDown(spaceKey))
                {
                    cannon.FireCannon();
                }
            }

        }
        void RightControls()
        {
            //Split controls in two for two players and dont for singleplayer.
            if (playerSide == PlayerSide.Right || GameAssets.singlePlayer)
            {
                //Forward
                if (Input.GetKey(upKey))
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
                if (Input.GetKey(leftKey))
                {
                    movement.TurnLeft();
                }
                //Right
                if (Input.GetKey(rightKey))
                {
                    movement.TurnRight();
                }
                //Shoot

                if (Input.GetKeyDown(leftClickKey))
                {
                    cannon.FireCannon();
                }
            }
        }

        void Update()
        {
            if (hit <= 0)
            {
                //Check if game is paused.
                if (Time.timeScale >= 1f)
                {
                    LeftControls();
                    RightControls();
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