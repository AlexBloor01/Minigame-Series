using UnityEngine;

namespace Pong
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Controller")]
        [SerializeField] private PaddleMovement paddleMovement; //Controls Player Movement. 
        public bool leftHandedMode = true; //Player controls left side player.
        public bool rightHandedMode = true; //Player controls left side player.

        //Right Keys
        private KeyCode upKey = KeyCode.UpArrow;
        private KeyCode downKey = KeyCode.DownArrow;

        //Left Keys
        private KeyCode wKey = KeyCode.W;
        private KeyCode sKey = KeyCode.S;


        // Update is called once per frame.
        void FixedUpdate()
        {
            leftHandedControl();
            RightHandedControl();

            //Update the Players position and extra speed bonus on ball.
            paddleMovement.UpdateHitSpeed();
        }

        //Left hand side of screen controls, dictated by leftHandedMode.
        void leftHandedControl()
        {
            if (leftHandedMode)
            {
                if (Input.GetKey(wKey))
                {
                    paddleMovement.MoveUp();
                }
                if (Input.GetKey(sKey))
                {
                    paddleMovement.MoveDown();
                }
            }
        }

        //Right hand side of screen controls, dictated by rightHandedMode.
        void RightHandedControl()
        {
            if (rightHandedMode)
            {
                if (Input.GetKey(upKey))
                {
                    paddleMovement.MoveUp();
                }
                if (Input.GetKey(downKey))
                {
                    paddleMovement.MoveDown();
                }
            }
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