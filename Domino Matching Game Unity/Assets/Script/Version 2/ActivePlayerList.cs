using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActivePlayerList : MonoBehaviour
{
    [SerializeField] PlayerReadyCheck playerListingPrefab;

    [SerializeField] Transform playersContainer;

    Player[] playerList;

    Dictionary<Player, PlayerReadyCheck> readyList;

    // need remove leader from received list
    public void InitializeList()
    {
        readyList = new Dictionary<Player, PlayerReadyCheck>();

        List<Player> tList = StartingGameLevel.activePlayers;

        tList.Remove(StartingGameLevel.designatedLeader);

        // now leader is no longer counted in list
        playerList = tList.ToArray();

        foreach (Player player in playerList)
        {
            if (!(bool)player.TagObject)
            {
                PlayerReadyCheck tempListing = Instantiate(playerListingPrefab, playersContainer);

                tempListing.SetPlayer(player);

                readyList.Add(player, tempListing);

                TMP_Text tempText = tempListing.transform.GetChild(0).GetComponent<TMP_Text>();

                tempText.text = player.NickName;
            }
        }
        // hide list after initialization
        gameObject.SetActive(false);
    }

    public void ShowList()
    {
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        foreach (Player player in playerList)
        {
            readyList[player].UpdateState(false);
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Updates the specified player's ready state then checks if all players are ready.
    /// </summary>
    /// <param name="updatingPlayer"></param>
    /// <param name="newState"></param>
    /// <returns></returns>
    public bool UpdatePlayerCheck(Player updatingPlayer, bool newState)
    {
        readyList[updatingPlayer].UpdateState(newState);
        if (ConfirmAllReady())
            return true;
        else
            return false;
        
    }

    private bool ConfirmAllReady()
    {
        foreach (Player player in playerList)
        {
            if (readyList[player].Ready)
                continue;
            else
                return false;
        }

        return true;
    }
}
