using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{

    private float startPosX;
    private float startPosY;
    private bool isBeingHeld = false;


    private void OnMouseDown()
    {
        //0 is left mouse button, 1 is right mouse button
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mousePose;
            mousePose = Input.mousePosition;
            mousePose = Camera.main.ScreenToWorldPoint(mousePose);

            startPosX = mousePose.x - this.transform.localPosition.x;
            startPosY = mousePose.y - this.transform.localPosition.y;

            isBeingHeld = true;
        }


        /*if (Input.GetMouseButtonDown(0))
        {
            this.transform.Rotate(0.0f, 0.0f, 90.0f, Space.World);
            
        }*/
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            this.transform.Rotate(0.0f, 0.0f, -90.0f, Space.World);
            Debug.Log("Has entered object");

        }
    }




    private void OnMouseUp()
    {
        isBeingHeld = false;
    }







    void Update()
    {
        if(isBeingHeld == true)
        {
            Vector3 mousePose;
            mousePose = Input.mousePosition;
            mousePose = Camera.main.ScreenToWorldPoint(mousePose);

            this.gameObject.transform.localPosition = new Vector3(mousePose.x - startPosX, mousePose.y - startPosY, 0);
        }
    }
}
