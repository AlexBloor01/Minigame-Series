using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static AudioManager iAudio;
    public AudioSource audioSourceSFX; //Sound Effects audio source.
    public AudioSource audioSourceMusic; //Music audio source.

    [Header(" ")]
    [Header("Noughts and Crosses Section")]
    public AudioClip infinity; //Intro Infinity sound.

    [Header(" ")]
    [Header("General Section")]
    public AudioClip[] click; //Button Click.
    public AudioClip win; //Game Win.
    public AudioClip draw; //Game Draw.
    public AudioClip restart; //Game Restart.


    [Header(" ")]
    [Header("Music Section")]
    Coroutine nextSong;
    public AudioClip[] music; //Music tracks. Will not play any music if empty.
    public bool playMusic = true; //Do you want music? Will not play if off.
    public bool loopMusic = true; //Do you want the playlist to loop?
    public bool randomMusicOrder = false; //Do you want the music to play in a random order?
    public float currentTimeInSong; //What time are we in the current song?
    int currentSong = 0; //Current place in music playlist. 


    private void Awake()
    {
        SetupAudioManager();
    }

    //Sets up the audio manager to play music and assigns.
    void SetupAudioManager()
    {
        iAudio = this;
        PlayNextSong();
    }


    //Play current song in music playlist then iterate it for next iteration of PlayNextSong().
    public void PlayNextSong()
    {
        if (music.Length > 0 && playMusic)
        {
            if (randomMusicOrder)
            {
                RandomiseMusicOrder();
            }

            audioSourceMusic.clip = music[currentSong];
            audioSourceMusic.Play();

            //Check if we want to stop at the end of the playlist or play forever. 
            if (loopMusic)
            {
                nextSong = StartCoroutine(WaitUntilSongOver(music[currentSong], 0));
            }
            else
            {
                if (currentSong <= music.Length - 1)
                {
                    nextSong = StartCoroutine(WaitUntilSongOver(music[currentSong], 0));
                }
            }

            //Iterate to next song next play of this script.
            currentSong++;
            if (currentSong >= music.Length)
            {
                currentSong = 0;
            }
        }
    }

    //Waits until the song is over to play the next track. This is a recursive loop as long as music loop is true.
    IEnumerator WaitUntilSongOver(AudioClip song, float currentTime)
    {
        currentTimeInSong = currentTime;
        while (currentTimeInSong < song.length)
        {
            currentTimeInSong += Time.deltaTime;
            yield return null;
        }
        PlayNextSong();
    }

    //Randomly Sorts the music playlist.
    void RandomiseMusicOrder()
    {
        List<AudioClip> sortArray = new List<AudioClip>();
        sortArray = music.ToList();
        music = new AudioClip[sortArray.Count - 1];

        while (true)
        {
            int randomPoint = Random.Range(0, sortArray.Count);
            int randomSortPoint = Random.Range(0, sortArray.Count);
            if (music[randomPoint] == null && sortArray[randomSortPoint] != null)
            {
                music[randomPoint] = sortArray[randomSortPoint];
                sortArray.RemoveAt(randomSortPoint);
            }
            if (sortArray.Count == 0)
            {
                break;
            }
        }
    }

    //Plays a sound effects audio clip once.
    //Play this with AudioManager.iAudio.PlayClipOneShot(AudioManager.iAudio.Click[Random.Range(0, AudioManager.iAudio.Click.Length)]);
    //A bit long so usually you can reference this prior to the clip with AudioClip[] click = AudioManager.iAudio.Click. Up to you.
    public void PlayClipOneShot(AudioClip clip)
    {
        if (clip != null)
        {
            audioSourceSFX.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("Clip for PlayClipOneShot is Null");
        }

    }

    //Turns music on and off with toggle button in ui.
    public void MusicToggle(bool toggle)
    {
        audioSourceMusic.mute = !toggle;
        if (audioSourceMusic.mute == true)
        {
            StopCoroutine(nextSong); //Stop recursion.
        }
        else
        {
            PlayNextSong();
        }
    }

    //Mutes the SFX on and off with toggle button in ui.
    public void SFXToggle(bool toggle)
    {
        audioSourceSFX.mute = !toggle;
    }


}
