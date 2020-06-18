using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public Item[] cards;
    public GameObject card;

    public GameObject[] emptySlots;
    public GameObject emptySlot;


    private void Start()
    {
        card = GameObject.Find("BlueSquare_UI");
        emptySlot = GameObject.Find("Empty01_UI");
    }


}

