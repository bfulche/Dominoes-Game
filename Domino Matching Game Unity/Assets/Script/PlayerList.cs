using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerList : MonoBehaviour
{
    [SerializeField] GameObject playerListingPrefab;
    [SerializeField] Transform playersContainer;

    bool listCreated = false;

 //  public void SetNewLeader(Player newLeader)
 //  {
 //      if (newLeader.IsMasterClient)
 //      {
 //          leader = newLeader;
 //      }
 //  }

    void ListPLayers()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject tempListing = Instantiate(playerListingPrefab, playersContainer);
            Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
            tempText.text = player.NickName;
        }
    }

    private void OnEnable()
    {
        if (!listCreated)
        {
            listCreated = true;
            ListPLayers();
        }
    }
}
