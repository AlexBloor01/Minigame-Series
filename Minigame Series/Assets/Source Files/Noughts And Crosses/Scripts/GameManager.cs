// using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace NoughtsAndCrosses
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Manager")]
        [SerializeField] private Tile currentPlayer; //Top left tile to indicate the current player.
        [SerializeField] private Tile tile; //Tile tile script to instantiate.
        private Tile[,] tileGrid; //Tile tile script grid matrix.
        private IntVector2 iSize = new IntVector2(3, 3); //Size of play area grid.
        [SerializeField] public static SquareOption currentTurn = SquareOption.Cross; //Whos turn is it?
        [SerializeField] private GameObject winner; //Turned on and off for win condition.
        Vector3 middle = new(); //Used to assign middle of screen for play area.

        //Keys
        readonly KeyCode spaceKey = KeyCode.Space;

        private void Start()
        {
            //Starts game fresh.
            RestartMatch();
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
                RestartMatch();
            }
        }

        private void RestartMatch()
        {
            //Set winner to none.
            winner.SetActive(false);

            //Losing player starts first.
            NextTurn();

            //Setup new grid.
            SetNewGrid();

            //Reset position and scale variables ready for conversion to canvas UI.
            MiddleScreenVariableReset();

            //Fill the grid with tile and their positions.
            FillNewGrid();

            //Setup the grid to the middle of the screen
            SetupGridPositioning();
        }

        //Setup new Grid.
        void SetNewGrid()
        {
            //clear previous tileGrid
            if (tileGrid != null)
            {
                foreach (Tile tile in tileGrid)
                {
                    Destroy(tile.gameObject);
                }
            }
            tileGrid = null;
            tileGrid = new Tile[iSize.x, iSize.y];
        }

        //Creates new grid
        void FillNewGrid()
        {
            //Instantiate new grid
            for (int x = 0; x < tileGrid.GetLength(0); x++)
            {
                for (int y = 0; y < tileGrid.GetLength(1); y++)
                {
                    Tile newSquare = Instantiate(tile, new Vector2(x, y), Quaternion.identity);

                    //Set Coordinates of this the sqaures location to reference later.
                    newSquare.iCoordinates = new IntVector2(x, y);

                    //Setting localScale to anything lower than 1f will create grid look.
                    newSquare.transform.localScale = tile.transform.localScale;

                    //Name for simplicity and debugging.
                    newSquare.name = $"Sqaure {x} || {y}";

                    //Assign Game Manager to tile to minimize processing.
                    newSquare.gameManager = this;

                    //Quick implementation of middle tile for positioning this object for center screen.
                    if (x == 1 && y == 1)
                    {
                        middle = newSquare.transform.position;
                    }

                    //Assign to grid point, based on its location.
                    tileGrid[x, y] = newSquare;
                }
            }
        }

        void MiddleScreenVariableReset()
        {
            //Set position to middle of screen and reset scale for UI conversion.
            transform.position = new Vector3();
            transform.localScale = new Vector3(1f, 1f, 1f);

            //Setup middle point to position to later.
            middle = new Vector3();

            //Set the size to an odd number, otherwise game can not be cetered correctly at this moment. 
            iSize = ConvertToOddNumbers(iSize);//CHANGE THIS ONCE AUTOMATION ADDED

        }

        void SetupGridPositioning()
        {
            //Assign the position to the middle of screen.
            transform.position = middle;

            foreach (Tile tile in tileGrid)
            {
                tile.transform.SetParent(transform);
                tile.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            transform.localScale = new Vector3(2.75f, 2.75f, 2.75f);
            transform.position = new Vector3(0f, 0.45f, 0f);
        }

        //Converts both x and y of IntVector2 to odd numbers.
        IntVector2 ConvertToOddNumbers(IntVector2 size)
        {
            size.x = Mathf.Clamp(size.x, 3, 11);
            size.y = Mathf.Clamp(size.y, 3, 11);

            if (size.x % 2 == 0)
            {
                size.x += 1;
            }

            if (size.y % 2 == 0)
            {
                size.y += 1;
            }

            return size;
        }

        //Accessed by individual squares to check win conditions and assign tile on grid variable.
        public void SwitchSquare(IntVector2 iCoordinates)
        {
            //Update the visuals for the tile
            TileUpdate.TileImageUpdate(tileGrid[iCoordinates.x, iCoordinates.y]);

            //Check if currentturn player has won.
            CheckWinCondition();
        }

        //Checks each tile win condition.
        void CheckWinCondition()
        {
            //These win conditions will work for a 3x3 grid but will  require automation for larger grids.
            //This was a quick implementation method.
            #region Manually Entered Win Conditions
            if (tileGrid[0, 0].squareState == currentTurn && tileGrid[0, 1].squareState == currentTurn && tileGrid[0, 2].squareState == currentTurn)
            {
                Win();
            }
            if (tileGrid[1, 0].squareState == currentTurn && tileGrid[1, 1].squareState == currentTurn && tileGrid[1, 2].squareState == currentTurn)
            {
                Win();
            }
            if (tileGrid[2, 0].squareState == currentTurn && tileGrid[2, 1].squareState == currentTurn && tileGrid[2, 2].squareState == currentTurn)
            {
                Win();
            }

            if (tileGrid[0, 0].squareState == currentTurn && tileGrid[1, 0].squareState == currentTurn && tileGrid[2, 0].squareState == currentTurn)
            {
                Win();
            }
            if (tileGrid[0, 1].squareState == currentTurn && tileGrid[1, 1].squareState == currentTurn && tileGrid[2, 1].squareState == currentTurn)
            {
                Win();
            }
            if (tileGrid[0, 2].squareState == currentTurn && tileGrid[1, 2].squareState == currentTurn && tileGrid[2, 2].squareState == currentTurn)
            {
                Win();
            }

            if (tileGrid[0, 0].squareState == currentTurn && tileGrid[1, 1].squareState == currentTurn && tileGrid[2, 2].squareState == currentTurn)
            {
                Win();
            }

            if (tileGrid[0, 2].squareState == currentTurn && tileGrid[1, 1].squareState == currentTurn && tileGrid[2, 0].squareState == currentTurn)
            {
                Win();
            }
            #endregion

            //If the win condition has not been played/set to active then count number of squares filled in.
            if (!winner.activeSelf)
            {
                int quickCount = 0;
                foreach (Tile tile in tileGrid)
                {
                    if (tile.squareState == SquareOption.Cross || tile.squareState == SquareOption.Naught)
                    {
                        quickCount++;
                    }
                }

                //If all squares are filled in make it a draw, else go to next players turn.
                if (quickCount >= (tileGrid.GetLength(0) * tileGrid.GetLength(1)))
                {
                    Draw(); //Draw game.
                }
                else
                {
                    NextTurn(); //Next players turn.
                }
            }
        }

        //Switch who's turn it is, from one to the other.
        void NextTurn()
        {
            //if not naught, its a cross.
            //Do not use None outside of unassigned tiles.
            if (currentTurn == SquareOption.Cross)
            {
                currentTurn = SquareOption.Naught;
            }
            else
            {
                currentTurn = SquareOption.Cross;
            }

            //Updates the top left tile to indicate the players turn has now been switched.
            TileUpdate.TileImageUpdate(currentPlayer);
        }

        void Win()
        {
            //Set Winner text to Win.
            winner.GetComponent<TextMeshProUGUI>().text = "Wins";
            WinConditionSet();
            //Confetti maybe?
        }

        void WinConditionSet()
        {
            //Disable all buttons.
            foreach (Tile tile in tileGrid)
            {
                tile.DisableThisButton();
            }
            //Reveal the winner of the game.
            winner.SetActive(true);
        }


        //Draw Game over condition.
        void Draw()
        {
            //Set Winner text to draw.
            winner.GetComponent<TextMeshProUGUI>().text = "Draw";
            WinConditionSet();
            //let down sound *crowd* awwwwww
        }

    }


    //Win Game over condition.

    //States of each tile.
    //None means not filled in. Do not use "None" outside of unassigned tiles.
    //Naught and Cross are each player.
    public enum SquareOption
    {
        None,
        Naught,
        Cross
    }
}