using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoughtsAndCrosses
{
    public class TileAnimations : MonoBehaviour
    {
        private float clickAnimSpeed = 3f; //Speed of click animation.
        private float winAnimSpeed = 2.5f; //Speed of win animation speed.
        private Vector3 largeScale = new Vector3(1.2f, 1.2f, 1.2f); //Smallest scale for button animation.
        private Vector3 smallScale = new Vector3(0.8f, 0.8f, 0.8f); //Largest scale for button animation.
        private float wiggleAnimDuration = 0.15f; //How long each position in wiggle anim to move to from the last.
        private float wigglePlusFactor = 4f; //What strength of rotation does each rotation have. The higher the larger rotations.


        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(RandomWiggleAnimation());
        }

        public IEnumerator ButtonClickAnimation()
        {
            transform.rotation = Quaternion.identity;
            StartCoroutine(MultiScaleLerp(clickAnimSpeed, new Vector3[] { Vector3.one, smallScale, Vector3.one }));

            yield return null;
        }

        public IEnumerator ButtonWinAnimation()
        {
            transform.rotation = Quaternion.identity;
            StartCoroutine(MultiScaleLerp(winAnimSpeed, new Vector3[] { transform.localScale, largeScale, smallScale, Vector3.one }));
            yield return null;
        }

        //Scales between different Vector3 rotation points.
        IEnumerator MultiScaleLerp(float duration, Vector3[] scale)
        {
            for (int i = 0; i < scale.Length - 1; i++)
            {
                Vector3 startPos = scale[i];
                Vector3 endPos = scale[i + 1];
                float journeyLength = Vector3.Distance(startPos, endPos);
                float startTime = Time.time;

                while (Time.time < startTime + (journeyLength / duration))
                {
                    float distanceCovered = (Time.time - startTime) * duration;
                    float fractionOfJourney = distanceCovered / journeyLength;
                    transform.localScale = Vector3.Lerp(startPos, endPos, fractionOfJourney);
                    yield return null;
                }
            }

            //Ensure the object ends up exactly at the last waypoint.
            transform.localScale = scale[scale.Length - 1];
        }

        //Wiggles the none filled in tile. 
        IEnumerator RandomWiggleAnimation()
        {
            //Create a random number of possible rotations.
            int numberOfRotations = Random.Range(2, 5);

            //Generate the rotation positions required to create the wiggle animation.
            List<Vector3> rotations = GenerateWiggleRotations(numberOfRotations);

            //Randomly pick a time to wiggle button again. 
            yield return new WaitForSeconds(Random.Range(2f, 40f));

            //Stop the rotation happening if the square has already been selected.
            if (GetComponent<Tile>() != null)
            {
                if (GetComponent<Tile>().state == SquareOption.None)
                {
                    //Play the wiggle animation and wait for it to be over.
                    StartCoroutine(MultiRotationLerp(wiggleAnimDuration, rotations.ToArray()));
                    yield return new WaitForSeconds(wiggleAnimDuration * numberOfRotations);
                    StartCoroutine(RandomWiggleAnimation());
                }
            }
            yield return null;
        }

        //Creates a list of vector3 rotations on the z axis.
        List<Vector3> GenerateWiggleRotations(int numberOfPositions)
        {
            List<Vector3> rotations = new List<Vector3>();

            for (int i = 0; i <= numberOfPositions; i++)
            {
                //Add this current rotation which is null so it will transition back to original rotation.
                if (i == 0 || i == numberOfPositions)
                {
                    rotations.Add(Vector3.zero);
                }

                float zRotation = 0;
                //Check if i is over half way through or not.
                if (i / 2 >= (numberOfPositions / 2))
                {
                    zRotation = (wigglePlusFactor - i) / i;
                }
                else
                {
                    zRotation = (wigglePlusFactor + i) * i;
                }

                //Add the rotation created and then reverse it for the reverse wiggle position.
                rotations.Add(new Vector3(0, 0, zRotation));
                rotations.Add(new Vector3(0, 0, -zRotation));
            }

            return rotations;
        }

        //Rotate between different Vector3 rotation points.
        IEnumerator MultiRotationLerp(float duration, Vector3[] rotations)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            for (int i = 0; i < rotations.Length - 1; i++)
            {
                Quaternion startRot = Quaternion.Euler(rotations[i]);
                Quaternion endRot = Quaternion.Euler(rotations[i + 1]);
                float startTime = Time.time;

                while (Time.time < startTime + duration)
                {
                    float distanceCovered = (Time.time - startTime) / duration;
                    rectTransform.rotation = Quaternion.Lerp(startRot, endRot, distanceCovered);
                    yield return null;
                }
            }

            // Ensure the object ends up exactly at the last position.
            rectTransform.rotation = Quaternion.Euler(rotations[rotations.Length - 1]);
            rectTransform.rotation = Quaternion.identity;
        }


    }
}