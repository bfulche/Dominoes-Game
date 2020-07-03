using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> selectedPlayer1Cards = new List<GameObject>();

    public List<GameObject> selectedPlayer2Cards = new List<GameObject>();

    public Text finalScore;

    private ItemSlot empty1;
    private ItemSlot empty2;
    private ItemSlot empty3;
    private ItemSlot empty4;
    private ItemSlot empty5;

    private ItemSlot P2empty1;
    private ItemSlot P2empty2;
    private ItemSlot P2empty3;
    private ItemSlot P2empty4;
    private ItemSlot P2empty5;


    public int matchedCards;
    

    private void Start()
    {
        empty1 = GameObject.Find("Empty01_UI").GetComponent<ItemSlot>();
        empty2 = GameObject.Find("Empty02_UI").GetComponent<ItemSlot>();
        empty3 = GameObject.Find("Empty03_UI").GetComponent<ItemSlot>();
        empty4 = GameObject.Find("Empty04_UI").GetComponent<ItemSlot>();
        empty5 = GameObject.Find("Empty05_UI").GetComponent<ItemSlot>();

        P2empty1 = GameObject.Find("P2Empty01_UI").GetComponent<ItemSlot>();
        P2empty2 = GameObject.Find("P2Empty02_UI").GetComponent<ItemSlot>();
        P2empty3 = GameObject.Find("P2Empty03_UI").GetComponent<ItemSlot>();
        P2empty4 = GameObject.Find("P2Empty04_UI").GetComponent<ItemSlot>();
        P2empty5 = GameObject.Find("P2Empty05_UI").GetComponent<ItemSlot>();

        


    }

  

    

    private void Update()
    {
        selectedPlayer1Cards[0] = empty1.currentCard;
        selectedPlayer1Cards[1] = empty2.currentCard;
        selectedPlayer1Cards[2] = empty3.currentCard;
        selectedPlayer1Cards[3] = empty4.currentCard;
        selectedPlayer1Cards[4] = empty5.currentCard;


        selectedPlayer2Cards[0] = P2empty1.currentCard;
        selectedPlayer2Cards[1] = P2empty2.currentCard;
        selectedPlayer2Cards[2] = P2empty3.currentCard;
        selectedPlayer2Cards[3] = P2empty4.currentCard;
        selectedPlayer2Cards[4] = P2empty5.currentCard;

        int matchedCards = 0;

       
            
        
           if(empty1.currentCard.tag == P2empty1.currentCard.tag)
        {
            matchedCards += 1;
            Debug.Log("slot 1 is matching for player 1 and player 2. CUrrent score is " + matchedCards);
        }

        if (empty2.currentCard.tag == P2empty2.currentCard.tag)
        {
            matchedCards += 1;
            Debug.Log("slot 2 is matching for player 1 and player 2. CUrrent score is " + matchedCards);
            
        }

        if (empty3.currentCard.tag == P2empty3.currentCard.tag)
        {
            matchedCards += 1;
            Debug.Log("slot 3 is matching for player 1 and player 2. CUrrent score is " + matchedCards);
        }

        if (empty4.currentCard.tag == P2empty4.currentCard.tag)
        {
            matchedCards += 1;
            Debug.Log("slot 4 is matching for player 1 and player 2. CUrrent score is " + matchedCards);
        }

        if (empty5.currentCard.tag == P2empty5.currentCard.tag)
        {
            matchedCards += 1;
            Debug.Log("slot 5 is matching for player 1 and player 2. CUrrent score is " + matchedCards);
        }


        finalScore.text = matchedCards.ToString();
        Debug.Log("final score is " + matchedCards);
      



    }


}
