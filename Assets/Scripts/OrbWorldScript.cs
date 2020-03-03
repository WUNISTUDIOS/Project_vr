using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;

public class OrbWorldScript : MonoBehaviour
{
    Animator anm;
    public AudioSource environmentAudio;
    public AudioSource voiceOverAudio;

    public bool orbIsBeingHeld;
    bool orbIsAttaching;

    enum OrbState { Idle, Highlighted, Pressed };

    OrbState currentState = OrbState.Idle;

    public Vector3 floatingPosition;
    public float enterOrbMinDistance;
    public float snapOrbDistance;
    public float orbReturnToFloatSpeed;

    public float TESTNUMBER;
    public int orbIndex;

    // Start is called before the first frame update
    void Start()
    {
        anm = transform.parent.GetComponent<Animator>();
        floatingPosition = new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z);
    }

    //CALLED WHEN BUTTON IS LOOKED AT
    public void Highlighted()
    {
        if (currentState == OrbState.Idle)
        {
            currentState = OrbState.Highlighted;

            anm.Play("Highlighted");


            //START TIMER
            StartCoroutine("WaitAndPress");

        }

    }

    //IF THEY LOOK AWAY FROM THE BUTTON
    public void UnHighlighted()
    {
        if (currentState == OrbState.Highlighted)
        {
            currentState = OrbState.Idle;

            StopCoroutine("WaitAndPress");

            anm.Play("Un-Highlighted");

        }

    }

    //WAIT A WHILE WHILE THEY HOLD THEIR GAZE, AND THEN PRESS
    public IEnumerator WaitAndPress()
    {

        yield return new WaitForSeconds(1);

        StartCoroutine("Pressed");


    }

    //CALLED TO PRESS BUTTON
    public IEnumerator Pressed()
    {
        anm.Play("Pressed");
        currentState = OrbState.Pressed;

        yield return new WaitForSeconds(0.65f);

        currentState = OrbState.Idle;

    }

    void FixedUpdate()
    {

        if (!orbIsBeingHeld && !orbIsAttaching)
        {
            //HOVER THE ORB UP AND DOWN
            if (transform.position.y < floatingPosition.y)
            {
                transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 0.05f);
            }

            if (transform.position.y >= floatingPosition.y)
            {
                transform.GetComponent<Rigidbody>().AddForce(Vector3.down * 0.05f);
            }

            //ALWAYS LERP THE ORB BACK TO ITS FLOATING POSITION
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, floatingPosition.x, orbReturnToFloatSpeed * Time.deltaTime), transform.position.y, Mathf.Lerp(transform.position.z, floatingPosition.z, orbReturnToFloatSpeed * Time.deltaTime)); 

        }

    }
    public void Grabbed()
    {
        orbIsBeingHeld = true;

        //PLAY THE VOICE OVER AUDIO AS A PREVIEW OF THE SCENE
        
        //voiceOverAudio.Play();

    }
    public void LetGo()
    {
        orbIsBeingHeld = false;

        //THE THE VOICE OVER AUDIO OUT
        StartCoroutine(fadeAudio(voiceOverAudio, false));

        print("trying to connect");
        print("Distance is " + Vector3.Distance(transform.position, GameManagerScript.instance.mainCameraPosition.position));
        //IF THE DISTANCE IS CLOSE ENOUGH ENTER THE WORLD
        if (Vector3.Distance(transform.position, GameManagerScript.instance.mainCameraPosition.position) < enterOrbMinDistance)
        {
            AttachOrbToHead_();
        }
    }
    //Helper function for calling from gammanager
    public void AttachOrbToHead_()
    {
        if (!orbIsAttaching)
        {
            orbIsAttaching = true;
            StartCoroutine("AttachOrbToHeadSecondOption");

        }

    }

    public IEnumerator AttachOrbToHead()
    {

        //Smoothly translate the position the rest of the way to the position of the camera
        for (float i = 0; i <= 1; i += 0.01f)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            //transform.position = new Vector3(Mathf.Lerp(transform.position.x, GameManagerScript.instance.mainCameraPosition.position.x, i), Mathf.Lerp(transform.position.y, GameManagerScript.instance.mainCameraPosition.position.y, i), Mathf.Lerp(transform.position.z, GameManagerScript.instance.mainCameraPosition.position.z, i));
            transform.position = Vector3.Lerp(transform.position, GameManagerScript.instance.mainCameraPosition.position, i);

            if (Vector3.Distance(transform.position, GameManagerScript.instance.mainCameraPosition.position) < 0.05f)
            {
                break;
            }
        }


        //Enable the skybox video player
        GameManagerScript.instance.skyboxVideoPlayerObjs[orbIndex].SetActive(true);

        //Set the correct skybox
        RenderSettings.skybox = GameManagerScript.instance.skyboxMaterials[orbIndex];

        //destroy everything on the "Lobby" layer
        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]; //will return an array of all GameObjects in the scene

        foreach (GameObject go in gos)
        {
            if (go.layer == 9)
            {
                if (go.name == this.gameObject.name)
                {
                    MeshRenderer[] mrs = go.GetComponentsInChildren<MeshRenderer>();
                    foreach( MeshRenderer mr in mrs)
                    {
                        mr.enabled = false;
                    }
                    Destroy(go, 5);
                }
                else
                {
                    Destroy(go.gameObject);
                }

            }
        }

        yield return new WaitForSeconds(1);

        //tell gamemanager to start the routine for that level
        StartCoroutine(GameManagerScript.instance.BeginWorld(orbIndex));
       
    }

    public IEnumerator AttachOrbToHeadSecondOption()
    {
        //STOP ANY LEFTOVER FLOATING FORCE
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        //GET THE CURRENT SPEED OF THE ORB
        float currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;

        //PLAY TRANSITION SOUND
        AudioManagerScript.instance.playOneShot(AudioManagerScript.instance.enterOrbSound);


        Vector3 pointJustPassedCamera = GameManagerScript.instance.mainCameraPosition.position + ((GameManagerScript.instance.mainCameraPosition.position - transform.position).normalized * TESTNUMBER);

        //SMOOTHLY TRANSLATE THE ORB THE REST OF THE WAY TO THE CENTER OF THE CAMERA
        while (Vector3.Distance(transform.position, GameManagerScript.instance.mainCameraPosition.position) > snapOrbDistance)
        {
            yield return new WaitForSecondsRealtime(0.001f);

            float step = 0.9f * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, pointJustPassedCamera, step);


        }

        print("-----------------DONE-----------------");

        //PARENT THE ORB TO THE CAMERA, AND SET SCALE TO 100 100 100
        this.transform.parent = Camera.main.gameObject.transform.parent;
        transform.localScale = new Vector3(100, 100, 100);

        //DESTROY EVERYTHING ON THE LOBBY LAYER EXCEPT FOR THIS OBJECT
        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]; //will return an array of all GameObjects in the scene

        foreach (GameObject go in gos)
        {
            if (go.layer == 9)
            {
                if (go.name != this.gameObject.name)
                {
                    Destroy(go.gameObject);
                }

            }
        }

        yield return new WaitForSeconds(1);

        //tell gamemanager to start the routine for that level
        StartCoroutine(GameManagerScript.instance.BeginWorld(orbIndex));

    }

    IEnumerator fadeAudio(AudioSource sourceToFade, bool inOrOut)
    {
        if (inOrOut)
        {

            sourceToFade.volume = 0;
            sourceToFade.Play();

            yield return new WaitForSeconds(0.5f);


            while (sourceToFade.volume < 1)
            {
                sourceToFade.volume += 0.05f;
                yield return new WaitForSeconds(0.1f);
            }

        }
        else
        {
            while (sourceToFade.volume > 0)
            {
                sourceToFade.volume -= 0.05f;
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0.5f);

            sourceToFade.Stop();
            sourceToFade.volume = 1;
        }
       
    }

}
