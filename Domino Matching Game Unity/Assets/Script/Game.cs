using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script helps with the dominoes snapping into their suare slots

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

    //DropItem function from Drag and Drop Word Game Website
    void DropItem()
    {
        draggingItem = false;
        draggedObject.transform.localScale = new Vector3(100, 100, 100);
        draggedObject.GetComponent<DragAndDrop>().Drop();
    }

    void Update()
    {
        
    }
}
