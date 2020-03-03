using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawnerScript : MonoBehaviour
{ public GameObject flower;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 50; i++)
        {
            Instantiate(flower, new Vector3(Random.Range(-25, 25), 0, Random.Range(-25, 25)), transform.rotation);
            
        }
    }

    
}
