using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomPanel : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text gameTitle;
    [SerializeField] TMP_Text playerCounter;

    // not sure if needed yet
    [SerializeField] TMP_Dropdown levelSelectDropdown;
    [SerializeField] Image levelPreviewImage;
    [SerializeField] TMP_Text levelPreviewDescription;
    [SerializeField] TMP_Text levelDominoAndTimerDescription;


    [SerializeField] TMP_Dropdown leaderSelectDropdown;
    [SerializeField] TMP_InputField leaderSelectTextBox; // for non hosts

    [SerializeField] Toggle observerStatusToggle;

    [SerializeField] MatchMaking matchMaking;

    [SerializeField] TextRefHolder hostLeftError;

    private List<Player> cachedPlayerList;
    private List<string> cachedPlayerListStrings;
    private LevelData data;
    private int roomMax;
    private int roomCurrent;

    private string hostName;

    public void OnLevelDropDownChanged(int levelIndex)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        this.photonView.RPC(PhotonProperty.ChangeLevelPreview, RpcTarget.All, levelIndex);

        ExitGames.Client.Photon.Hashtable currentLevel = new ExitGames.Client.Photon.Hashtable();
        currentLevel["CurrentLevel"] = levelIndex;

        PhotonNetwork.CurrentRoom.SetCustomProperties(currentLevel);
    }

    public void OnLeaderDropDownChanged(int leaderIndex)
    {
        string newLeader = leaderSelectDropdown.options[leaderIndex].text;

        // -1 because dropdown at index 0 isn't a player.
        if (leaderIndex == 0)   // picked randomize leader
        {
            int random = Random.Range(0, cachedPlayerList.Count);
            StartingGameLevel.designatedLeader = cachedPlayerList[random];
            Debug.Log("Randomize leader selected. Randomized leader set to: " + StartingGameLevel.designatedLeader.NickName);
        }
        else
        {
            StartingGameLevel.designatedLeader = cachedPlayerList[leaderIndex - 1];
            Debug.Log("Game Leader set to: " + StartingGameLevel.designatedLeader.NickName);
        }
        photonView.RPC(PhotonProperty.UpdateLeaderText, RpcTarget.All, newLeader);

        ExitGames.Client.Photon.Hashtable Leader = new ExitGames.Client.Photon.Hashtable();

        Leader["Leader"] = newLeader;
    }

    public void OnObserverStatusChanged(bool isObserving)
    {
        photonView.RPC(PhotonProperty.UpdatePlayerObservingState, RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer, isObserving);
    }

    [PunRPC]
    public void RPC_UpdatePlayerObservingState(Player targetPlayer, bool newState)
    {
        // attempted safety measure for buffered rpc approach.
        // if (targetPlayer != null)
        targetPlayer.TagObject = newState;
    }

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

        cachedPlayerList = new List<Player>();
        cachedPlayerListStrings = new List<string>();
        leaderSelectDropdown.ClearOptions();

        cachedPlayerList = PhotonNetwork.PlayerList.ToList();
        cachedPlayerListStrings.Add("Random Leader");

        //    cachedPlayerList.Add(PhotonNetwork.MasterClient);
        //     cachedPlayerListStrings.Add(PhotonNetwork.MasterClient.NickName);

        foreach (Player player in cachedPlayerList)
        {
            cachedPlayerListStrings.Add(player.NickName);
            // while we're here. Set tagObject to false in case we later become leader and play again
            if (player.TagObject == null)
                player.TagObject = false;
        }

        leaderSelectDropdown.AddOptions(cachedPlayerListStrings);

        #endregion

        #region Player Counter

        roomMax = PhotonNetwork.CurrentRoom.MaxPlayers;

        roomCurrent = PhotonNetwork.CurrentRoom.PlayerCount;

        playerCounter.text = string.Format("{0}/{1}", roomCurrent, roomMax);

        #endregion
    }

    #region Room Callbacks
    /// <summary>
    /// Might be able to scrap the Update Player Counter RPC. Need testing
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        // only host will bother keeping track of player list. After correcting list will send via RPC
        if (PhotonNetwork.IsMasterClient)
        {
            int currentleader = leaderSelectDropdown.value;
            cachedPlayerList.Add(newPlayer);
            cachedPlayerListStrings.Add(newPlayer.NickName);
            leaderSelectDropdown.ClearOptions();
            leaderSelectDropdown.AddOptions(cachedPlayerListStrings);

            leaderSelectDropdown.value = currentleader;
            leaderSelectDropdown.RefreshShownValue();
        }

        roomCurrent += 1;
        playerCounter.text = string.Format("{0}/{1}", roomCurrent, roomMax);

        newPlayer.TagObject = false;

        photonView.RPC(PhotonProperty.UpdatePlayerList, RpcTarget.All);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        // if the host leaves. Tell everyone host left and kick everyone from the room.
        if (otherPlayer.NickName == hostName)
        {
            // pop up a "host has left" message and then send everyone back to... join lobby?
            // still getting the duplicating room listings bug... for now just sending people back to main menu scene
            // gives list chance to "refresh" until a proper fix can be found.
            hostLeftError.gameObject.SetActive(true);
            hostLeftError.Text.text = string.Format("{0} has closed the game. Returning to Main Menu.", otherPlayer.NickName);

            return;

        }

        if (PhotonNetwork.IsMasterClient)
        {
            // if the selected leader left. Default back to the host as the leader
            if (leaderSelectDropdown.options[leaderSelectDropdown.value].text == otherPlayer.NickName)
            {
                Debug.Log("Assigned leader has left the game. Defaulting to host.");
                leaderSelectDropdown.value = 1;
                leaderSelectDropdown.RefreshShownValue();
                photonView.RPC(PhotonProperty.UpdateLeaderText, RpcTarget.Others, PhotonNetwork.MasterClient.NickName);
            }


            int currentleader = leaderSelectDropdown.value;

            cachedPlayerList.Remove(otherPlayer);
            cachedPlayerListStrings.Remove(otherPlayer.NickName);
            leaderSelectDropdown.ClearOptions();
            leaderSelectDropdown.AddOptions(cachedPlayerListStrings);
            leaderSelectDropdown.value = currentleader;
            leaderSelectDropdown.RefreshShownValue();
        }

        roomCurrent -= 1;
        playerCounter.text = string.Format("{0}/{1}", roomCurrent, roomMax);

        photonView.RPC(PhotonProperty.UpdatePlayerList, RpcTarget.All);
    }

    #endregion

    [PunRPC]
    public void RPC_UpdatePlayerList()
    {
        matchMaking.RefreshPlayerList();
    }

    [PunRPC]
    public void RPC_UpdateLeaderText(string newLeader)
    {
        leaderSelectTextBox.text = newLeader;
    }

    public override void OnEnable()
    {
        base.OnEnable();

        gameTitle.text = PhotonNetwork.CurrentRoom.Name;
        InitializeRoom();

        if (PhotonNetwork.IsMasterClient)
        {
            leaderSelectTextBox.gameObject.SetActive(false);
            leaderSelectDropdown.gameObject.SetActive(true);
            hostName = PhotonNetwork.MasterClient.NickName;

            StartCoroutine(DelayInitializeDropDownValues());
            //  leaderSelectDropdown.gameObject.SetActive(false);
            //   levelSelectDropdown.gameObject.SetActive(false);

            // leaderSelectTextBox.gameObject.SetActive(true);
        }
        else
        {
            hostName = PhotonNetwork.MasterClient.NickName;
            levelSelectDropdown.interactable = false;

            int lvl = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentLevel"];
            // string leader = (string)PhotonNetwork.CurrentRoom.CustomProperties["Leader"];

            //    Debug.Log("Leader from Custom properties: " + leader);
            Debug.Log("Level from custom properties: " + lvl);
            levelSelectDropdown.value = lvl;
            RPC_ChangeLevelPreview(lvl);

            RPC_UpdatePlayerList();
        }

        PhotonNetwork.LocalPlayer.TagObject = false; // initialize observer state to false.
    }

    IEnumerator DelayInitializeDropDownValues()
    {
        yield return new WaitForEndOfFrame();

        OnLevelDropDownChanged(0);
        OnLeaderDropDownChanged(0);
        photonView.RPC(PhotonProperty.UpdatePlayerList, RpcTarget.All);
    }

    #region UI Buttions

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartingGameLevel.startingLevel = levelSelectDropdown.value;

            List<Player> observerList = new List<Player>();

            List<Player> eligiblePlayers = new List<Player>();

            // build observer list AND active players list
            for (int i = 0; i < cachedPlayerList.Count; i++)
            {
                if ((bool)cachedPlayerList[i].TagObject)
                {
                    Debug.Log("Player: " + cachedPlayerList[i].NickName + " is registered as an observer!");
                    observerList.Add(cachedPlayerList[i]);
                }
                else
                {
                    eligiblePlayers.Add(cachedPlayerList[i]);
                }
            }

            // if using random lead. Pick random lead now
            if (leaderSelectDropdown.value == 0)
            {
                // assign random leader from eligible players
                int random = Random.Range(0, eligiblePlayers.Count);
                StartingGameLevel.designatedLeader = eligiblePlayers[random];
                Debug.Log("Randomize leader selected. Randomized leader set to: " + StartingGameLevel.designatedLeader.NickName);
            }
            else
            {
                StartingGameLevel.designatedLeader = cachedPlayerList[leaderSelectDropdown.value - 1];
                Debug.Log(StartingGameLevel.designatedLeader + " has been sent as the selected leader");
            }

            StartingGameLevel.observingPlayers = observerList;
            StartingGameLevel.activePlayers = eligiblePlayers;


            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(3);
        }
    }

    public void LeaveRoomLobby()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

    #endregion
}
