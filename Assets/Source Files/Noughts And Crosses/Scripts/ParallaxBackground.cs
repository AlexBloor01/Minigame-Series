using System.Collections;
using System.Collections.Generic;

// using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;

namespace NoughtsAndCrosses
{
    public class ParallaxBackground : MonoBehaviour
    {
        public GameObject backgroundPrefab; //Prefab of the background asset.
        float duration = 20f; //What time frame do you want the effect to take over.
        int backgroundNumber = 5; //TThe higher this number the more detailed the effect. I think 5 works well.
        float spaceRotation = 0; //Curent rotation to rotate from.
        bool positiveRotation = true; //Rotating posivitely on the z or negatively.
        Vector3 largeScale = new Vector3(5, 5, 5); //Largest scale point of the animation. What Scale is needed to cover the whole screen?
        GameObject[] backgrounds;
        Coroutine[] coroutines;

        private void Start()
        {
            Parallax();
        }

        //For toggle in UI.
        public void ToggleParallax(bool trigger)
        {
            if (trigger)
            {
                Parallax();
            }
            else
            {
                for (int i = 0; i < backgrounds.Length; i++)
                {
                    //Turn off all coroutines and destroy all current backgrounds.
                    StopAllCoroutines();
                    Destroy(backgrounds[i]);
                }
            }
        }

        //Instntiates all of the objects in the correct order for the parallax effect.
        private void Parallax()
        {
            backgrounds = new GameObject[backgroundNumber];
            coroutines = new Coroutine[backgroundNumber];
            for (int i = 0; i < backgroundNumber; i++)
            {
                GameObject newBackground = Instantiate(backgroundPrefab, transform.position, Quaternion.identity);
                StartCoroutine(StarBackgroundLerp(newBackground, i));
                backgrounds[i] = newBackground;
            }
        }

        //Lerps the background scale and position at specific intervals before destroying object.
        //startLerpPos starts the illusion off immediately in the correct positions.
        private IEnumerator StarBackgroundLerp(GameObject obj, float startLerpPos)
        {
            //Control where the position along the lerp this object is at the start.
            float elapsedTime = 0;
            if (startLerpPos > 0)
            {
                elapsedTime = duration / backgroundNumber * startLerpPos; //Start the lerp position from startLerpPos out of 10 positions.
            }

            //Starting scale is 0. 
            Vector3 startScale = new Vector3();

            //Control position so they stay in the same area.
            Vector3 startPosition = this.transform.localPosition;
            Vector3 endPosition = startPosition + Vector3.forward * 5;

            //Rotate back and forward on the Rotation. This makes it look more like a tunnel.
            Quaternion startRotation = Quaternion.Euler(0, 0, spaceRotation);
            if (positiveRotation)
            {
                spaceRotation += Random.Range(20, 60f);
            }
            else
            {
                spaceRotation -= Random.Range(20, 60f);
            }
            Quaternion newRotation = Quaternion.Euler(0, 0, spaceRotation); //set new rotation.

            //Change direction when full rotation reached.
            if (spaceRotation > 360f)
            {
                positiveRotation = false;
            }
            else if (spaceRotation < -360f)
            {
                positiveRotation = true;
            }

            //Randomly Flip between all 4 positions of the sprite.
            obj.GetComponent<SpriteRenderer>().flipX = (Random.Range(0, 2) == 0);
            obj.GetComponent<SpriteRenderer>().flipY = (Random.Range(0, 2) == 0);

            while (elapsedTime <= duration)
            {
                elapsedTime += Time.deltaTime;
                float percentage = elapsedTime / duration;
                obj.transform.localScale = Vector3.Lerp(startScale, largeScale, percentage);
                obj.transform.position = Vector3.Lerp(startPosition, endPosition, percentage);
                obj.transform.rotation = Quaternion.Slerp(startRotation, newRotation, percentage);
                yield return null;
            }
            //Recursive loop.
            StartCoroutine(StarBackgroundLerp(obj, 0));
            yield return null;
        }
    }
}