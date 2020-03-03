using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerScript : MonoBehaviour
{
    [Header("100 - Random, 0/7 - Flowers")]

    public int flowerIndex; //0 through 6
    public AudioClip[] flowerBloomSounds;

    Animator anm;
    SpriteRenderer sr;
    AudioSource audioSrc;

    public List<Sprite[]> flowerSprites = new List<Sprite[]>();


    void Start()
    {
        //GET ANIMATOR
        anm = GetComponent<Animator>();
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        audioSrc = GetComponent<AudioSource>();
        //IF NO INDEX IS SPECIFIED THEN CHOOSE RANDOM
        if (flowerIndex == 100)
        {
            flowerIndex = Random.Range(0, 7);
        }


        //LOAD ALL THE FLOWER SPRITES
        for (int i = 0; i < 8; i++)
        {
            string path = "Flowers/FlowerSprites" + i;

            object[] loadedIcons = Resources.LoadAll(path, typeof (Sprite));


            flowerSprites.Add(new Sprite[loadedIcons.Length]);


            for (int j = 0; j < loadedIcons.Length; j++)
            {
                flowerSprites[i][j] = loadedIcons[j] as Sprite;
            }

        }

        //START WITH FIRST FRAME BEFORE WE BLOOM
        sr.sprite = flowerSprites[flowerIndex][0];

        Invoke("Bloom_", Random.Range(2, 15));
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }


    //HELPER FUNCTION
    public void Bloom_()
    {
        StartCoroutine("Bloom");
    }


    public IEnumerator Bloom()
    {

        //Choose a random Speed
        float speed = Random.Range(0.1f, 0.15f);

        audioSrc.PlayOneShot(flowerBloomSounds[Random.Range(0,flowerBloomSounds.Length)]);
        anm.Play("Bloom");

        //Play the sprite animation
        int i;
        i = 0;
        while (i < flowerSprites[flowerIndex].Length)
        {
            sr.sprite = flowerSprites[flowerIndex][i];
            //print("set frame to " + flowerSprites[flowerIndex][i]);

            i++;
            yield return new WaitForSeconds(speed);
            print("Waitintg");
            yield return 0;
        }

    }
}
