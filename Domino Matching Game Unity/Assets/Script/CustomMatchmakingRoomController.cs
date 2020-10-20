using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.IO;

public class CustomMatchmakingRoomController : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    [SerializeField]
    private int multiPlayerSceneIndex;

    [SerializeField]
    private GameObject lobbyPanel;
    [SerializeField]
    private GameObject roomPanel;

    [SerializeField]
    private GameObject startButton;

    [SerializeField]
    private GameObject startButton2;

    [SerializeField]
    private GameObject startButton3;

    [SerializeField]
    private Transform playersContainer;
    [SerializeField]
    private GameObject playerListingPrefab;

    [SerializeField]
    private Text roomNameDisplay;





    void ClearPLayerListings()
    {
        for(int i = playersContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(playersContainer.GetChild(i).gameObject);
        }
    }

    void ListPLayers()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject tempListing = Instantiate(playerListingPrefab, playersContainer);
            Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
            tempText.text = player.NickName;
        }
    }

    public override void OnJoinedRoom()
    {
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        roomNameDisplay.text = PhotonNetwork.CurrentRoom.Name;
        if(PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
            startButton2.SetActive(true);
            startButton3.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
            startButton2.SetActive(false);
            startButton3.SetActive(false);
        }

        ClearPLayerListings();
        ListPLayers();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ClearPLayerListings();
        ListPLayers();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ClearPLayerListings();
        ListPLayers();
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
            startButton2.SetActive(true);
            startButton3.SetActive(true);
        }
    }

    public void StartGame()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(1);
        }
    }

    public void StartGameLevelTwo()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(7);
        }
    }

    public void StartGameLevelThree()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(13);
        }
    }

    IEnumerator rejoinlobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }

    public void BackOnClick()
    {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinlobby());
    }

}
