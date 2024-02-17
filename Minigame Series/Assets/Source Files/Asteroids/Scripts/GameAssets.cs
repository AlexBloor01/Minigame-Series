using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids
{
    public static class GameAssets
    {
        [Header("Store Game Assets and Values")]
        public static bool singlePlayer = true; //Check if we are playing with more than one player.
        public static bool gamePlaying = false; //Game Officially playing for player, stops and starts game, use Time.timeScale for pause.
        public static int score = 0; //game Score, gets reset on restart.
        public static int numberOfPlayers = 0; //If multiple players we need to know if both are dead or not.
        public static float inGameTimer = 0f; //In game Time, reset at game restart.
        public static readonly float edgeOfScreenX = 8.9f; //end of the x position of screen.
        public static readonly float edgeOfScreenY = 5f; //end of the y position of screen.
        private static readonly float t = 0.01f; //Used to add a bit of space between edge and teleport point to stop rapid teleporting.

        //Teleports objects to the opposite side of the screen.
        public static void ContainingArea(GameObject obj)
        {
            float newX = obj.transform.position.x;
            float newY = obj.transform.position.y;

            if (newX >= edgeOfScreenX)
            {
                newX = (edgeOfScreenX * -1) + t;
                Teleport(obj, newX, newY);
            }
            else if (newX <= -edgeOfScreenX)
            {
                newX = edgeOfScreenX - t;
                Teleport(obj, newX, newY);
            }

            if (newY >= edgeOfScreenY)
            {
                newY = (edgeOfScreenY * -1) + t;
                Teleport(obj, newX, newY);
            }
            else if (newY <= -edgeOfScreenY)
            {
                newY = edgeOfScreenY - t;
                Teleport(obj, newX, newY);
            }

        }

        //Teleport using transform and not rigidbody as rigidbody is more expensive and t will put position further away from edge.
        static void Teleport(GameObject obj, float newX, float newY)
        {
            obj.transform.position = new Vector2(newX, newY);
        }

        //Checks if a given position is within the game area.
        public static bool WithinArea(Vector2 position)
        {
            return position.x <= GameAssets.edgeOfScreenX && position.x >= -GameAssets.edgeOfScreenX && position.y <= GameAssets.edgeOfScreenY && position.y >= -GameAssets.edgeOfScreenY;
        }

    }

    //Used to differentiate players by side of keyboard.
    public enum PlayerSide
    {
        Left,
        Right
    }
}
