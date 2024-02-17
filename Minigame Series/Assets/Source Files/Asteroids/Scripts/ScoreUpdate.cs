using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;

namespace Asteroids
{
    public class ScoreUpdate : MonoBehaviour
    {
        [Header("Updates Score & Score Text")]
        [SerializeField] private TextMeshProUGUI scoreText;

        float tick = 0.1f; //time between recursion.

        private void Start()
        {
            StartCoroutine(UpdateScoreText());
        }

        //Just in case.
        private void OnDisable()
        {
            StopAllCoroutines();
        }

        // Recursion needed, but not every frame.
        // IEnumerator allows for minimal processing for recursion.
        IEnumerator UpdateScoreText()
        {
            //if game is playing according to player.
            if (GameAssets.gamePlaying)
            {
                UpdateTime();
                UpdateScore();
            }
            else
            {
                //Reset inGameTimer and Score.
                GameAssets.inGameTimer = 0;
                yield return new WaitUntil(() => GameAssets.gamePlaying == true);
                //reset score only after game starts again, this gives time for animations chance to look at score.
                GameAssets.score = 0;
            }

            //Do nothing when game is paused, this stops score going while in settings.
            while (Time.timeScale == 0f)
            {
                // Pause the coroutine while Time.timeScale is 0.
                yield return null;
            }

            //Set score text to current score.
            TextUpdate();

            //This stops Overflow error
            yield return new WaitForSeconds(tick);
            // Continue the recursion.
            StartCoroutine(UpdateScoreText());

            yield return null;
        }

        //Update the score based on the current game time.
        void UpdateScore()
        {
            GameAssets.score += (int)GameAssets.inGameTimer;
        }

        //Updates the game time.
        void UpdateTime()
        {
            GameAssets.inGameTimer += Time.deltaTime;
        }

        //Update the score text to add zeros to give it a game feel.
        void TextUpdate()
        {
            string newScore = "00000";
            string currentScore = GameAssets.score.ToString();

            if (GameAssets.score < 9 && GameAssets.score > 0)
            {
                newScore = "0000" + currentScore;
            }
            else if (GameAssets.score < 99 && GameAssets.score > 9)
            {
                newScore = "000" + currentScore;
            }
            else if (GameAssets.score < 999 && GameAssets.score > 99)
            {
                newScore = "00" + currentScore;
            }
            else if (GameAssets.score < 9999 && GameAssets.score > 999)
            {
                newScore = "0" + currentScore;
            }
            else if (GameAssets.score < 99999 && GameAssets.score > 9999)
            {
                newScore = GameAssets.score.ToString();
            }
            scoreText.text = newScore;
        }

    }

}