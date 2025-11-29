using System.Collections;
using System.Collections.Generic;
using Asteroids;
using UnityEngine;

public class OverviewGameManager : MonoBehaviour
{

    //This script is for a later update.

    //Create a grid of games that you can play within a grid.
    public GameObject gameManagerPrefab;
    private GameManager[,] gameGrid;
    private IntVector2 gridSize = new IntVector2(3, 3); //Size of play area grid.

    private void Awake()
    {
        RestartGame();
    }

    void RestartGame()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject newGame = Instantiate(gameManagerPrefab);
                newGame.transform.parent = transform;
            }
        }


    }


}
