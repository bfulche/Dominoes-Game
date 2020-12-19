using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using System.Threading;
using UnityEngine.SceneManagement;


//I'm using this script to manage mouse buttons on the dominoes.
//It's also being used to print the player list for the Photon Room and print a player's domino information.
//It also is attached to the Manager in each scene. 
//It's also used to call the 'Next Scene' function for buttons.
public class InputManager : MonoBehaviourPunCallbacks
{
    private bool draggingItem = false;
    private GameObject draggedObject;
    private Vector2 touchOffset;

    GameObject[] dominoArray;

    [SerializeField] GameObject ScoreboardPanel;



    //Trying to create an array of dominoes for each player and a round score.
    void Awake()
    {
        dominoArray = GameObject.FindGameObjectsWithTag("Tile");
        //ScoreboardPanel.SetActive(false);
    }
    
    

    void Update()
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

        PrintDominoArrayInfo();
        PrintPlayerList();




        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach (GameObject domino in dominoArray)
            {
                print(domino);
            }

        }
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



    //Prints the names of each player in the current Photon Room
    void PrintPlayerList()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                print("The players in this room are: " + player.NickName);
            }
            
        }
    }





    //If a domino has a parent, this prints the domino's name, z-rotation value, and parent name
    void PrintDominoArrayInfo()
    {
        GameObject[] playerDominoes = GameObject.FindGameObjectsWithTag("Tile");

        if (Input.GetKeyDown(KeyCode.O))
        {
            foreach (GameObject playerDomino in playerDominoes)
            {
                if (playerDomino.transform.parent != null)
                {
                    print("The " + playerDomino.name + " is in " + playerDomino.transform.parent.name + ", with a rotation of " + playerDomino.transform.rotation.eulerAngles.z + " degrees.");
                }  
            }
        }
    }










    // This calls the scoreboard for each round
    // Looks for game object "ScoreboardPanel"
    public void toScoreboard()
    {
        ScoreboardPanel.SetActive(true);
    }



    //This is to be called on buttons that take the player to the next scene
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }








    //This is to be called on buttons that take the player to the first scene
    public void FirstScene()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}