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

        [Header("References")]
        [SerializeField] private GridNumbers gridNumbers; // Numbers Chnage Script.
        [SerializeField] private Tile currentPlayer; // Top left tile to indicate the current player.
        [SerializeField] private Tile tile; // Tile tile script to instantiate.
        [SerializeField] private GameObject winner; // Turned on and off for win condition.


        [Header("Game Grid Attributes")]
        private Tile[,] tileGrid; // Tile tile script grid matrix.
        private Vector2Int gridSize = new Vector2Int(3, 3); // Size of play area grid.
        [SerializeField] public static SquareOption currentTurn = SquareOption.Cross; // Whos turn is it?



        [Header("Physical Game Attributes")]
        [SerializeField] private RectTransform rt; // Get Game managers RectTransform.
        [SerializeField] private GridLayoutGroup gridLayout; // GridLayout should be on this Game Manager.
        Vector3 gridPosition = new Vector3(0f, 35f, 0f); // Where is the going to sit? Go just above middle to leave for text. 
        private float gridSlotSize = 0.95f; // Represents the percentage size of each space excluding space between, must be below 1f.
        private float winAnimationDelay = 0.2f; // Time delay for win animation, gives the animation a more natural look.

        [Header("Optimisation")]
        private int activeTiles; // how many tiles are currently active? This is used for optimisation.

        [Header("Minimal Matches Win")]
        private int minimalMatchesToWin = 1; // This is used by the diagonal but can also be used for optimisation. 
        private const int minimalMatchesToWinBarrier = 3; // This must be 3.

        [Header("Diagonal Win Varience")]
        private int diagonalWinVariance = 0; // How many nmbers down from minimumMatchesToWin does a diagonal require? This one is a bit hard to explain. However, it allows higher grids to allow for lower diagonal matches.
        readonly int varianceGridThreshhold = 2; // Where on the board do you want to come up to on the board (grid).
        readonly int defaultDiagonalWinVarienceApplied = 1; // This is the number of varience that is applied. The corner + 1 for minimalMatchesToWin which goes 1 down from the corners to account for none square grid games.
        readonly int defaultOddDiagonalWinVarienceIncrease = 1; // If its an odd number then how much does the diagonal increase by.


        [Header("Corner Coordinates")]
        // Where are the corners of the grid?
        private Vector2Int topLeftCorner;
        private Vector2Int bottomRightCorner;
        private Vector2Int topRightCorner;
        private Vector2Int bottomLeftCorner;


        #region Setup

        private void Start()
        {
            // Starts game fresh on default settings and 3x3 grid.
            RestartMatch();
        }

        public void RestartMatch()
        {
            // Set winner to none.
            winner.SetActive(false);

            // Losing player starts first.
            NextTurn();

            // Setup new grid.
            // Reset position and scale variables ready for conversion to canvas UI.
            SetNewGrid();

            // Fill the grid with tile and their positions.
            FillNewGrid();

            // Setup the grid to the middle of the screen
            SetupGridPositioningAndSizing();
        }

        // Setup new Grid.
        void SetNewGrid()
        {
            // Set position to middle of screen and reset scale for UI conversion.
            transform.position = new Vector3();
            transform.localScale = new Vector3(1f, 1f, 1f);

            // Assign the new grid based on User input.
            gridSize = gridNumbers.GridSize();

            // clear previous tileGrid
            if (tileGrid != null)
            {
                foreach (Tile tile in tileGrid)
                {
                    Destroy(tile.gameObject);
                }
            }
            tileGrid = null;
            tileGrid = new Tile[gridSize.x, gridSize.y];

            AssignCornerPlacements();
            SetupDiagonalWinVariance();

            // Reset Active Tiles.
            activeTiles = 0;
        }

        private void AssignCornerPlacements()
        {
            topLeftCorner = new Vector2Int();
            bottomRightCorner = new Vector2Int(gridSize.x - 1, gridSize.y - 1);
            topRightCorner = new Vector2Int(0, gridSize.y - 1);
            bottomLeftCorner = new Vector2Int(gridSize.x - 1, 0);
        }

        private void SetupDiagonalWinVariance()
        {
            // Calculate minimal number of positions required to get.
            minimalMatchesToWin = (gridSize.x <= gridSize.y) ? gridSize.x : gridSize.y;

            if (minimalMatchesToWin < minimalMatchesToWinBarrier)
                return;

            minimalMatchesToWin--;

            if (gridSize.x != gridSize.y || minimalMatchesToWin < minimalMatchesToWinBarrier)
            {
                diagonalWinVariance = 0;
                return;
            }

            diagonalWinVariance = Mathf.CeilToInt((gridSize.x / varianceGridThreshhold) - defaultDiagonalWinVarienceApplied);

            // Odd numbers need an extra diagonalWinVariance to make them more fair games.
            if (gridSize.x % 2 != 0)
                diagonalWinVariance += defaultOddDiagonalWinVarienceIncrease;
        }

        // Creates a new grid.
        void FillNewGrid()
        {
            // Instantiate new grid
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    // Create new Tile.
                    Tile newTile = Instantiate(tile, new Vector2(x, y), Quaternion.identity);

                    // Set Coordinates of this the sqaures location to reference later.
                    newTile.tileCoordinates = new Vector2Int(x, y);

                    // Setting localScale to anything lower than 1f will create grid look.
                    newTile.transform.localScale = tile.transform.localScale;

                    // Name for simplicity and debugging.
                    newTile.name = $"Sqaure {x} || {y}";

                    // Assign Game Manager to tile to minimize processing.
                    newTile.gameManager = this;

                    // Assign to grid point, based on its location.
                    tileGrid[x, y] = newTile;
                }
            }
        }

        void SetupGridPositioningAndSizing()
        {
            // Check if rt is actually been assigned.
            if (GetComponent<RectTransform>() != null && rt == null)
            {
                rt = GetComponent<RectTransform>();
            }

            // Assign to the game manager grid and set to 1f localScale.
            foreach (Tile tile in tileGrid)
            {
                tile.transform.SetParent(rt);
                tile.transform.localScale = new Vector3(1f, 1f, 1f);
            }

            // Check if gridLayout is actually been assigned.
            if (gridLayout == null)
            {
                gridLayout = GetComponent<GridLayoutGroup>();
            }

            // Get the width and height of the Game Manager RectTransform.
            float width = rt.rect.width;
            float height = rt.rect.height;

            // Stop messes with changes to gridSlotSize, it cannot go over 1f without issues.
            gridSlotSize = Mathf.Clamp(gridSlotSize, 0.1f, 0.99f);

            // Create the size of each tile in relation to the available size.
            float cellSize = width / gridSize.x * gridSlotSize;

            // The Remaining amount of gridSlotSize will be put into spacing, to create an even looking field of tiles.
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

            // Assign cellSize and cellSSpacing to the gridLayout on Game Manager.
            gridLayout.cellSize = new Vector2(cellSize, cellSize);
            gridLayout.spacing = new Vector2(cellSSpacing, cellSSpacing);

            // Constrain to the x axis, visually if you want to reverse to y axis use FixedColumnCount and instantiate in FillNewGrid() x then y.
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            //  gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = gridSize.x;

            // Set the RectTransform position to the chosen center of the screen.
            rt.localPosition = gridPosition;
        }

        #endregion

        #region Gameplay
        // Accessed by individual squares to check win conditions and assign tile on grid variable.
        public void SwitchTile(Vector2Int tilePosition)
        {
            // New tile has been activated.
            activeTiles++;

            // Update the visuals for the tile
            TileUpdate.TileImageUpdate(tileGrid[tilePosition.x, tilePosition.y]);

            // Check if current turn player has won.
            CheckWinCondition(tilePosition);
        }

        // Checks each tile win condition.
        void CheckWinCondition(Vector2Int tilePosition)
        {
            // Check each possible win combination with current points active for a win condition.
            CheckWin(tilePosition);
            // Check if it is a draw or next turn.
            CheckDraw();
        }

        void CheckWin(Vector2Int tilePosition)
        {
            // If the minimum for a win condition has not been met, do not check for a win condition.
            if (minimalMatchesToWin <= activeTiles)
            {
                CheckStraightDirection(tilePosition);
                CheckDiagonal(tilePosition);
            }
        }

        void CheckStraightDirection(Vector2Int tilePosition)
        {
            int checkX = 0;
            int checkY = 0;

            List<Tile> xWinTiles = new List<Tile>();
            List<Tile> yWinTiles = new List<Tile>();

            // Check Horizontal Direction for Win.
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

            // Check Vertical Direction for Win.
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

        void CheckDiagonal(Vector2Int tilePosition)
        {
            List<Tile> LeftToRightTiles = new List<Tile>(); // top left to bottom right tiles.
            List<Tile> RightToLeftTiles = new List<Tile>(); // top right to bottom left tiles.

            for (int x = -gridSize.x; x < gridSize.x; x++)
            {
                for (int y = -gridSize.y; y < gridSize.y; y++)
                {
                    Vector2Int newCoordinate = new Vector2Int(tilePosition.x + x, tilePosition.y + y);
                    if (ContainingCoordinates(newCoordinate))
                    {
                        if (x == y)
                        {
                            if (tileGrid[tilePosition.x + x, tilePosition.y + y].state == currentTurn)
                            {
                                LeftToRightTiles.Add(tileGrid[tilePosition.x + x, tilePosition.y + y]);
                            }
                        }
                    }

                    if (ContainingCoordinates(newCoordinate))
                    {
                        if (x + y == 0)
                        {
                            if (tileGrid[tilePosition.x + x, tilePosition.y + y].state == currentTurn)
                            {
                                RightToLeftTiles.Add(tileGrid[tilePosition.x + x, tilePosition.y + y]);
                            }
                        }
                    }
                }
            }

            CheckDiagonalWinCondition(LeftToRightTiles, true);
            CheckDiagonalWinCondition(RightToLeftTiles, false);
        }

        private void CheckDiagonalWinCondition(List<Tile> tiles, bool leftToRightTiles)
        {
            //  Check asending win, top left to bottom right of the grid.
            if (tiles.Count <= minimalMatchesToWin - diagonalWinVariance)
                return;

            // If the grid is too small, it does not need to check if a diagonal is correct.
            if (diagonalWinVariance < 1)
            {
                Win(tiles);
                return;
            }

            // Is this an none square, then skip the rest of this.
            if (gridSize.x != gridSize.y)
            {
                if (tiles.Count >= minimalMatchesToWin)
                {
                    Win(tiles);
                }
                return;
            }

            // Where is the starting corner for the given direction.
            Vector2Int topCorner = topLeftCorner;
            Vector2Int bottomCorner = bottomRightCorner;
            if (!leftToRightTiles)
            {
                topCorner = topRightCorner;
                bottomCorner = bottomLeftCorner;
            }

            Vector2Int topCornerOffset = new Vector2Int();
            Vector2Int bottomCornerOffset = new Vector2Int();

            for (int xy = -diagonalWinVariance; xy < diagonalWinVariance + 1; xy++)
            {

                if (xy >= 0)
                {
                    topCornerOffset = topCorner + (Directions.GetDirection(Direction.East) * xy);
                    bottomCornerOffset = bottomCorner + (Directions.GetDirection(Direction.North) * xy);
                }
                else
                {
                    topCornerOffset = topCorner + (Directions.GetDirection(Direction.South) * Mathf.Abs(xy));
                    bottomCornerOffset = bottomCorner + (Directions.GetDirection(Direction.West) * Mathf.Abs(xy));
                }


                if (tiles[0].tileCoordinates == topCornerOffset
              || tiles[tiles.Count - 1].tileCoordinates == bottomCornerOffset)
                {
                    for (int i = 1; i < tiles.Count; i++)
                    {
                        Vector2Int previousTile = tiles[i - 1].tileCoordinates;
                        Vector2Int currentTile = tiles[i].tileCoordinates;
                        Vector2Int direction = Directions.GetDirection(Direction.NorthWest);

                        if (!leftToRightTiles)
                            direction = Directions.GetDirection(Direction.NorthEast);

                        //  Check if it is a full match between the corner points of the grid.
                        if (previousTile == currentTile + direction)
                        {
                            if (i >= minimalMatchesToWin - Mathf.Abs(xy))
                            {
                                Win(tiles);
                                return;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }




        bool ContainingCoordinates(Vector2Int coordinate)
        {
            return coordinate.x <= gridSize.x - 1 && coordinate.y <= gridSize.y - 1 && coordinate.x >= 0 && coordinate.y >= 0;
        }


        void CheckDraw()
        {
            // If a win has been attained CheckDraw will not trigger.
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

                // If all squares are filled in make it a draw, else go to next players turn.
                if (quickCount >= (tileGrid.GetLength(0) * tileGrid.GetLength(1)))
                {
                    Draw(); // Draw game.
                }
                else
                {
                    NextTurn(); // Next players turn.
                }
            }
        }

        // Switch who's turn it is, from one to the other.
        void NextTurn()
        {
            // if not naught, its a cross.
            // Do not use None outside of unassigned tiles.
            if (currentTurn == SquareOption.Cross)
            {
                currentTurn = SquareOption.Naught;
            }
            else
            {
                currentTurn = SquareOption.Cross;
            }

            // Updates the top left tile to indicate the players turn has now been switched.
            TileUpdate.TileImageUpdate(currentPlayer);
        }

        #endregion
        #region Win Conditions 
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

            // Set Winner text to Win.
            winner.GetComponent<TextMeshProUGUI>().text = "Wins";
            WinConditionSet();

            // Confetti maybe?

            // Play Win sound.
            AudioClip winSound = AudioManager.iAudio.win;
            AudioManager.iAudio.PlayClipOneShot(winSound);
        }

        void WinConditionSet()
        {
            // Disable all buttons.
            foreach (Tile tile in tileGrid)
            {
                tile.DisableThisButton();
            }
            // Reveal the winner of the game.
            winner.SetActive(true);
        }


        // Draw Game over condition.
        void Draw()
        {
            // Set Winner text to draw.
            winner.GetComponent<TextMeshProUGUI>().text = "Draw";
            WinConditionSet();
            // let down sound *crowd* awwwwww

            // Play Draw sound.
            AudioClip drawSound = AudioManager.iAudio.draw;
            AudioManager.iAudio.PlayClipOneShot(drawSound);
        }
        #endregion

    }

    // States of each tile.
    // None means not filled in. Do not use "None" outside of unassigned tiles.
    // Naught and Cross are each player.
    public enum SquareOption
    {
        None,
        Naught,
        Cross
    }
}