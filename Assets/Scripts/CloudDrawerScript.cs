using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CloudDrawerScript : MonoBehaviour
{
    List<InputDevice> foundLeftControllers = new List<InputDevice>();

    public ParticleSystem CloudsParticleSystem;

    public AudioSource cloudsSpawningSource;
    void Start()
    {
        //MAKE A LIST OF REQUIREMENTS THAT THE CONTROLLER WE ARE LOOKING FOR MUST HAVE
        InputDeviceCharacteristics leftTrackedControllerFilter = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.Left, leftHandedControllers;

        //SEARCH ALL CONNECTED DEVICES FOR A DEVICE THAT MATCES THE ABOVE CHARACTERISTICS, AND STORE IT IN THE "foundLeftControllers" LIST
        InputDevices.GetDevicesWithCharacteristics(leftTrackedControllerFilter, foundLeftControllers);

        print("Found " + foundLeftControllers.Count + " Left handed controller (s)");


    }

   
    void Update()
    {

        //CHECK IF TRIGGER IS PRESSED
        bool triggerValue;

        if (foundLeftControllers.Count > 0)
        {
            if (foundLeftControllers[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton,
                                          out triggerValue)
                && triggerValue)
            {
                Debug.Log("Trigger button is pressed");


                var emmision = CloudsParticleSystem.emission;

                //START THE CLOUD PARTICLES
                if (emmision.enabled == false)
                {

                    emmision.enabled = true;
                }

                //START THE CLOUDS SPAWNING AUDIO SOURCE
                if (!cloudsSpawningSource.isPlaying)
                {
                    cloudsSpawningSource.Play();
                }
            }
            else
            {
                Debug.Log("Trigger button was let go");


                var emmision = CloudsParticleSystem.emission;

                if (emmision.enabled == true)
                {
                    emmision.enabled = false;
                }

                //FADE THE CLODUS SPAWNING AUDIO SOURCE OUT
                if (cloudsSpawningSource.isPlaying)
                {
                    StartCoroutine(fadeAudio(cloudsSpawningSource, false));
                }
            }
        }
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
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.06f);

            sourceToFade.Stop();
            sourceToFade.volume = 1;
        }

    }
}
