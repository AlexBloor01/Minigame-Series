// using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Collections;
using UnityEngine.UI;
using System.Drawing;

namespace NoughtsAndCrosses
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Manager")]

        [SerializeField] private GridNumbers gridNumbers; //Numbers Chnage Script.
        [SerializeField] private Tile currentPlayer; //Top left tile to indicate the current player.
        [SerializeField] private Tile tile; //Tile tile script to instantiate.
        private Tile[,] tileGrid; //Tile tile script grid matrix.
        private IntVector2 iSize = new IntVector2(3, 3); //Size of play area grid.
        [SerializeField] public static SquareOption currentTurn = SquareOption.Cross; //Whos turn is it?
        [SerializeField] private GameObject winner; //Turned on and off for win condition.


        Vector3 gridPosition = new Vector3(0f, 35f, 0f); //Where is the going to sit? Go just above middle to leave for text. 
        [SerializeField] private GridLayoutGroup gridLayout; //GridLayout should be on this Game Manager.
        [SerializeField] private RectTransform rt; //Get Game managers RectTransform.

        float gridSlotSize = 0.95f; //Represents the percentage size of each space excluding space between, must be below 1f.

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

        public void RestartMatch()
        {
            //Set winner to none.
            winner.SetActive(false);

            //Losing player starts first.
            NextTurn();

            //Reset position and scale variables ready for conversion to canvas UI.
            ScreenPositionVariableReset();

            //Setup new grid.
            SetNewGrid();

            //Fill the grid with tile and their positions.
            FillNewGrid();

            //Setup the grid to the middle of the screen
            SetupGridPositioningAndSizing();
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

                    //Assign to grid point, based on its location.
                    tileGrid[x, y] = newSquare;
                }
            }
        }

        void ScreenPositionVariableReset()
        {
            //Set position to middle of screen and reset scale for UI conversion.
            transform.position = new Vector3();
            transform.localScale = new Vector3(1f, 1f, 1f);

            //Assign the new grid based on User input.
            iSize = gridNumbers.GridSize();

        }

        void SetupGridPositioningAndSizing()
        {
            //Check if rt is actually been assigned.
            if (rt == null)
            {
                rt = this.GetComponent<RectTransform>();
            }

            //Assign to the game manager grid and set to 1f localScale.
            foreach (Tile tile in tileGrid)
            {
                tile.transform.SetParent(rt);
                tile.transform.localScale = new Vector3(1f, 1f, 1f);
            }

            //Check if gridLayout is actually been assigned.
            if (gridLayout == null)
            {
                gridLayout = GetComponent<GridLayoutGroup>();
            }

            //Get the width and height of the Game Manager RectTransform.
            float width = rt.rect.width;
            float height = rt.rect.height;

            //Stop messes with changes to gridSlotSize, it cannot go over 1f without issues.
            gridSlotSize = Mathf.Clamp(gridSlotSize, 0.1f, 0.99f);

            //Create the size of each tile in relation to the available size.
            float cellSize = (width / iSize.x) * gridSlotSize;

            //The Remaining amount of gridSlotSize will be put into spacing, to create an even looking field of tiles.
            float cellSSpacing = (width / iSize.x) * (1f - gridSlotSize);

            //Assign cellSize and cellSSpacing to the gridLayout on Game Manager.
            gridLayout.cellSize = new Vector2(cellSize, cellSize);
            gridLayout.spacing = new Vector2(cellSSpacing, cellSSpacing);

            //Set the RectTransform position to the chosen center of the screen.
            rt.localPosition = gridPosition;
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
            //Check each possible win combination with current points active for a win condition.
            CheckDirections();

            //Check if it is a draw or next turn.
            CheckDraw();
        }

        //Check for win condition in straight lines
        private bool CheckDirectionForWin(int startPoint, int endPoint, bool horizontal)
        {
            int match = 0;

            for (int i = 0; i < endPoint; i++)
            {
                //Check the Horizontal.
                if (horizontal)
                {
                    if (tileGrid[i, startPoint].state == currentTurn)
                    {
                        match++;
                    }
                }
                else //Check the Vertical.
                {
                    if (tileGrid[startPoint, i].state == currentTurn)
                    {
                        match++;
                    }
                }
            }

            return CheckWinList(match, endPoint);
        }

        //Confirm if CheckDirectionForWin returns as all true.
        bool CheckWinList(int matches, int length)
        {
            if (matches >= length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void CheckDirections()
        {
            //Check Horizontal Direction for Win.
            for (int x = 0; x < tileGrid.GetLength(0); x++)
            {
                if (CheckDirectionForWin(x, tileGrid.GetLength(1), true) == true)
                {
                    Win();
                }
            }

            //Check Vertical Direction for Win.
            for (int y = 0; y < tileGrid.GetLength(1); y++)
            {
                if (CheckDirectionForWin(y, tileGrid.GetLength(0), false) == true)
                {
                    Win();
                }
            }

            //Check the diagonals of the square grid.
            CheckDiagonal(true);
            CheckDiagonal(false);
        }

        void CheckDiagonal(bool Ascending)
        {
            int match = 0;

            //Check between i is lower than both grid lengths.
            for (int i = 0; i < tileGrid.GetLength(0) && i < tileGrid.GetLength(1); i++)
            {
                //Ascending Order
                if (Ascending)
                {
                    if (tileGrid[i, i].state == currentTurn)
                    {
                        match++;
                    }
                }
                else //Descending Order
                {
                    if (tileGrid[i, tileGrid.GetLength(1) - 1 - i].state == currentTurn)
                    {
                        match++;
                    }
                }
            }

            if (CheckWinList(match, tileGrid.GetLength(0)))
            {
                Win();
            }

        }

        void CheckDraw()
        {
            //If a win has been attained CheckDraw will not trigger.
            if (!winner.activeSelf)
            {
                int quickCount = 0;
                foreach (Tile tile in tileGrid)
                {
                    if (tile.state == SquareOption.Cross || tile.state == SquareOption.Naught)
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

        //Go fullscreen.
        public void Fullscreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
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