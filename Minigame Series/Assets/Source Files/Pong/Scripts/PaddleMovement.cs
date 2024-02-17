using UnityEngine;
namespace Pong
{
    public class PaddleMovement : MonoBehaviour
    {
        [Header("Controls Player Paddle Movement")]

        [SerializeField] private PaddleCollision paddleCollision; //paddleCollision confirms ball touches while transfering information 
        [SerializeField] private float maxHeight = 4.22f; //Max height, change this based on where you want paddle to stop on top and bottom
        private float minHeight = 0f; //this is based on max height and does not need to be changed
        private readonly float maxTopHitSpeed = 7f; //Top speed of paddle
        private readonly float accelerationSpeed = 15f; //Acceleration speed of paddle this * yAxisPosition
        private float yAxisPosition = 0f; //Current position of paddle, 0f is middle of screen
        private float currentHitSpeed = 0f; //current speed of the paddle


        private void Awake()
        {
            //set minimum height of screen
            ReverseMinimumHeight();
        }

        //Allows players to go to bottom of screen.
        void ReverseMinimumHeight()
        {
            minHeight = maxHeight * -1;
        }

        //Move Player Up.
        public void MoveUp()
        {
            //Positive on y position.
            yAxisPosition += Time.deltaTime * accelerationSpeed;

            //Positive Current speed.
            currentHitSpeed += Time.deltaTime * accelerationSpeed;
        }

        //Move Player Down.
        public void MoveDown()
        {
            //Negative on y position.
            yAxisPosition -= Time.deltaTime * accelerationSpeed;

            //Negative Current speed.
            currentHitSpeed -= Time.deltaTime * accelerationSpeed;
        }

        //Updates the position of the paddle and limits it
        public void UpdateHitSpeed()
        {
            //Stop from going above boundaries of the screen.
            yAxisPosition = Mathf.Clamp(yAxisPosition, minHeight, maxHeight);

            //Update position of paddle.
            transform.position = new Vector2(transform.position.x, yAxisPosition);

            //Stop player from going above max speed.
            currentHitSpeed = Mathf.Clamp(currentHitSpeed, -maxTopHitSpeed, maxTopHitSpeed);
            //Slows the player down smoothly
            currentHitSpeed = Mathf.Lerp(currentHitSpeed, 0f, Time.deltaTime * accelerationSpeed);

            //transfer speed to paddle mechanic.
            PaddleSpeedTransfer();
        }

        //Transfer calculations to the paddle to transfer speed and direction to ball on hit.
        //Creates a mechanic for player to play with and stops game from getting stuck for too long.
        void PaddleSpeedTransfer()
        {
            paddleCollision.yPlus = currentHitSpeed;
        }
    }
}