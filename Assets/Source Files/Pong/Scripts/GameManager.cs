using System.Collections;
// using System.Collections.Generic;
using UnityEngine;



namespace Pong
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Manager")]
        //Sets game settings from the start of the game launch.
        //Game does not need a manager but good to set singleplayer on start.
        public PauseMenu pauseMenu;

        // int frameRate = 12; //just a bit of fun messing with the frame rates.
        private void Awake()
        {
            //Messing around with the fram rate, game is more challenging the less frames, due to not using physics.
            Application.targetFrameRate = -1; //default value is -1.

            //Start in SinglePlayer, it is all already setup for singleplayer but this just garentees.
            pauseMenu.SinglePlayer();
        }
    }
}


