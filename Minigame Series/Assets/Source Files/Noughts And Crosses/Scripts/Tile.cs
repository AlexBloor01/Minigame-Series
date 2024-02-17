using UnityEngine;
using UnityEngine.UI;

namespace NoughtsAndCrosses
{
    public class Tile : MonoBehaviour
    {
        [Header("Square Tile")]
        public GameManager gameManager; //used to access whole grid, check win condition, and apply visuals.
        public IntVector2 iCoordinates; //This objects position in the grid for reference.
        public SquareOption squareState = SquareOption.None; //This tiles current state.

        private void Awake()
        {
            UpdateSquare(SquareOption.None); //set tile to none if not already.
        }

        //This Square tile button has been clicked.
        public void SquareClicked()
        {
            //Get game manager.
            if (gameManager == null)
            {
                gameManager = FindObjectOfType<GameManager>();
            }

            //Stop players from overwritting tiles after assignment
            DisableThisButton();

            //Switch the current state of the tile from None to Naught or Cross. 
            //Access the grid and other functions then come back here to UpdateSquare;
            gameManager.SwitchSquare(iCoordinates);
        }

        //Disable this button
        public void DisableThisButton()
        {
            //Gets button from the event system.
            //Leave this here for later, if need the code.
            // Button button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            // button.interactable = false;

            //Access button in this script.
            GetComponent<Button>().interactable = false;
        }

        //Updates square tile, this is called in Game Manager to get the current turn then apply it.
        public void UpdateSquare(SquareOption currentState)
        {
            squareState = currentState;
        }


    }
}
