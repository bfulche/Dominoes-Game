using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool draggingItem = false;
    private GameObject draggedObject;
    private Vector2 touchOffset;

    [SerializeField]
    private GameObject[] BoardCellsArray;

    



    private void Update()
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

        PrintArrayInfo();
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
            if (touches.Length > 0)
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








    void DropItem()
    {
        draggingItem = false;
        draggedObject.transform.localScale = new Vector3(100, 100, 1);
        draggedObject.GetComponent<Tile>().Drop();
    }











    //Print list of game objects in cells and their z rotation value with button press
    void PrintArrayInfo()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach(GameObject BoardCell in BoardCellsArray)
            {
                if(BoardCell.transform.childCount > 0)   //check is BoardCell has a child
                {
                    print(this + " has child object " + this.transform.GetChild(0).gameObject);  //print each cell's name
                }
                
            }
        }
    }


}