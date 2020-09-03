using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject[,] positions = new GameObject[3, 2];
    private GameObject[] playerDominos = new GameObject[2];


    public GameObject domino1;
    public GameObject domino2;
    public GameObject domino3;


    void Start()
    {
        Instantiate(domino1, new Vector3(-200, -160, -1), Quaternion.identity);
        Instantiate(domino2, new Vector3(0, -160, -1), Quaternion.identity);
        Instantiate(domino3, new Vector3(200, -160, -1), Quaternion.identity);

    }


    void Update()
    {
        
    }
}
