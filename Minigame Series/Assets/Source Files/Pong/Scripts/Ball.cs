using System.Collections;
using UnityEngine;

namespace Pong
{
    public class Ball : MonoBehaviour
    {
        [Header("Ball")]

        [SerializeField] private ScoreManager scoreManager; //Score points here
        Coroutine countdownCoroutine; //Used to remove current start timer, overwise rapid StartGame() would apply
        private readonly float maxXTopSpeed = 20f; //Top speed of Ball on X.
        private float minXSpeed; //Top speed of Ball on minus X, will be changed in later.
        private readonly float maxYTopSpeed = 8f; //Top speed of Ball on Y.
        private float minYMinSpeed; //Top speed of Ball on minus Y, will be changed in later.

        private readonly float initialSpeed = 9f; //start speed of the ball from the beggining.
        private float xMovement = 0f; //current speed of ball on X axis.
        private float yMovement = 0f; //current speed of ball on Y axis.

        private readonly float startGameWaitTimer = 3f; //Pause time between game reset and match start.

        private void Awake()
        {
            CheckReverseVariables();
            StartCoroutine(StartCountdown());
        }

        void CheckReverseVariables()
        {
            minXSpeed = -maxXTopSpeed;
            minYMinSpeed = -maxYTopSpeed;
        }

        //Pause before game starts.
        IEnumerator StartCountdown()
        {
            yield return new WaitForSeconds(startGameWaitTimer);
            StartGame();
        }

        //Flip a coin between which direction.
        public void StartGame()
        {
            //coin flip between 0 and 1.
            int direction = UnityEngine.Random.Range(0, 2);

            //Fire left or fire right.
            if (direction == 0)
            {
                xMovement = -initialSpeed;
            }
            else
            {
                xMovement = initialSpeed;
            }

            //Randomise the up and down speed to give immediate challenge to players.
            yMovement = UnityEngine.Random.Range(-5f, 5f);
        }

        //When paddle collides with ball.
        //Plus increase speed of ball to increase pressure over time on players.
        public void PaddleBounce(float yPlus)
        {
            //flip the x direction.
            xMovement *= -1f;
            //Add yPlus to the y direction speed.
            yMovement += yPlus;

            //Convert yPlus to something the x direction can use.
            if (yPlus != 0)
            {
                yPlus /= 2;
            }

            if (xMovement < 0 && yPlus > 0)
            {
                yPlus *= -1f;
            }
            else if (xMovement > 0 && yPlus < 0)
            {
                yPlus *= -1f;
            }

            //Add yPlus to the x direction speed after conversion.
            xMovement += yPlus;
        }

        //Score point for opposite player to whos goal it hits.
        public void ScorePoint(Owner goalOwner)
        {
            //Score point depending on the goal it belonged too.
            scoreManager.PointGained(goalOwner);

            //restart level.
            Restart();
        }

        public void Restart()
        {
            transform.position = Vector2.zero;
            xMovement = 0f;
            yMovement = 0f;

            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
            }
            countdownCoroutine = StartCoroutine(StartCountdown());
        }

        //When wall is hit, flip the balls y direction.
        public void BoundaryBounce()
        {
            yMovement *= -1;
            // yMovement -= 1f;
        }

        private void FixedUpdate()
        {
            //Update position of ball.
            UpdatePosition();
        }

        //Update position of ball.
        void UpdatePosition()
        {
            //Get new Position then move towards it on a lerp.
            Vector2 aimPosition = new Vector2(transform.position.x + xMovement, transform.position.y + yMovement);
            transform.position = Vector2.Lerp(transform.position, aimPosition, Time.deltaTime);

            //Clamp Ball Speed on both axis.
            xMovement = Mathf.Clamp(xMovement, minXSpeed, maxXTopSpeed);
            yMovement = Mathf.Clamp(yMovement, minYMinSpeed, maxYTopSpeed);
        }
    }
}