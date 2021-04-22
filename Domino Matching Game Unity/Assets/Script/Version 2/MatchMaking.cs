using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Basically combining the custom room and lobby matchmaking scripts. Lots of copy/paste from 
/// the work already done there.
/// </summary>
public class MatchMaking : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    [SerializeField] GameObject lobbyPanel, roomPanel, roomFailurePanel;

    [SerializeField] GameObject playerListingPrefab;

    [SerializeField] Transform playersContainer;

    #region Lobby Related callbacks
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.AutomaticallySyncScene = true;
        roomListings = new List<RoomInfo>();

        if (PlayerPrefs.HasKey("NickName"))
        {
            string storedName = PlayerPrefs.GetString("NickName");
            if (storedName == "")
                PhotonNetwork.NickName = "Player " + UnityEngine.Random.Range(0, 1000);
            else
                PhotonNetwork.NickName = storedName;
        }
        else
            PhotonNetwork.NickName = "Player " + UnityEngine.Random.Range(0, 1000);

        PhotonNetwork.JoinLobby();
    }

    private List<RoomInfo> roomListings;
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        Debug.Log("Room List Updated!");
        int tempIndex;

        foreach (RoomInfo room in roomList)
        {
            if (roomListings != null)
                tempIndex = roomListings.FindIndex(ByName(room.Name));
            else
                tempIndex = -1;
      
            if (tempIndex != -1)
            {
                roomList.RemoveAt(tempIndex);
                // Destroy listed object for that room
            }
            if (room.PlayerCount > 0)
            {
                roomListings.Add(room);
                ListRoom(room);
            }
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        // Debug.LogError("Tried to create a new room but failed. Room must already exist with that name.");
        //  Debug.LogError("Error: " + message);
        ReportRoomCreationFailure();
    }

    #endregion

    static Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        { return room.Name == name; };
    }

    [SerializeField] private RoomButton roomListingPrefab;
    [SerializeField] private Transform roomsContainer;
    private void ListRoom(RoomInfo room)
    {
        if (room.IsOpen && room.IsVisible)
        {
            RoomButton tempListing = Instantiate(roomListingPrefab, roomsContainer);
            //  GameObject tempListing = Instantiate(roomListingPrefab, roomsContainer);
            //  RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempListing.SetRoom(room.Name, room.MaxPlayers, room.PlayerCount);
        }
    }

    public IEnumerator RejoinLobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }


    #region Room related Callbacks

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);

        ClearPlayerListings();
        ListPlayers();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
       // Debug.LogError("Attempted to join room \"" + directNameInput + "\" but that room name does not exist.");

        roomJoinFailPanel.Text.text = "The requested game \"" + directNameInput + "\" could not be found.";
        roomJoinFailPanel.gameObject.SetActive(true);

    }
    void ClearPlayerListings()
    {
        for (int i = playersContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(playersContainer.GetChild(i).gameObject);
        }
    }

    void ListPlayers()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject tempListing = Instantiate(playerListingPrefab, playersContainer);

            TMP_Text tempText = tempListing.transform.GetChild(0).GetComponent<TMP_Text>();

            tempText.text = player.NickName;
        }
    }

    public void RefreshPlayerList()
    {
        ClearPlayerListings();
        ListPlayers();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }



    #endregion


    #region Room Creation Dialogue

    private string gameName = "New Game";
    private int maxPlayers = 10;
    private bool isPublic = true;


    public void CreateRoom()
    {
        Debug.Log("Creating room now");
        RoomOptions options = new RoomOptions() { IsVisible = isPublic, IsOpen = true, MaxPlayers = (byte)maxPlayers, BroadcastPropsChangeToAll = true };
        PhotonNetwork.CreateRoom(gameName, options);
    }

    public void CancelMatchMaking()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
    }

    private void ReportRoomCreationFailure()
    {
        roomFailurePanel.SetActive(true);
    }

    public void OnGameNameChanged(string newName)
    {
        gameName = newName;
    }

    public void OnPlayerMaxChanged(string newMax)
    {
        int t = int.Parse(newMax);

        if (t <= 0)
        {
            Debug.LogWarning("User attempted to set max player size as a negative or 0. Using default size 10");
            t = 10;
        }
        else if (t > 999)
        {
            Debug.LogWarning("User attempted to set max player size to greater than player cap of " + 999 + "using cap instead");
            t = 999;
        }

        maxPlayers = t;
    }

    /// <summary>
    /// The dialogue window asks if the game is "Private". So we're inverting the received value.
    /// </summary>
    /// <param name="newValue"></param>
    public void OnIsPrivateChanged(bool newValue)
    {
        isPublic = !newValue;
    }

    #endregion

    #region Private/Direct Join room

    private string directNameInput;
    [SerializeField] TextRefHolder roomJoinFailPanel;

    public void OnDirectJoinInputChanged(string newName)
    {
        directNameInput = newName;
    }

    public void TryDirectJoin()
    {
        PhotonNetwork.JoinRoom(directNameInput);
    }

    #endregion
}
