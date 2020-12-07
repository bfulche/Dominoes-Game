﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2 startingPosition;
    private List<Transform> touchingTiles;
    private Transform myParent;

    [SerializeField] private int id;
    public int ID => id;    // read only to get tile ID.



    private void Awake()
    {
        startingPosition = transform.position;
        touchingTiles = new List<Transform>();
        myParent = transform.parent;
    }








    public void PickUp()
    {
        transform.localScale = new Vector3(1.1f, 1.1f, 1);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }






    public void Drop()
    {

        transform.localScale = new Vector3(1, 1, 1);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;

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
            Debug.Log("Has entered cell");
            touchingTiles.Add(other.transform);
        }
    }








    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Cell") return;
        if (touchingTiles.Contains(other.transform))
        {
            touchingTiles.Remove(other.transform);
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







    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            this.transform.Rotate(0.0f, 0.0f, -90.0f, Space.World);

        }
    }
}
