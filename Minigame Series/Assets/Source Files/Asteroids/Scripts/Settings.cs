using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Asteroids
{
    public class Settings : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private GameManager gameManager; //used for restart.
        [SerializeField] private Transform settingsPanel; //size the settings scale 1 1 1.
        bool settingsOpen = false; //check if settings is currently open.

        private void Awake()
        {
            //Check if gameManager is empty for some reason
            if (gameManager == null)
            {
                gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            //Escape to open settings menu.
            if (Input.GetKeyDown(KeyCode.Escape))
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
            settingsPanel.localScale = new Vector3(1f, 1f, 1f);
            settingsOpen = true;
            //Freeze time.
            Time.timeScale = 0f;
        }

        //Close Settings.
        void SettingsClose()
        {
            //Minimize Settings.
            settingsPanel.localScale = new Vector3();
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