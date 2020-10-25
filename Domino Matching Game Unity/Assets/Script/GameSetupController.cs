using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//This script is placed in each game scene and allows for player instantiation at each scene. 
//It also prints player's domino rotation and parent information, I'm hoping to use this info when comparing the player's dominoes to the host's dominoes in order to create a score.

public class GameSetupController : MonoBehaviour
{
    


    
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        CreatePlayer();
        DontDestroyOnLoad(this.gameObject);
        
    }

    private void Update()
    {
        PrintPlayerArrayInfo();
    }


    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
        //GameObject[] playerDominoes = GameObject.FindGameObjectsWithTag("Tile");
        /*foreach (GameObject domino in playerDominoes)  //prints the name of each gameobject in the created array
        {
            print(domino);
        }*/
    }

    void PrintPlayerArrayInfo()
    {
        GameObject[] playerDominoes = GameObject.FindGameObjectsWithTag("Tile");

        if (Input.GetKeyDown(KeyCode.O))
        {
            foreach (GameObject playerDomino in playerDominoes)
            {
                print("The " + playerDomino + " is at an angle of " + playerDomino.transform.rotation.eulerAngles.z + "degrees. ");

                if(playerDomino.transform.parent != null)
                {
                    print("The " + playerDomino + " is in " + playerDomino.transform.parent);
                }
                /*if (playerDomino.transform.ParentCount > 0)   //check is BoardCell has a child
                {
                    print(this + " has child object " + this.transform.GetChild(0).gameObject);  //print each cell's name
                }*/

            }
        }
    }
}
