using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public class AIController : MonoBehaviour
    {
        [Header("AI Controller works through Movement")]
        [SerializeField] private PlayerController playerController; //for turning off player controller controls to this player.
        [SerializeField] private PaddleMovement paddleMovement; //controls movement to seem more like player.
        [SerializeField] private GameObject ball; //get ball position for input into movement, follow ball.

        //how far away does the ball need to be to start movement.
        //9-10.5f is the best for the regular player.
        private float reactionTime = 10f; //Higher reaction times mean faster movement and harder gameplay. 

        //Difficulty modes for reaction times.
        // private float mode_Easy = 5f;
        // private float mode_Normal = 9.5f; 
        // private float mode_Hard = 12f;

        //How far should ai drag behind ball position before updating.
        float delay = 0.5f; //Lower delay, harder gameplay, minimum needs to be 0.1f to stop visually paddle flickering.

        //Difficulty modes for delay.
        // private float mode_Easy_Delay = 0.8f;
        // private float mode_Normal_Delay = 0.5f;
        // private float mode_Hard_Delay = 0.3f;

        private void Update()
        {
            TrackBallMovement();
        }

        //Check the balls position and move accordingly.
        void TrackBallMovement()
        {
            //If ball is within distance check reactionTime to get difficulty.
            //This reaction time allows for easier gameplay and gives a sense of thought to the player.
            if (Vector3.Distance(transform.position, ball.transform.position) <= reactionTime)
            {
                //Move toward ball with a drag on its position.
                if (ball.transform.position.y - delay > transform.position.y + delay)
                {
                    paddleMovement.MoveUp();
                }
                else if (ball.transform.position.y + delay < transform.position.y - delay)
                {
                    paddleMovement.MoveDown();
                }
                else if (ball.transform.position.y == transform.position.y)
                {
                    //Do nothing.
                }
                else
                {
                    //Do nothing.
                }
            }

            //Update the ai's position and extra speed bonus on ball.
            paddleMovement.UpdateHitSpeed();
        }

        //BotMode, take control of player controller.
        public void AiControl()
        {
            playerController.leftHandedMode = false;
            playerController.rightHandedMode = false;
        }
    }
}