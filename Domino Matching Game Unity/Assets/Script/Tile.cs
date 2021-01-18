using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script determines the movement for 'tile' tagged objects, meaning dominoes. It helps with dropping the dominoes into the squares/'cells'

public class Tile : MonoBehaviour
{
    private Vector2 startingPosition;
    private List<Transform> touchingTiles;
    private Transform myParent;

    private Vector3 initialPosition;
    private Quaternion initialRotation;



    int id = -1;
    public int ID
    {
        get { return id; }
        set
        {
            if (id != -1)   // only allow id edits if value isn't initialized (-1)
                return;
            if (0 > value)  // don't edit if initialization value is negative
                return;

            id = value; // initialize id using specified value
        }
    }

    private static List<Tile> allTiles = new List<Tile>();
    private static List<Tile> mimicTiles = new List<Tile>();

    BoxCollider2D parentedCell = null;

    private void Awake()
    {
        startingPosition = transform.position;
        touchingTiles = new List<Transform>();
        myParent = transform.parent;
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        allTiles.Add(this);
    }

    public void SetInitialPositionAndRotation(Vector3 newPosition, Quaternion newRotation)
    {
        initialPosition = newPosition;
        initialRotation = newRotation;
    }

    /// <summary>
    /// Resets all tiles to initial positions
    /// </summary>
    public static void ResetTiles()
    {
        foreach (Tile tile in allTiles)
        {
            tile.transform.position = tile.initialPosition;
            tile.transform.rotation = tile.initialRotation;
            tile.transform.parent = tile.myParent;

            // should fix being unable to slot a tile into a cell in the next round. The cell still had it's collider disabled until
            // the previously slotted tile was picked up and the parented cell was re-enabled as part of picking up
            if (tile.parentedCell != null)
            {
                tile.parentedCell.enabled = true;
                tile.parentedCell = null;
            }
        }
    }

    public static void ResetMimics()
    {
        foreach (Tile tile in mimicTiles)
        {
            tile.transform.position = tile.initialPosition;
            tile.transform.rotation = tile.initialRotation;
            tile.transform.parent = tile.myParent;

            // should fix being unable to slot a tile into a cell in the next round. The cell still had it's collider disabled until
            // the previously slotted tile was picked up and the parented cell was re-enabled as part of picking up
            if (tile.parentedCell != null)
            {
                tile.parentedCell.enabled = true;
                tile.parentedCell = null;
            }
        }
    }

    public void SetAsMimic()
    {
        allTiles.Remove(this);
        mimicTiles.Add(this);
    }

    /// <summary>
    /// Remove Tiles from list in between levels.
    /// </summary>
    public static void ClearTileList()
    {
        allTiles.Clear();
        mimicTiles.Clear();
    }

    public void SetTileID(int newID)
    {
        id = newID;
    }

    public void PickUp()
    {
        if (parentedCell != null)
            parentedCell.enabled = true;
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
            // getting and disabling cell's box collider. When trying to pickup tile later. The collider sometimes
            // gets in the way.
            parentedCell = currentCell.GetComponent<BoxCollider2D>();
            parentedCell.enabled = false;
            StartCoroutine(SlotIntoPlace(transform.position, newPosition));
        }
    }






    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Cell") return;
        if (!touchingTiles.Contains(other.transform))
        {
            //Debug.Log("Has entered cell");
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
            // check input manager enabled
            if (InputManager.Instance.enabled)
                this.transform.Rotate(0.0f, 0.0f, -45.0f, Space.World);

        }
    }
}
