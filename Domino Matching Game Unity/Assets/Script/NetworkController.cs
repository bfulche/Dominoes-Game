using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script gets the Photon Networking up and running

public class NetworkController : MonoBehaviourPunCallbacks
{

    // Scripting API: https://doc-api.photonengine.com/en/pun/v2/index/html


    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();{
            Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " server!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
