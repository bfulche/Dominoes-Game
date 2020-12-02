using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script helps with spawning dominoes. It also deals with picking up dominoes and moving them around with a mouse click

public class Game : MonoBehaviour
{
    public GameObject[,] positions = new GameObject[3, 2];
    private GameObject[] playerDominos = new GameObject[2];


    public GameObject domino1;
    public GameObject domino2;
    public GameObject domino3;

    //FromWord Game Drag and Drop website
    private bool draggingItem = false;
    private GameObject draggedObject;
    private Vector2 touchOffset;


    void Start()
    {
        Instantiate(domino1, new Vector3(-200, -160, -1), Quaternion.identity);
        Instantiate(domino2, new Vector3(0, -160, -1), Quaternion.identity);
        Instantiate(domino3, new Vector3(200, -160, -1), Quaternion.identity);

    }

    void Update()
    {
        if (HasInput)
        {
            DragOrPickUp();
        }
        else
        {
            if (draggingItem)
                DropItem();
        }
    }

    Vector2 CurrentTouchPosition
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }


    private void DragOrPickUp()
    {
        var inputPosition = CurrentTouchPosition;
        if (draggingItem)
        {
            draggedObject.transform.position = inputPosition + touchOffset;
        }
        else
        {
            RaycastHit2D[] touches = Physics2D.RaycastAll(inputPosition, inputPosition, 0.5f);
            if(touches.Length > 0)
            {
                var hit = touches[0];
                if (hit.transform != null && hit.transform.tag == "Tile")
                {
                    draggingItem = true;
                    draggedObject = hit.transform.gameObject;
                    touchOffset = (Vector2)hit.transform.position - inputPosition;
                    hit.transform.GetComponent<Tile>().PickUp();
                }
            }
        }
    }


    private bool HasInput
    {
        get
        {
            return Input.GetMouseButton(0);
        }
    }


    //DropItem function from Drag and Drop Word Game Website
    void DropItem()
    {
        draggingItem = false;
        draggedObject.transform.localScale = new Vector3(1, 1, 1);
        draggedObject.GetComponent<Tile>().Drop();
    }
}
