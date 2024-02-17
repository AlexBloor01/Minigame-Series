// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NoughtsAndCrosses
{
    public static class TileUpdate
    {

        //Assigns a square to the current turn and assigns the appropriate image.
        public static void TileImageUpdate(Tile tileObj)
        {
            //Update the information to the current turn.
            tileObj.UpdateSquare(GameManager.currentTurn);

            //Get tiles Image to change it.
            Image thisImage = tileObj.GetComponent<Image>();

            //Assign image to tile.
            switch ((int)GameManager.currentTurn)
            {
                case 0:
                    thisImage.sprite = GameAssets.img_States[0];
                    break;
                case 1:
                    thisImage.sprite = GameAssets.img_States[1];
                    break;
                case 2:
                    thisImage.sprite = GameAssets.img_States[2];
                    break;

            }
        }
    }
}