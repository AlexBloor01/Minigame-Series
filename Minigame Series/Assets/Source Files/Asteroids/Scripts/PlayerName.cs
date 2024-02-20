using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq.Expressions;

namespace Asteroids
{
    public class PlayerName : MonoBehaviour
    {

        private string playerName = "Not Applied"; //This will be changed based on number of players
        [SerializeField] private TextMeshPro nameText; //Text above players head in player prefab.
        [SerializeField] private Transform controllerPosition; //Assign the player to this
        private Vector3 distanceVector = new Vector3(0f, 0f, 0f); //This will be whatever position is set in the prefab player.


        private void Awake()
        {
            SetupPositioning();
        }

        //Disconnect from player and get position before game begins.
        void SetupPositioning()
        {
            distanceVector = transform.localPosition;
            AssignPlayerName();
        }

        //Setup the players names.
        void AssignPlayerName()
        {

            //If Singleplayer it does not matter too much.
            if (GameAssets.singlePlayer)
            {
                //Could allow players to enter names before game.
                playerName = $"Player";
            }
            else
            {
                //Set to the current number of players which is added by 1 before spawning each player.
                playerName = $"Player {GameAssets.numberOfPlayers + 1}";
            }

            //Assign playerName to the text above the player.
            nameText.text = playerName;
        }

        private void LateUpdate()
        {
            AssignPosition();
        }

        //Maybe use a delegate here instead of the switch wait for the flare thing
        void AssignPosition()
        {
            if (GameAssets.gamePlaying && controllerPosition)
            {
                //Assign to players position + the spawn position added on.
                gameObject.transform.position = controllerPosition.position + distanceVector;
            }
        }

    }
}