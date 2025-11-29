using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids
{
    public class Settings : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private GameManager gameManager; //Used for game restart.
        [SerializeField] private Transform settingsPanel; //For scaling the settings menu.
        private bool gamePaused = false; //Check if settings is currently open, set to false on game begin.
        Vector3 openScale = new Vector3(1f, 1f, 1f);  //Represents the scale when the settings panel is open.
        Vector3 closedScale = new Vector3();  //Represents the scale when the settings panel is closed (initially set to zero scale).

        //Keys
        readonly KeyCode escKey = KeyCode.Escape;


        private void Awake()
        {
            CheckVariables();
        }

        //Check if gameManager and SettingsPanel transform are empty for some reason
        void CheckVariables()
        {

            if (settingsPanel == null)
            {
                settingsPanel = this.gameObject.transform;
            }


            gamePaused = false;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            Controls();
        }

        void Controls()
        {
            //Escape to open settings menu.
            if (Input.GetKeyDown(escKey))
            {
                if (gamePaused == true)
                {
                    SettingsClose();

                }
                else
                {
                    SettingsOpen();
                }
            }
        }

        //Open Settings.
        void SettingsOpen()
        {
            //Enlarge Settings.
            settingsPanel.localScale = openScale;
            gamePaused = true;

            //Freeze time.
            Time.timeScale = 0f;
        }

        //Close Settings.
        void SettingsClose()
        {
            //Minimize Settings.
            settingsPanel.localScale = closedScale;
            gamePaused = false;

            //Unfreeze time.
            Time.timeScale = 1f;
        }

        //stick multiplayer on and restart game.
        public void TwoPlayers()
        {
            GameAssets.singlePlayer = false;
            RestartLevel();
        }

        //Stick singlePlayer on and restart game.
        public void SinglePlayer()
        {
            GameAssets.singlePlayer = true;
            RestartLevel();
        }

        //Go fullscreen.
        public void Fullscreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }

        //Restart Game.
        public void RestartLevel()
        {
            if (gameManager == null)
            {
                gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            }

            //Close the settings menu if open.
            SettingsClose();
            gameManager.Restart();
        }
    }
}