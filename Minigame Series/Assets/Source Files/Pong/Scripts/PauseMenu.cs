using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pong
{
    public class PauseMenu : MonoBehaviour
    {
        [Header("Pause Menu and Mode Switcher")]
        //Place this script on Pause Menu Object in canvas.
        //Allows the switching to different Modes
        private bool gamePaused = false; //is the game paused currently?
        public Ball ball; //Get ball script for match restarts.
        public ScoreManager scoreManager; //Get Score Manager script for match restarts.

        //Player references allow access to multiple scripts in the player gameobjects.
        [SerializeField] private GameObject player_left; //Reference to left Player.
        [SerializeField] private GameObject player_right; //Reference to left Player.
        Vector3 openScale = new Vector3(1f, 1f, 1f); //Represents the scale when the settings panel is open.
        Vector3 closedScale = new Vector3(); //Represents the scale when the settings panel is closed (initially set to zero scale).

        //Keys
        readonly KeyCode spaceKey = KeyCode.Space;
        readonly KeyCode escKey = KeyCode.Escape;


        // Update is called once per frame
        void Update()
        {
            Controls();
        }

        void Controls()
        {
            //Space to restart match.
            if (Input.GetKeyDown(spaceKey))
            {
                RestartMatch();
            }

            //Escape to pause/unpause game.
            if (Input.GetKeyDown(escKey))
            {
                if (gamePaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        //Shows pause menu and freezes time.
        public void PauseGame()
        {
            //Make Pause Menu big
            transform.localScale = openScale;
            gamePaused = true;

            //Freeze Time.
            Time.timeScale = 0;
        }

        //Removes pause menu and unfreezes time.
        public void ResumeGame()
        {
            //Make Pause Menu Small
            transform.localScale = closedScale;
            gamePaused = false;

            //Unfreeze Time.
            Time.timeScale = 1;
        }

        //Restarts whole game.
        public void RestartMatch()
        {
            ResumeGame();

            //Reset Score.
            scoreManager.ResetScore();

            //Restarts balls position.
            ball.Restart();
        }

        //Sets mode to Single Player.
        public void SinglePlayer()
        {
            //Disable AIController for left and enable for right.
            player_left.GetComponent<AIController>().enabled = false;
            player_right.GetComponent<AIController>().enabled = true;

            //Let the player controllers know their roles.
            player_left.GetComponent<PlayerController>().SinglePlayer();
            player_right.GetComponent<AIController>().AiControl();

            //Resume Current Game, allows players to mess about with different modes while in the same match.
            ResumeGame();
        }

        //Coop/Competative mode of two real players.
        public void VsMode()
        {
            //Disable all ai controllers.
            player_left.GetComponent<AIController>().enabled = false;
            player_right.GetComponent<AIController>().enabled = false;

            //Assign each player control.
            player_left.GetComponent<PlayerController>().LeftSide();
            player_right.GetComponent<PlayerController>().RightSide();

            //Resume Current Game, allows players to mess about with different modes while in the same match.
            ResumeGame();
        }

        //Watch two bots fight
        public void BotMatch()
        {
            //Enable both controllers.
            player_left.GetComponent<AIController>().enabled = true;
            player_right.GetComponent<AIController>().enabled = true;

            //Let the player controllers know their roles.
            player_left.GetComponent<AIController>().AiControl();
            player_right.GetComponent<AIController>().AiControl();

            //Resume Current Game, allows players to mess about with different modes while in the same match.
            ResumeGame();
        }

        //Flip from current screen view to full and back
        public void FullScreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }

    }
}

