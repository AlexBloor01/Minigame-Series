using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

namespace NoughtsAndCrosses
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Manager")]

        [SerializeField] private GridNumbers gridNumbers; //Numbers Chnage Script.
        [SerializeField] private Tile currentPlayer; //Top left tile to indicate the current player.
        [SerializeField] private Tile tile; //Tile tile script to instantiate.
        private Tile[,] tileGrid; //Tile tile script grid matrix.
        private IntVector2 gridSize = new IntVector2(3, 3); //Size of play area grid.
        [SerializeField] public static SquareOption currentTurn = SquareOption.Cross; //Whos turn is it?
        [SerializeField] private GameObject winner; //Turned on and off for win condition.


        Vector3 gridPosition = new Vector3(0f, 35f, 0f); //Where is the going to sit? Go just above middle to leave for text. 
        [SerializeField] private GridLayoutGroup gridLayout; //GridLayout should be on this Game Manager.
        [SerializeField] private RectTransform rt; //Get Game managers RectTransform.

        float gridSlotSize = 0.95f; //Represents the percentage size of each space excluding space between, must be below 1f.
        int minimalMatchesToWin = 1; //This is used by the diagonal but can also be used for optimisation. 
        int activeTiles; //how many tiles are currently active? This is used for optimisation.
        float winAnimationDelay = 0.2f; //Time delay for win animation, gives the animation a more natural look.



        private void Start()
        {
            //Starts game fresh.
            RestartMatch();

            // FSLibrary.

            // float[] bubble = { 9, 2, 5, 7, 8, 5, 1 };
            // string bubblestring = "";

            // foreach (float num in bubble)
            // {
            //     bubblestring += num.ToString() + " ";
            // }
            // Debug.Log(bubblestring);
            // bubblestring = "";
            // Debug.Log("----------------------------");

            // bubble = BubbleSortFloatList(bubble.ToList()).ToArray();

            // foreach (float num in bubble)
            // {
            //     bubblestring += num.ToString() + " ";
            // }
            // Debug.Log(bubblestring);
        }



        public void RestartMatch()
        {
            //Set winner to none.
            winner.SetActive(false);

            //Losing player starts first.
            NextTurn();

            //Setup new grid.
            //Reset position and scale variables ready for conversion to canvas UI.
            SetNewGrid();

            //Fill the grid with tile and their positions.
            FillNewGrid();

            //Setup the grid to the middle of the screen
            SetupGridPositioningAndSizing();
        }

        //Setup new Grid.
        void SetNewGrid()
        {
            //Set position to middle of screen and reset scale for UI conversion.
            transform.position = new Vector3();
            transform.localScale = new Vector3(1f, 1f, 1f);

            //Assign the new grid based on User input.
            gridSize = gridNumbers.GridSize();

            //clear previous tileGrid
            if (tileGrid != null)
            {
                foreach (Tile tile in tileGrid)
                {
                    Destroy(tile.gameObject);
                }
            }
            tileGrid = null;
            tileGrid = new Tile[gridSize.x, gridSize.y];

            //Calculate minimal number of positions required to get.
            minimalMatchesToWin = (gridSize.x <= gridSize.y) ? gridSize.x : gridSize.y;
            if (minimalMatchesToWin != 3 && minimalMatchesToWin != 1 && minimalMatchesToWin % 2 != 0)
            {
                minimalMatchesToWin--;
            }

            //Reset Active Tiles.
            activeTiles = 0;
        }

        //Creates a new grid.
        void FillNewGrid()
        {
            //Instantiate new grid
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    //Create new Tile.
                    Tile newTile = Instantiate(tile, new Vector2(x, y), Quaternion.identity);

                    //Set Coordinates of this the sqaures location to reference later.
                    newTile.tileCoordinates = new IntVector2(x, y);

                    //Setting localScale to anything lower than 1f will create grid look.
                    newTile.transform.localScale = tile.transform.localScale;

                    //Name for simplicity and debugging.
                    newTile.name = $"Sqaure {x} || {y}";

                    //Assign Game Manager to tile to minimize processing.
                    newTile.gameManager = this;

                    //Assign to grid point, based on its location.
                    tileGrid[x, y] = newTile;
                }
            }
        }

        void SetupGridPositioningAndSizing()
        {
            //Check if rt is actually been assigned.
            if (GetComponent<RectTransform>() != null && rt == null)
            {
                rt = GetComponent<RectTransform>();
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
            float cellSize = width / gridSize.x * gridSlotSize;

            //The Remaining amount of gridSlotSize will be put into spacing, to create an even looking field of tiles.
            float cellSSpacing = width / gridSize.x * (1f - gridSlotSize);

            if (gridSize.x >= gridSize.y)
            {
                cellSize = width / gridSize.x * gridSlotSize;
                cellSSpacing = width / gridSize.x * (1f - gridSlotSize);
            }
            else
            {
                cellSize = width / gridSize.y * gridSlotSize;
                cellSSpacing = width / gridSize.y * (1f - gridSlotSize);
            }

            //Assign cellSize and cellSSpacing to the gridLayout on Game Manager.
            gridLayout.cellSize = new Vector2(cellSize, cellSize);
            gridLayout.spacing = new Vector2(cellSSpacing, cellSSpacing);

            //Constrain to the x axis, visually if you want to reverse to y axis use FixedColumnCount and instantiate in FillNewGrid() x then y.
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            // gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = gridSize.x;

            //Set the RectTransform position to the chosen center of the screen.
            rt.localPosition = gridPosition;
        }


        //Accessed by individual squares to check win conditions and assign tile on grid variable.
        public void SwitchTile(IntVector2 tilePosition)
        {
            //New tile has been activated.
            activeTiles++;

            //Update the visuals for the tile
            TileUpdate.TileImageUpdate(tileGrid[tilePosition.x, tilePosition.y]);

            //Check if current turn player has won.
            CheckWinCondition(tilePosition);
        }

        //Checks each tile win condition.
        void CheckWinCondition(IntVector2 tilePosition)
        {
            //Check each possible win combination with current points active for a win condition.
            // CheckDirections();
            CheckWin(tilePosition);


            //Check if it is a draw or next turn.
            CheckDraw();
        }

        void CheckStraightDirection(IntVector2 tilePosition)
        {
            int checkX = 0;
            int checkY = 0;

            List<Tile> xWinTiles = new List<Tile>();
            List<Tile> yWinTiles = new List<Tile>();

            //Check Horizontal Direction for Win.
            for (int x = 0; x < gridSize.x; x++)
            {
                if (tileGrid[x, tilePosition.y].state == currentTurn)
                {
                    checkX++;
                    xWinTiles.Add(tileGrid[x, tilePosition.y]);
                }
                else
                {
                    break;
                }
            }

            //Check Vertical Direction for Win.
            for (int y = 0; y < gridSize.y; y++)
            {
                if (tileGrid[tilePosition.x, y].state == currentTurn)
                {
                    checkY++;
                    yWinTiles.Add(tileGrid[tilePosition.x, y]);
                }
                else
                {
                    break;
                }
            }

            if (xWinTiles.Count >= gridSize.x)
            {
                Win(xWinTiles);
            }

            if (yWinTiles.Count >= gridSize.y)
            {
                Win(yWinTiles);
            }

        }

        void CheckWin(IntVector2 tilePosition)
        {
            //If the minimum for a win condition has not been met, do not check for a win condition.
            if (minimalMatchesToWin <= activeTiles)
            {
                CheckStraightDirection(tilePosition);
                DiaganolCheck(tilePosition);
            }
        }

        void DiaganolCheck(IntVector2 tilePosition)
        {
            int checkAscending = 0;
            int checkDescending = 0;

            List<Tile> ascendingWinTiles = new List<Tile>();
            List<Tile> descendingWinTiles = new List<Tile>();

            for (int x = -gridSize.x; x < gridSize.x; x++)
            {
                for (int y = -gridSize.y; y < gridSize.y; y++)
                {
                    if (ContainintCoordinates(new IntVector2(tilePosition.x + x, tilePosition.y + y)))
                    {
                        if (x == y)
                        {
                            if (tileGrid[tilePosition.x + x, tilePosition.y + y].state == currentTurn)
                            {
                                checkAscending++;
                                ascendingWinTiles.Add(tileGrid[tilePosition.x + x, tilePosition.y + y]);
                            }
                        }
                    }

                    if (ContainintCoordinates(new IntVector2(tilePosition.x + x, tilePosition.y + y)))
                    {
                        if (x + y == 0)
                        {
                            if (tileGrid[tilePosition.x + x, tilePosition.y + y].state == currentTurn)
                            {
                                checkDescending++;
                                descendingWinTiles.Add(tileGrid[tilePosition.x + x, tilePosition.y + y]);
                            }
                        }
                    }
                }
            }

            if (ascendingWinTiles.Count >= minimalMatchesToWin)
            {
                Win(ascendingWinTiles);
            }

            if (descendingWinTiles.Count >= minimalMatchesToWin)
            {
                Win(descendingWinTiles);
            }
        }

        bool ContainintCoordinates(IntVector2 coordinate)
        {
            return coordinate.x <= gridSize.x - 1 && coordinate.y <= gridSize.y - 1 && coordinate.x >= 0 && coordinate.y >= 0;
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

        void Win(List<Tile> winningTiles)
        {
            StartCoroutine(WinAnimation());

            IEnumerator WinAnimation()
            {
                foreach (Tile tile in winningTiles)
                {
                    if (tile.GetComponent<TileAnimations>() != null)
                    {
                        StartCoroutine(tile.GetComponent<TileAnimations>().ButtonWinAnimation());
                    }

                    yield return new WaitForSeconds(winAnimationDelay);
                }
                yield return null;
            }

            //Set Winner text to Win.
            winner.GetComponent<TextMeshProUGUI>().text = "Wins";
            WinConditionSet();

            //Confetti maybe?

            //Play Win sound.
            AudioClip winSound = AudioManager.iAudio.win;
            AudioManager.iAudio.PlayClipOneShot(winSound);
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

            //Play Draw sound.
            AudioClip drawSound = AudioManager.iAudio.draw;
            AudioManager.iAudio.PlayClipOneShot(drawSound);
        }


    }

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