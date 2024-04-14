using System.Collections;
using UnityEngine;
using System;

public class MovementLibrary : MonoBehaviour
{

    public static IEnumerator USScaleLerp(GameObject obj, float duration, Vector3 startScale, Vector3 endScale)
    {
        float elapsedTime = 0;
        while (elapsedTime <= duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float percentage = elapsedTime / duration;
            obj.transform.localScale = Vector3.Lerp(startScale, endScale, percentage);
            yield return null;
        }
        yield return null;
    }

    public static IEnumerator ScaleLerp(GameObject obj, float duration, Vector3 startScale, Vector3 endScale)
    {
        float elapsedTime = 0;
        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / duration;
            obj.transform.localScale = Vector3.Lerp(startScale, endScale, percentage);
            yield return null;
        }
        yield return null;
    }

}