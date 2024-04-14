using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OverlapColour : MonoBehaviour
{
    Coroutine coroutine;
    public Color[] colours; //Range of colours you want to cycle through, I would suggest having colour 0 be clear white.
    public Image overlapImage; //Image that contains the colour to overlap the background.
    public float duration = 4.5f; //Duration for each lerp between colours to complete.
    float maxOpacity = 0.25f; //Maximum opacity allowed.

    // Start is called before the first frame update
    void Start()
    {
        StartColourLerping();
    }

    void StartColourLerping()
    {
        if (overlapImage == null)
        {
            overlapImage.GetComponent<Image>();
        }
        ClampAlpha();
        coroutine = StartCoroutine(ColourLerp());
    }

    //Stops colours from being too over saturated.
    void ClampAlpha()
    {
        for (int i = 0; i < colours.Length; i++)
        {
            Color colour = colours[i];
            Color opacityChecked = new Color(colour.r, colour.g, colour.b, Mathf.Clamp(colour.a, 0, maxOpacity));
            colours[i] = opacityChecked;
        }
    }

    //Lerp over each colour in the Colours array giving the background a disco effect.
    IEnumerator ColourLerp()
    {
        //This for loop will go forever until manually stopped by the toggle option.
        for (int currentColour = 0; currentColour < colours.Length; currentColour++)
        {
            int nextColour;
            if (currentColour + 1 < colours.Length)
            {
                nextColour = currentColour + 1;
            }
            else
            {
                nextColour = 0;
            }

            float timer = 0;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float percentage = timer / duration;
                overlapImage.color = Color.Lerp(colours[currentColour], colours[nextColour], percentage);
                yield return null;
            }

            if (currentColour >= colours.Length - 1) currentColour = -1;
        }
        yield return null;
    }

    //This is for the toggle in the UI, turning off and restarting the colour lerping.
    public void ToggleColourLerp(bool trigger)
    {
        if (trigger == false)
        {
            StopCoroutine(coroutine);
            coroutine = null;
            overlapImage.color = Color.clear;
        }
        else
        {
            coroutine = StartCoroutine(ColourLerp());
        }
    }
}
