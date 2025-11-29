using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoughtsAndCrosses
{
    public class Intro : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            AudioClip infinity = AudioManager.iAudio.infinity;
            AudioManager.iAudio.PlayClipOneShot(infinity);
        }

    }

}

