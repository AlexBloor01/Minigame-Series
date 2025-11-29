using System.Collections;
using System.Collections.Generic;
using Asteroids;
using UnityEngine;

namespace NoughtsAndCrosses
{
    public class UserInterface : MonoBehaviour
    {

        public GameManager gameManager;

        //Keys.
        readonly KeyCode spaceKey = KeyCode.Space;

        private void Awake()
        {

            if (gameManager == null)
            {
                gameManager = FindObjectOfType<GameManager>();
            }
        }

        //Go fullscreen or come out of full screen.
        public void Fullscreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }

        //Resets the game updating all of the required areas.
        public void RestartGame()
        {
            //Play restart sound.
            if (AudioManager.iAudio.restart != null)
            {
                AudioClip restartSound = AudioManager.iAudio.restart;
                AudioManager.iAudio.PlayClipOneShot(restartSound);
            }
            GameAssets.iGameAssets.AssignImageStates();
            gameManager.RestartMatch();
        }

        //if spacebar pressed restart the match.
        //Allows players to gloat a victory and to restart quickly.
        private void Update()
        {
            Controls();
        }

        void Controls()
        {
            if (Input.GetKeyDown(spaceKey))
            {
                RestartGame();
            }
        }
    }
}