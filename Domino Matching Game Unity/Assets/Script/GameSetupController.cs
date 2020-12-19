using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//This script is placed in each game scene and allows for player instantiation at each scene that the prefab is placed in.

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


    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
    }
}
