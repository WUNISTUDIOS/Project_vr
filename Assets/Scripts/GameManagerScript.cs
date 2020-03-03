using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
   
    public static GameManagerScript instance;
    public Transform mainCameraPosition;

    public GameObject[] skyboxVideoPlayerObjs;
    public Material[] skyboxMaterials;

    [Header("Clouds Scene")]
    public GameObject cloudsSpawnerPrefab;


    [Header("Bodyscan Scene")]
    public GameObject bodyScanCanvasPrefab;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject.Find("World 1 Glass").GetComponent<OrbWorldScript>().AttachOrbToHead_();

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject.Find("World 2 Glass").GetComponent<OrbWorldScript>().AttachOrbToHead_();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameObject.Find("World 3 Glass").GetComponent<OrbWorldScript>().AttachOrbToHead_();

        }
    }

    public IEnumerator BeginWorld(int worldIndex)
    {
        print("beginning world" + worldIndex);
        switch (worldIndex)
        {
            case 1:

                //START CLOUDS SCENE
                Instantiate(cloudsSpawnerPrefab, GameObject.Find("RightHand Controller").transform);

                Destroy(skyboxVideoPlayerObjs[2]);
                Destroy(skyboxVideoPlayerObjs[3]);

                break;
            case 2:

                Instantiate(bodyScanCanvasPrefab);

                Destroy(skyboxVideoPlayerObjs[1]);
                Destroy(skyboxVideoPlayerObjs[3]);

                break;
            case 3:


                Destroy(skyboxVideoPlayerObjs[2]);
                Destroy(skyboxVideoPlayerObjs[3]);

                break;
        }

        yield return new WaitForSeconds(1);
    }
}
