using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NoughtsAndCrosses
{
    public class Tile : MonoBehaviour
    {
        [Header("Square Tile")]

        public Action clickAction;
        public GameManager gameManager; //used to access whole grid, check win condition, and apply visuals.
        public IntVector2 tileCoordinates; //This objects position in the grid for reference.
        public SquareOption state = SquareOption.None; //This tiles current state.

        private void Awake()
        {
            UpdateSquare(SquareOption.None); //set tile to none if not already.
        }

        //This Square tile button has been clicked.
        public void SquareClicked()
        {
            //Play Click Animation.
            if (GetComponent<TileAnimations>() != null)
            {
                StartCoroutine(GetComponent<TileAnimations>().ButtonClickAnimation());
            }

            //Get game manager.
            if (gameManager == null)
            {
                // gameManager = FindObjectOfType<GameManager>();
                gameManager = transform.parent.GetComponent<GameManager>();
            }

            //Play click sound.
            AudioClip[] clickSound = AudioManager.iAudio.click;
            AudioManager.iAudio.PlayClipOneShot(clickSound[UnityEngine.Random.Range(0, clickSound.Length)]);

            //Stop players from overwritting tiles after assignment
            DisableThisButton();

            //Switch the current state of the tile from None to Naught or Cross. 
            //Access the grid and other functions then come back here to UpdateSquare;
            gameManager.SwitchTile(tileCoordinates);
        }

        //Disable this button
        public void DisableThisButton()
        {
            //Access button in this script.
            GetComponent<Button>().interactable = false;
        }

        //Updates square tile, this is called in Game Manager to get the current turn then apply it.
        public void UpdateSquare(SquareOption currentState)
        {
            state = currentState;
        }


    }
}
