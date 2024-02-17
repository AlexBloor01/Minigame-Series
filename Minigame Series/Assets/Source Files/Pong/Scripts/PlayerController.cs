using UnityEngine;

namespace Pong
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Controller")]
        [SerializeField] private PaddleMovement paddleMovement; //Controls Player Movement. 
        public bool leftHandedMode = true; //Player controls left side player.
        public bool rightHandedMode = true; //Player controls left side player.

        // Update is called once per frame.
        void FixedUpdate()
        {
            if (leftHandedMode)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    paddleMovement.MoveUp();
                }
                if (Input.GetKey(KeyCode.S))
                {
                    paddleMovement.MoveDown();
                }
            }

            if (rightHandedMode)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    paddleMovement.MoveUp();
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    paddleMovement.MoveDown();
                }
            }

            //Update the Players position and extra speed bonus on ball.
            paddleMovement.UpdateHitSpeed();
        }

        //SinglePlayer Controls
        public void SinglePlayer()
        {
            leftHandedMode = true;
            rightHandedMode = true;
        }

        //Multiplayer Controls
        public void LeftSide()
        {
            leftHandedMode = true;
            rightHandedMode = false;
        }
        //Multiplayer Controls
        public void RightSide()
        {
            leftHandedMode = false;
            rightHandedMode = true;
        }
    }
}