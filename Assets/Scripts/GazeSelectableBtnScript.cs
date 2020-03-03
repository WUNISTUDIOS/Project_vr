using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazeSelectableBtnScript : MonoBehaviour
{


    /* This script should be put on every button that we want to be selectable through gazing
     * It recieves orders from the script thats on the camera | 0_0 - Btn Highlighted |  )_) - Btn Unhighlighted  |
     * 
     * 
     */  
    Animator anm;

    enum BtnState { Idle, Highlighted, Pressed};

    BtnState currentState = BtnState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        anm = GetComponent<Animator>();

    }
    private void Update()
    {
        transform.GetChild(0).gameObject.GetComponent<Text>().text = currentState.ToString();
    }
    //CALLED WHEN BUTTON IS LOOKED AT
    public void Highlighted()
    {
        if (currentState == BtnState.Idle)
        {
            currentState = BtnState.Highlighted;

            anm.Play("Highlighted");


            //START TIMER
            StartCoroutine("WaitAndPress");

        }

    }

    //IF THEY LOOK AWAY FROM THE BUTTON
    public void UnHighlighted()
    {
        if (currentState == BtnState.Highlighted)
        {
            currentState = BtnState.Idle;

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
        currentState = BtnState.Pressed;

        yield return new WaitForSeconds(0.65f);

        currentState = BtnState.Idle;

    }


}
