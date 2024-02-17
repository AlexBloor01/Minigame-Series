using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Pong
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("Manages the Score for both Players")]
        public TextMeshProUGUI text_LeftPlayerScore; //Text for score of left player.
        public TextMeshProUGUI text_RightPlayerScore; //Text for score of right player.

        public static int value_LeftPlayerScore; //Score of left player 
        public static int value_RightPlayerScore; //Score of right player 

        //Adds a point to the player who scored 
        public void PointGained(Owner goalOwner)
        {
            if (goalOwner == Owner.Right_Player)
            {
                value_LeftPlayerScore++;
                Debug.Log("Left Player Scored");
            }
            else
            {
                value_RightPlayerScore++;
                Debug.Log("Right Player Scored");
            }

            //update text for players score
            UpdateScoreText();
        }

        //resets score to 0.
        public void ResetScore()
        {
            value_LeftPlayerScore = 0;
            value_RightPlayerScore = 0;
            UpdateScoreText();
        }

        //Update each players score text to current score values.
        public void UpdateScoreText()
        {
            text_LeftPlayerScore.text = value_LeftPlayerScore.ToString();
            text_RightPlayerScore.text = value_RightPlayerScore.ToString();
        }

    }

    //Owners of Goals
    public enum Owner
    {
        Left_Player,//Left
        Right_Player //Right
    }
}

