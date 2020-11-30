using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{

    //from "Clicking and Dragging 2D Spriters" YouTube tutorial by Nade
    private float startPosX;
    private float startPosY;
    private bool isBeingHeld = false;


    //From Word GameDrag and Drop website code
    private Vector2 startingPosition;
    private List<Transform> touchingTiles;
    private Transform myParent;



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
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            this.transform.Rotate(0.0f, 0.0f, -45, Space.World);
            Debug.Log("Has entered object");

        }
    }




    private void OnMouseUp()
    {
        isBeingHeld = false;
    }


    public void Drop()
    {
        Vector2 newPosition;
        if (touchingTiles.Count == 0)
        {
            transform.position = startingPosition;
            transform.parent = myParent;
            return;
        }

        var currentCell = touchingTiles[0];
        if (touchingTiles.Count == 1)
        {
            newPosition = currentCell.position;
        }
        else
        {
            var distance = Vector2.Distance(transform.position, touchingTiles[0].position);

            foreach (Transform cell in touchingTiles)
            {
                if (Vector2.Distance(transform.position, cell.position) < distance)
                {
                    currentCell = cell;
                    distance = Vector2.Distance(transform.position, cell.position);
                }
            }
            newPosition = currentCell.position;

        }
        if (currentCell.childCount != 0)
        {
            transform.position = startingPosition;
            transform.parent = myParent;
            return;
        }
        else
        {
            transform.parent = currentCell;
            StartCoroutine(SlotIntoPlace(transform.position, newPosition));
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Cell") return;
        if (!touchingTiles.Contains(other.transform))
        {
            touchingTiles.Add(other.transform);
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Cell") return;
        if (!touchingTiles.Contains(other.transform))
        {
            touchingTiles.Add(other.transform);
        }
    }


    IEnumerator SlotIntoPlace(Vector2 startingPos, Vector2 endingPos)
    {
        float duration = 0.1f;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            transform.position = Vector2.Lerp(startingPos, endingPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = endingPos;
    }




    void Update()
    {
        if(isBeingHeld == true)
        {
            Vector3 mousePose;
            mousePose = Input.mousePosition;
            mousePose = Camera.main.ScreenToWorldPoint(mousePose);

            this.gameObject.transform.localPosition = new Vector3(mousePose.x - startPosX, mousePose.y - startPosY, 0);

            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            transform.localScale = new Vector3(110, 110, 110);
        }

        if(isBeingHeld == false)
        {
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            transform.localScale = new Vector3(100, 100, 100);
        }
    }
}
