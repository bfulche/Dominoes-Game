using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> cardList = new List<GameObject>();
    public List<GameObject> slotList = new List<GameObject>();
    public List<GameObject> selectedCards = new List<GameObject>();

   

    private void Start()
    {
        foreach (GameObject ctag in GameObject.FindGameObjectsWithTag("cards"))
        {
            cardList.Add(ctag);

        }

        foreach (GameObject stag in GameObject.FindGameObjectsWithTag("emptySlots"))
        {
            slotList.Add(stag);

        } 

        
    }

    private void Update()
    {




    }


}
