using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGazeSelectionScript : MonoBehaviour
{


    /*

        So this script works by having this one variable "currentFocusedBtn" which is a refrence to a script from the buttons. 
        This variable is constantly updated to whatever button is in front of the camera. 

        Even when the camera turns to look at a new button, it still has the refrence to the last button it was looking at, which is told to become "Unselected"

        Whole thing works by raycasting out from the camera and looking for something with the tag of "GazeSelectableBtn"
      
     */


    OrbWorldScript currentFocusedBtn;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {

        RaycastHit hit;
        Vector3 startPoint = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);

        // RAYCAST HAS HIT SOMETHING
        if (Physics.Raycast(startPoint, transform.forward, out hit, Mathf.Infinity))
        {
            //AND ITS A BUTTON WE WANT TO BE INTERACTABLE WITH GAZE
            if (hit.collider.gameObject.tag == "GazeSelectable")
            {

                //IF WE ALREADY HAVE A BUTTON SCRIPT
                if (currentFocusedBtn)
                {

                    //AND ITS NOT THE ONE WE ARE CURRENTLY LOOKING AT
                    if (currentFocusedBtn.gameObject.GetInstanceID() != hit.collider.gameObject.GetInstanceID())
                    {
                        //SET THE CURRENT ONE TO BE THIS ONE AND HIGHLIGHT IT
                        //currentFocusedBtn = hit.collider.gameObject.GetComponent<GazeSelectableBtnScript>();

                        currentFocusedBtn = hit.collider.gameObject.GetComponent<OrbWorldScript>();

                    }

                }
                else //OTHERWISE OUR REFRENCE IS NULL SO WE SET IT INSTANTLY
                {

                    currentFocusedBtn = hit.collider.gameObject.GetComponent<OrbWorldScript>();

                }

                //TELL THE BUTTON TO HIGHLIGHT
                currentFocusedBtn.Highlighted();

            }
        }
        else
        {
            //ONCE THERE IS NOTHING BEING LOOKED AT

            //IF WE STILL HAVE A REFRENCE TO BUTTON
            if (currentFocusedBtn)
            {
                //UNHIGHLIGHT IT
                currentFocusedBtn.UnHighlighted();
            }

            //IGNORE (JUST DRAWS A LINE IN THE SCENE VIEW)
            Debug.DrawRay(startPoint, transform.forward * 1000, Color.white);

        }

        //IGNORE (JUST DRAWS A LINE IN THE SCENE VIEW)
        Debug.DrawRay(startPoint, transform.forward * hit.distance, Color.yellow);


    }

}
