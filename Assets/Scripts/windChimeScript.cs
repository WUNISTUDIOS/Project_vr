using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windChimeScript : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip chimeSound;

    bool canPlaySound = true;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canPlaySound)
        {
            canPlaySound = false;
            audioSource.PlayOneShot(chimeSound);
        }

        Invoke("ReenableDelay", 0.2f);
    }
    void ReenableDelay()
    {
        canPlaySound = true;
    }
}
