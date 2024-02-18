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
        bool settingsOpen = false; //Check if settings is currently open, set to false on game begin.

        //Represents the scale when the settings panel is open.
        Vector3 openScale = new Vector3(1f, 1f, 1f);

        //Represents the scale when the settings panel is closed (initially set to zero scale).
        Vector3 closedScale = new Vector3();

        //Keys
        readonly KeyCode escKey = KeyCode.Escape;


        private void Awake()
        {
            CheckVariables();
        }

        //Check if gameManager and SettingsPanel transform are empty for some reason
        void CheckVariables()
        {
            if (gameManager == null)
            {
                gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            }

            if (settingsPanel == null)
            {
                settingsPanel = this.gameObject.transform;
            }


            settingsOpen = false;
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
                if (settingsOpen == true)
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
            settingsOpen = true;

            //Freeze time.
            Time.timeScale = 0f;
        }

        //Close Settings.
        void SettingsClose()
        {
            //Minimize Settings.
            settingsPanel.localScale = closedScale;
            settingsOpen = false;

            //Unfreeze time.
            Time.timeScale = 1f;
        }

        //stick multiplayer on and restart game.
        public void TwoPlayers()
        {
            GameAssets.singlePlayer = false;
            RestartLevel();
        }

        //stick singlePlayer on and restart game.
        public void SinglePlayer()
        {
            GameAssets.singlePlayer = true;
            RestartLevel();
        }

        //Go fullscreen, cannot test if this works.
        public void Fullscreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }

        //Restart Game.
        public void RestartLevel()
        {
            //Close the settings menu if open.
            SettingsClose();
            gameManager.Restart();
        }
    }
}