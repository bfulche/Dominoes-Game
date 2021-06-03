using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReplayGame : MonoBehaviourPunCallbacks
{
  //  [SerializeField] TMP_Text gameTitle;
 //   [SerializeField] TMP_Text playerCounter;

    // not sure if needed yet
    [SerializeField] TMP_Dropdown levelSelectDropdown;
    [SerializeField] Image levelPreviewImage;
    [SerializeField] TMP_Text levelPreviewDescription;
    [SerializeField] TMP_Text levelDominoAndTimerDescription;


    [SerializeField] TMP_Dropdown leaderSelectDropdown;
 //   [SerializeField] TMP_InputField leaderSelectTextBox; // for non hosts

 //   [SerializeField] Toggle observerStatusToggle;


  //  [SerializeField] TextRefHolder hostLeftError;

 //   private List<Player> cachedPlayerList;
    private List<string> cachedPlayerListStrings;
    private LevelData data;
 //   private int roomMax;
//    private int roomCurrent;

 //   private string hostName;

    public void OnLevelDropDownChanged(int levelIndex)
    {
        RPC_ChangeLevelPreview(levelIndex);

     //   if (!PhotonNetwork.IsMasterClient)
     //       return;
     //
     //   this.photonView.RPC(PhotonProperty.ChangeLevelPreview, RpcTarget.AllBuffered, levelIndex);
    }

    public void OnLeaderDropDownChanged(int leaderIndex)
    {
        string newLeader = leaderSelectDropdown.options[leaderIndex].text;

        // -1 because dropdown at index 0 isn't a player.
        if (leaderIndex == 0)   // picked randomize leader
        {
            return;
          //  int random = Random.Range(0, cachedPlayerList.Count);
          //  StartingGameLevel.designatedLeader = cachedPlayerList[random];
          //  Debug.Log("Randomize leader selected. Randomized leader set to: " + StartingGameLevel.designatedLeader.NickName);
        }
        else
        {
            StartingGameLevel.designatedLeader = StartingGameLevel.activePlayers[leaderIndex - 1];
            Debug.Log("Game Leader set to: " + StartingGameLevel.designatedLeader.NickName);
        }
      //  photonView.RPC(PhotonProperty.UpdateLeaderText, RpcTarget.All, newLeader);
    }

  //  public void OnObserverStatusChanged(bool isObserving)
  //  {
  //      photonView.RPC(PhotonProperty.UpdatePlayerObservingState, RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer, isObserving);
  //  }

  // [PunRPC]
  // public void RPC_UpdatePlayerObservingState(Player targetPlayer, bool newState)
  // {
  //     // attempted safety measure for buffered rpc approach.
  //     // if (targetPlayer != null)
  //     targetPlayer.TagObject = newState;
  // }

    [PunRPC]
    public void RPC_ChangeLevelPreview(int levelIndex)
    {
        levelSelectDropdown.value = levelIndex;
        levelSelectDropdown.RefreshShownValue();
        data = GlobalGameData.Instance.GetLevelData(levelIndex);

        // gameTitle.text = data.LevelName;
        levelPreviewImage.sprite = data.LevelImage;    // might need to save as sprite in lvl data
        levelPreviewDescription.text = data.LevelDescription;

        // Setup level Domino and Timer description
        float timer = data.RoundDuration;

        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);

        string timeDisplay = string.Format("Time Limit: {0}m {1}s", minutes, seconds);

        levelDominoAndTimerDescription.text = "Dominos: " + data.DominoCount + "\t" + timeDisplay;
    }


    private void InitializeRoom()
    {

        #region Level Select Dropdown
        int levelCount = GlobalGameData.Instance.Levels.Count;

        levelSelectDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < levelCount; i++)
        {
            options.Add(GlobalGameData.Instance.GetLevelData(i).LevelName);
            //   Debug.Log("Option: " + GlobalGameData.Instance.GetLevelData(i).LevelName + " added.");
        }

        levelSelectDropdown.AddOptions(options);

        #endregion

        #region Leader Select Dropdown

        // Besides the master client (host), the leader select dropdown is not updated/resized as players join or leave the room
        // Only the master client has the updated list since it could change frequently.
        // Everyone else is sent the player name so their one dropdown option can be renamed to the player name.

        cachedPlayerListStrings = new List<string>();
        leaderSelectDropdown.ClearOptions();

        cachedPlayerListStrings.Add("Random Leader");

        foreach (Player player in StartingGameLevel.activePlayers)
        {
            cachedPlayerListStrings.Add(player.NickName);
        }

        leaderSelectDropdown.AddOptions(cachedPlayerListStrings);

        #endregion
    }

    #region Room Callbacks
    /// <summary>
    /// Might be able to scrap the Update Player Counter RPC. Need testing
    /// </summary>
    /// <param name="newPlayer"></param>
    //   public override void OnPlayerEnteredRoom(Player newPlayer)
    //   {
    //       base.OnPlayerEnteredRoom(newPlayer);
    //
    //       // only host will bother keeping track of player list. After correcting list will send via RPC
    //       if (PhotonNetwork.IsMasterClient)
    //       {
    //           int currentleader = leaderSelectDropdown.value;
    //           cachedPlayerList.Add(newPlayer);
    //           cachedPlayerListStrings.Add(newPlayer.NickName);
    //           leaderSelectDropdown.ClearOptions();
    //           leaderSelectDropdown.AddOptions(cachedPlayerListStrings);
    //
    //           leaderSelectDropdown.value = currentleader;
    //           leaderSelectDropdown.RefreshShownValue();
    //       }
    //
    //       roomCurrent += 1;
    //       playerCounter.text = string.Format("{0}/{1}", roomCurrent, roomMax);
    //
    //       newPlayer.TagObject = false;
    //
    //       photonView.RPC(PhotonProperty.UpdatePlayerList, RpcTarget.All);
    //   }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {

        // probably need to add protections for when a player decides to leave...

     //   base.OnPlayerLeftRoom(otherPlayer);
     //
     //   // if the host leaves. Tell everyone host left and kick everyone from the room.
     //   if (otherPlayer.NickName == hostName)
     //   {
     //       // pop up a "host has left" message and then send everyone back to... join lobby?
     //       // still getting the duplicating room listings bug... for now just sending people back to main menu scene
     //       // gives list chance to "refresh" until a proper fix can be found.
     //       hostLeftError.gameObject.SetActive(true);
     //       hostLeftError.Text.text = string.Format("{0} has closed the game. Returning to Main Menu.", otherPlayer.NickName);
     //
     //       return;
     //
     //   }
     //
     //   if (PhotonNetwork.IsMasterClient)
     //   {
     //       // if the selected leader left. Default back to the host as the leader
     //       if (leaderSelectDropdown.options[leaderSelectDropdown.value].text == otherPlayer.NickName)
     //       {
     //           Debug.Log("Assigned leader has left the game. Defaulting to host.");
     //           leaderSelectDropdown.value = 1;
     //           leaderSelectDropdown.RefreshShownValue();
     //           photonView.RPC(PhotonProperty.UpdateLeaderText, RpcTarget.Others, PhotonNetwork.MasterClient.NickName);
     //       }
     //
     //
     //       int currentleader = leaderSelectDropdown.value;
     //
     //       cachedPlayerList.Remove(otherPlayer);
     //       cachedPlayerListStrings.Remove(otherPlayer.NickName);
     //       leaderSelectDropdown.ClearOptions();
     //       leaderSelectDropdown.AddOptions(cachedPlayerListStrings);
     //       leaderSelectDropdown.value = currentleader;
     //       leaderSelectDropdown.RefreshShownValue();
     //   }
     //
     //   roomCurrent -= 1;
     //   playerCounter.text = string.Format("{0}/{1}", roomCurrent, roomMax);
     //
     //   //  photonView.RPC(PhotonProperty.UpdatePlayerList, RpcTarget.All);
    }

    #endregion

    //  [PunRPC]
    //  public void RPC_UpdatePlayerList()
    //  {
    //      //   matchMaking.RefreshPlayerList();
    //  }

    //  [PunRPC]
    //  public void RPC_UpdateLeaderText(string newLeader)
    //  {
    //      leaderSelectTextBox.text = newLeader;
    //  }

    public override void OnEnable()
    {
        base.OnEnable();

        InitializeRoom();


        OnLevelDropDownChanged(0);
        OnLeaderDropDownChanged(0);
    }

    #region UI Buttions

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartingGameLevel.startingLevel = levelSelectDropdown.value;

            // if using random lead. Pick random lead now
            if (leaderSelectDropdown.value == 0)
            {
                // assign random leader from eligible players
                int random = Random.Range(0, StartingGameLevel.activePlayers.Count);
                StartingGameLevel.designatedLeader = StartingGameLevel.activePlayers[random];
                Debug.Log("Randomize leader selected. Randomized leader set to: " + StartingGameLevel.designatedLeader.NickName);
            }
            else
            {
                StartingGameLevel.designatedLeader = StartingGameLevel.activePlayers[leaderSelectDropdown.value - 1];
                Debug.Log(StartingGameLevel.designatedLeader + " has been sent as the selected leader");
            }

            PhotonNetwork.LoadLevel(2);
        }
    }

    public void LeaveRoomLobby()
    {
        // to circumvent the duplicate room listing bug... just sending leavers to main menu for now
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();

        // PhotonNetwork.Disconnect();
        //  SceneManager.LoadScene(0);
    }

    #endregion
}
