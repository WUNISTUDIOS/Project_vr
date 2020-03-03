using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BodyscanCanvasScript : MonoBehaviour
{
    public GameObject[] bodyscanOverlays;
    List<int> timeIntervals = new List<int>() {25, 60, 85, 20, 35, 35, 33, 32, 12, 40, 63, 55};

    // Start is called before the first frame update
    void Start()
    {

        GetComponent<Canvas>().worldCamera = GameManagerScript.instance.mainCameraPosition.GetComponent<Camera>();

        StartCoroutine("BodyScanRoutine");

    }

    public IEnumerator BodyScanRoutine()
    {
       
        for (int i = 0; i < timeIntervals.Count; i++)
        {
            if (i < bodyscanOverlays.Length)
            {

                //WAIT FOR THE NEXT TIME INTERVAL
                yield return new WaitForSecondsRealtime(timeIntervals[i]);

                if (i != 0)
                {
                    //STOP THE LAST OVERLAYs ANIMATION
                    bodyscanOverlays[i - 1].GetComponent<Animator>().SetBool("Exiting", true);

                }

                //PLAY THE ANIMATION FOR THE
                bodyscanOverlays[i].GetComponent<Animator>().Play("Enter");

            }

        }

    }
}
