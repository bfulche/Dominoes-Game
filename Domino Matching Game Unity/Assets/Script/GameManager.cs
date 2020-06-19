using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //public List<GameObject> cardList = new List<GameObject>();
    //public List<GameObject> slotList = new List<GameObject>();
    public List<GameObject> selectedCards = new List<GameObject>();

    /*public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    public GameObject slot4;
    public GameObject slot5;*/

    private ItemSlot empty1;
    private ItemSlot empty2;
    private ItemSlot empty3;
    private ItemSlot empty4;
    private ItemSlot empty5;

    private void Start()
    {
        empty1 = GameObject.Find("Empty01_UI").GetComponent<ItemSlot>();
        empty2 = GameObject.Find("Empty02_UI").GetComponent<ItemSlot>();
        empty3 = GameObject.Find("Empty03_UI").GetComponent<ItemSlot>();
        empty4 = GameObject.Find("Empty04_UI").GetComponent<ItemSlot>();
        empty5 = GameObject.Find("Empty05_UI").GetComponent<ItemSlot>();
        /*foreach (GameObject ctag in GameObject.FindGameObjectsWithTag("cards"))
        {
            cardList.Add(ctag);

        }

        foreach (GameObject stag in GameObject.FindGameObjectsWithTag("emptySlots"))
        {
            slotList.Add(stag);

        } */

    }

    

    private void Update()
    {
        selectedCards[0] = empty1.currentCard;
        selectedCards[1] = empty2.currentCard;
        selectedCards[2] = empty3.currentCard;
        selectedCards[3] = empty4.currentCard;
        selectedCards[4] = empty5.currentCard;



    }


}
