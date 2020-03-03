using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlowerScript : MonoBehaviour
{
  
    public bool blowingWind;
    public float windStrength;

    ParticleSystem windParticles;
    public float testPower;
    // Start is called before the first frame update
    void Start()
    {
        windParticles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //testPower = Mathf.Sin(Time.time/1.5f) + 1;
        testPower = GetComponent<MicInput>().MicLoudnessinDecibels;

        //TODO
        //REPLACE WITH MICROPHONE VOLUME
        if (testPower > -50)
        {
            blowingWind = true;
            windStrength = map(testPower, -100, 0, 0, 5);
        }
        else
        {
            blowingWind = false;
        }

        if (blowingWind)
        {
            //ENABLE THE PARTICLES
            if (!windParticles.isPlaying)
            {
                windParticles.Play();
            }


            //DYNAMICLY CHANGE THE SPEED AND COLOUR DEPENDING ON BREATH STRENGTH
            var main = windParticles.main;
            main.startSpeed = map(windStrength, 0, 5, 1, 7);
            main.startColor = new Color(255, 255, 255, map(windStrength, 0, 5, 0, .4f));


            //ADD FORCE TO NEARBY COLLIDERS
            Collider[] colliders = Physics.OverlapSphere(transform.position, 5);

            foreach(Collider coll in colliders)
            {
                //IF LAYER IS "WINDINTERACTABLE"
                if (coll.gameObject.layer == 10)
                {
                    var heading = coll.transform.position - transform.position;
                    float dot = Vector3.Dot(heading, transform.forward);


                    //IF THE COLLIDER IS IN FRONT OF THE WIND OBJ
                    if (dot > 0.75f)
                    {

                        Vector3 direction = coll.transform.position - transform.position;
                        //coll.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(direction.normalized, transform.position);
                        coll.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * map(Vector3.Distance(coll.transform.position,transform.position),0,5,3,0));

                    }
                }
            }

        }
        else
        {
            windParticles.Stop();
        }
    }

    // c#
    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }



}
