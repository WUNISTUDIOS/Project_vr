using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public static AudioManagerScript instance;

    AudioSource mainOneShotSource;

    public AudioClip enterOrbSound;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        mainOneShotSource = GetComponent<AudioSource>();
    }

    public void playOneShot(AudioClip clipToPlay)
    {
        mainOneShotSource.PlayOneShot(clipToPlay);
    }
}
