using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviourPun
{
    private static GameplayManager _instance;

    public static GameplayManager Instance => _instance;

    [SerializeField] DominoTray dominoTrayRef;
    [SerializeField] DominoRotator dominoRotatorRef;
    [SerializeField] Transform gameplayContainer;

    [SerializeField] ActivePlayerList playerList;
    [SerializeField] GameObject scoreBoardVisual; // replace GameObject with scoreboard-related script
    TimeManager timer;

    [SerializeField] GameObject beginRoundButton;
    [SerializeField] GameObject readySubmitButton;
    [SerializeField] GameObject nextRoundButton;
    [SerializeField] GameObject mainMenuButton;
    //  [SerializeField] GameObject[] leaderOnlyButtons;
    [SerializeField] GameObject playAgainButton; // start new game button and main menu button
    GameBoard currentGameBoard;

    #region Round Management Variables

    private int currentRound = 0;
    private int totalScoreTally = 0;

    // Text representation of scores
    [SerializeField] TMP_Text[] personalScores;
    [SerializeField] TMP_Text[] roundScores;
    [SerializeField] TMP_Text[] totalScores;
    [SerializeField] TMP_Text roundTextDisplay;

    #endregion

    [SerializeField] TabGroup debriefTabGroup;
    [SerializeField] TabButton defaultTab;

    public DominoTray DominoTray => dominoTrayRef;

    public DominoRotator DominoRotator => dominoRotatorRef;

    Dictionary<Player, BoardState> gameState;

    Player currentLeader; // who is supposed to be the sender/leader of the group

    Domino[] localDominoes; // initialized after Dominoes are created in Awake(). Used to create Board state which will be sent 
                            // via RPC to everyone else in Start()

    [SerializeField] RawImage[] comparisonImages;
    [SerializeField] RenderTexture localCamTexture;
    private bool observerMode = false;

    private bool enableInput = true;

    public bool IsObserver => observerMode;

    public bool InputEnabled
    {
        get { return enableInput; }
        set { enableInput = value; }
    }

    public bool IsGameLeader
    {
        get { return (PhotonNetwork.LocalPlayer == currentLeader); }
    }

    private void Awake()
    {
        if (_instance != null)
            Destroy(this);
        else
            _instance = this;

        timer = GetComponent<TimeManager>();

        if (PhotonNetwork.IsMasterClient)
        {
            // send RPC to initialize level (place dominoes and board items etc.
            //   currentLeader = StartingGameLevel.designatedLeader;

            photonView.RPC(PhotonProperty.CreateDominoes, RpcTarget.All, StartingGameLevel.startingLevel, StartingGameLevel.activePlayers.ToArray(), StartingGameLevel.observingPlayers.ToArray(), StartingGameLevel.designatedLeader);
        }
    }

    private void Start()
    {
        timer.timerDone += EndRound;
        dominoTrayRef.OnTrayEmptied += DominoTrayEmptied;
        roundTextDisplay.text = (currentRound + 1).ToString();

        // check if player is observing
        if ((bool)PhotonNetwork.LocalPlayer.TagObject)
        {
            observerMode = true;
            enableInput = false;

            // enable drop down list - still need to make dropdown list of all active players. So 
            // observer can pick who to "mimic" or observe.
        }
    }

    private void DominoTrayEmptied()
    {
        if (PhotonNetwork.LocalPlayer == currentLeader)
        {
            beginRoundButton.SetActive(true);
        }
        else
        {
            readySubmitButton.SetActive(true);
        }
    }
    //  public void HostSendTimerStart()
    //  {
    //      Texture2D newTexture = new Texture2D(240, 160);
    //      RenderTexture.active = localTexture;
    //      newTexture.ReadPixels(new Rect(0, 0, localTexture.width, localTexture.height), 0, 0);
    //      newTexture.Apply();
    //
    //      this.photonView.RPC("SetMimicTexture", RpcTarget.All, newTexture.EncodeToPNG());
    //      this.photonView.RPC("StartTimer", RpcTarget.All);
    //  }
    //
    //  [SerializeField] RawImage tempTestPicSend;
    //  [SerializeField] RenderTexture localTexture;
    //
    //  [PunRPC]
    //  public void SetMimicTexture(byte[] receivedTexture)
    //  {
    //      Texture2D newTexture = new Texture2D(1, 1);
    //      newTexture.LoadImage(receivedTexture);
    //      tempTestPicSend.texture = newTexture;
    //      //  tempTestPicSend.texture = texture;
    //      // tempTestPicSend.gameObject.SetActive(true);
    //  }


    public void BeginRound()
    {
        StartCoroutine(StartRound());
    }
    /// <summary>
    /// In case the leader tried holding a domino while hitting round button... we're using
    /// a coroutine. We'll wait a full frame before taking a picture
    /// so game object(s) have time to resize, and go inactive.
    /// </summary>
    /// <returns></returns>
    IEnumerator StartRound()
    {
        // make sure the leader didn't try clicking button while holding a domino
        Domino.DropSelected();
        InputEnabled = false;

        this.photonView.RPC(PhotonProperty.StartTimer, RpcTarget.AllViaServer);

        yield return new WaitForEndOfFrame();

        // Take a snapshot of the host board so it's ready to send
        Texture2D renderTextureAs2D = new Texture2D(1920, 1080);
        RenderTexture.active = localCamTexture;
        renderTextureAs2D.ReadPixels(new Rect(0, 0, localCamTexture.width, localCamTexture.height), 0, 0);
        renderTextureAs2D.Apply();

      //  yield return new WaitForSeconds(5f);

        this.photonView.RPC(PhotonProperty.UpdateComparisonImages, RpcTarget.All, renderTextureAs2D.EncodeToPNG());
    }

    private void EndRound()
    {
        StartCoroutine(EndRoundDelay());
    }

    IEnumerator EndRoundDelay()
    {
        Domino.DropSelected();

        // disable input
        enableInput = false;

        // wait to ensure any force-drop RPCs were received before calculating score
        yield return new WaitForSeconds(1f);


        //calculate score using game state dictionary. Everyone does this. not just leader
        int groupScore = 0;
        int personalScore = 0;

        int personalMax = GlobalGameData.Instance.Levels[StartingGameLevel.startingLevel].DominoCount;
        int groupMax = personalMax * (StartingGameLevel.activePlayers.Count); // -1 to remove host from tally
        foreach (Player player in StartingGameLevel.activePlayers)
        {
            // skip leader's board
            if (player == currentLeader)
                continue;

            int playerScore = BoardState.CompareBoards(gameState[currentLeader], gameState[player]);

            if (PhotonNetwork.LocalPlayer == player)
                personalScore = playerScore;

            groupScore += playerScore;
        }

        // update personal score
        if (IsGameLeader)
            personalScores[currentRound].text = "N/A";
        else
            personalScores[currentRound].text = personalScore + "/" + personalMax;

        roundScores[currentRound].text = groupScore + "/" + groupMax;

        totalScoreTally += groupScore;
        totalScores[currentRound].text = totalScoreTally.ToString();

        scoreBoardVisual.SetActive(true);

        readySubmitButton.SetActive(false);
        if (currentRound >= 2)
        {
            nextRoundButton.SetActive(false);
            mainMenuButton.SetActive(true);

            if (IsGameLeader)
            {
                    playAgainButton.SetActive(true);
            }
        }

        playerList.Reset();
    }

    public void ShowGamePlayContainer()
    {
        gameplayContainer.gameObject.SetActive(true);
    }

    public void NextRound()
    {
        beginRoundButton.SetActive(false);
        this.photonView.RPC(PhotonProperty.ResetBoards, RpcTarget.All);
    }

    public void MainMenu()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

    public void PlayAgain()
    {
        PhotonNetwork.CurrentRoom.IsOpen = true;
        PhotonNetwork.LoadLevel(2);
    }

    public void ConfirmedReady()
    {
        Domino.DropSelected();

        // disable input
        enableInput = false;
        photonView.RPC(PhotonProperty.SendReadyCheck, RpcTarget.All, PhotonNetwork.LocalPlayer, true);
    }

    IEnumerator WaitForConfirmation()
    {
        bool confirmed = PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);

        yield return null;

        if (!confirmed)
        {
            Debug.LogError("Unable to set " + PhotonNetwork.LocalPlayer + " as the new Master client!");
        }
        else
            Debug.Log("Master client set to: " + PhotonNetwork.MasterClient.NickName);

        PhotonNetwork.CurrentRoom.IsOpen = true;
        PhotonNetwork.LoadLevel(2);
    }

    #region RPCs

    [PunRPC]
    public void RPC_UpdateComparisonImages(byte[] textureAsPNG)
    {
        // not the ideal spot for this line... if cleaning up code in future:
        // Reorganize round start so this isn't shoe-horned in with picture-related stuff
 //       gameplayContainer.gameObject.SetActive(true);

        Texture2D newTexture = new Texture2D(1920, 1080);
        newTexture.LoadImage(textureAsPNG);

        foreach (RawImage image in comparisonImages)
        {
            image.texture = newTexture;
        }
    }

    [PunRPC]
    public void RPC_CreateDominoes(int level, Player[] active, Player[] observing, Player leader)
    {
        // sync startinggamelevel data with what host sent over
        StartingGameLevel.startingLevel = level;
        StartingGameLevel.activePlayers = active.ToList();
        StartingGameLevel.observingPlayers = observing.ToList();
        StartingGameLevel.designatedLeader = leader;

        currentLeader = leader;

        if (PhotonNetwork.LocalPlayer == currentLeader)
        {
            gameplayContainer.gameObject.SetActive(true); // show game board
            nextRoundButton.SetActive(true);

            PhotonNetwork.SetMasterClient(currentLeader);
            //   foreach (GameObject obj in leaderOnlyButtons) // enable leader buttons
            //   {
            //       obj.SetActive(true);
            //   }
        }

        // create the dominoes
        Domino[] prefabData = GlobalGameData.Instance.Levels[level].DominoPrefabs;

        Player[] players = PhotonNetwork.PlayerList;

        gameState = new Dictionary<Player, BoardState>();
        // create initial board states for other players before creating local dominos
        foreach (Player player in players)
        {
            gameState.Add(player, new BoardState(prefabData));
        }

        // create actual dominoes, and Q up rpcs for everyone to get accurate position/rotation

        localDominoes = new Domino[prefabData.Length];

        List<int> possibleIndex = new List<int>(localDominoes.Length);

        float[] possibleRotations = new float[] { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };

        for (int i = 0; i < possibleIndex.Capacity; i++)
        {
            possibleIndex.Add(i);
        }

        // Create random domino in random order, but store in localDominoes in the correct order
        // (so it matches the order in prefabData. This way domino IDs are consistent for all players
        for (int i = 0; i < localDominoes.Length; i++)
        {
            int randomIndex = Random.Range(0, possibleIndex.Count);
            int randomRotation = Random.Range(0, 8);
            Domino t = Instantiate(prefabData[possibleIndex[randomIndex]]);

            t.ID = possibleIndex[randomIndex];
            localDominoes[t.ID] = t;

            // Debug.LogWarning("Domino ID for " + t.gameObject.name + " set as: " + t.ID);
            t.transform.Rotate(0f, 0f, possibleRotations[randomRotation]);
            dominoTrayRef.AddDominoToTray(t);
            photonView.RPC(PhotonProperty.UpdateDominoPosition, RpcTarget.All, PhotonNetwork.LocalPlayer, t.ID, t.transform.position);
            photonView.RPC(PhotonProperty.UpdateDominoRotation, RpcTarget.All, PhotonNetwork.LocalPlayer, t.ID, t.transform.rotation);

            possibleIndex.RemoveAt(randomIndex);
        }

        // Create domino "board" where pieces are... placed
        currentGameBoard = Instantiate(GlobalGameData.Instance.Levels[level].gameBoard, gameplayContainer);
        timer.SetRoundTimer(GlobalGameData.Instance.Levels[level].RoundDuration);
        playerList.InitializeList();
    }

    [PunRPC]
    public void RPC_UpdateDominoPosition(Player targetPlayer, int dominoID, Vector3 newPosition)
    {
        gameState[targetPlayer].Dominoes[dominoID].position = newPosition;
        Debug.Log("Player " + targetPlayer + " has moved Domino: " + localDominoes[dominoID] + " to new position: " + newPosition);
    }

    [PunRPC]
    public void RPC_UpdateDominoRotation(Player targetPlayer, int dominoID, Quaternion newRotation)
    {
        gameState[targetPlayer].Dominoes[dominoID].rotation = newRotation;
        Debug.Log("Player " + targetPlayer + " has rotated Domino: " + localDominoes[dominoID] + " to new rotation: " + newRotation);
    }

    [PunRPC]
    public void RPC_ResetBoards()
    {
        enableInput = true;
        currentRound++;
        roundTextDisplay.text = (currentRound + 1).ToString();

        debriefTabGroup.OnTabSelected(defaultTab);

        scoreBoardVisual.SetActive(false);

        // "reset" the selectable squares so they can be usable again 
        // (their box colliders are disabled where the dominos sat for the round)
        currentGameBoard.ResetLocations();

        Domino.Clear();
        DominoTray.ClearDominoTray();

        float[] possibleRotations = new float[] { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };

        // move the dominoes back to the tray
        foreach (Domino domino in localDominoes)
        {
            int randomRotation = Random.Range(0, 8);

            domino.ResetDomino();
            domino.transform.Rotate(0f, 0f, possibleRotations[randomRotation]);
            DominoTray.AddDominoToTray(domino);
        }

        if (PhotonNetwork.LocalPlayer != currentLeader)
            gameplayContainer.gameObject.SetActive(false);
    }

    [PunRPC]
    public void RPC_SendReadyCheck(Player player, bool newState)
    {
        if (IsGameLeader)
        {
            if (playerList.UpdatePlayerCheck(player, newState))
            {
                timer.FinishedEarly();
            }
        }
        else
            playerList.UpdatePlayerCheck(player, newState);
    }

    #endregion
}
