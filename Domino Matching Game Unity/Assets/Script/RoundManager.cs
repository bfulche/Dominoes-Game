using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Timer))]
public class RoundManager : MonoBehaviourPunCallbacks
{
    Timer timer;
    [SerializeField] Button startRoundButton;
    int currentRoundScore = 0;
    int currentRound;

    private int currentRoundNumber;                 // The current round number, an int between 1-3
    public Text roundDisplay;                       // The UI text element displaying the current round

    private static readonly int totalRounds = 3;    // will always have 3 rounds to a level.

    [SerializeField] ScoreBoardMatrix scoreBoard;
    Dictionary<Player, int> playerScores;   // attempt to prevent duplicate scores being sent
    InputManager inputManager;
    bool nextLevel = false;

    public ScoreBoardMatrix ScoreBoard => scoreBoard;

    // Start is called before the first frame update
    void Start()
    {
        playerScores = new Dictionary<Player, int>();
        timer = GetComponent<Timer>();
        timer.timerDone += DisplayScore;
        inputManager = GetComponent<InputManager>();
        if (!PhotonNetwork.IsMasterClient)
        {
            // only show button for host
            startRoundButton.gameObject.SetActive(false);
        }

        currentRound = 0;   // start at the first round
        currentRoundNumber = currentRound + 1;      // Turning currentRoundNumber to 1 for UI display purposes
        roundDisplay.text = currentRoundNumber.ToString();

        Debug.Log("currentRoundNumber is " + currentRoundNumber);
    }

 //   private void Update()
 //   {
 //       currentRoundNumber = currentRound + 1;
 //       roundDisplay.text = currentRoundNumber.ToString();
 //   }


    /// <summary>
    /// not sure why but players are sending score multiple times currently
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="changedProps"></param>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log("Current # of keys in playerScores: " + playerScores.Count);

        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (targetPlayer == PhotonNetwork.MasterClient) // don't count host's score
            return;

    //    if (!playerScores.ContainsKey(targetPlayer))
     //   {
            // we don't have this player as a key yet. So this is the first time score being sent
            int playerScore = (int)changedProps[PhotonProperty.PlayerScore];
            currentRoundScore += playerScore;
            playerScores[targetPlayer] = playerScore;   // add player to our dictionary
            Debug.Log("Round score for player:" + targetPlayer.NickName + playerScore + ".Round Score after update: " + currentRoundScore);
 //       }
    }

    public void OnClick()
    {
        // only host can click this.

        // prevent host from being able to move tiles. after stating "ready"
        inputManager.enabled = false;

        // hide begin round button
        startRoundButton.gameObject.SetActive(false);

        //  Send host's current board state.
        SendBoardState();

        // Send message to other clients that round is starting.
        timer.photonView.RPC("StartTimer", RpcTarget.All);
        //start round timer

        timer.StartTimer();


        playerScores.Clear();   // should remove all keys in dictionary?
                                                        // not sure if this is part of the client-side bug
                                                        // for round 2/3 scoring showing up as 0

      //  playerScores.Clear();   // ready for new round
    }

    private void DisplayScore()
    {
        StartCoroutine(DisplayScoreAfterSeconds());
    }

    IEnumerator DisplayScoreAfterSeconds()
    {                                       // 12/26
        yield return new WaitForSeconds(5); // artificial delay before working with score values.
                                            // delay begins after round timer ends. 
                                            // probably want to give some feedback on UI that score is
                                            // loading or something.

        int score = 0;
        foreach (int value in playerScores.Values)
        {
            score += value;
        }

        scoreBoard.UpdateLocalScoreBoard(score, currentRound);
        scoreBoard.photonView.RPC("ShowScorePanel", RpcTarget.All);
        //scoreBoard.ShowScorePanel();

        PrepareForNextRound();
    }

    private void PrepareForNextRound()
    {
        currentRound++;

        currentRoundNumber = currentRound + 1;
        roundDisplay.text = currentRoundNumber.ToString();

        currentRoundScore = 0;
        if (currentRound < totalRounds)
        {
            // clean up board state to redo round.
            Tile.ResetTiles();
            // re-enable host's start round button
            if (PhotonNetwork.IsMasterClient)
                startRoundButton.gameObject.SetActive(true);

            inputManager.enabled = true;
            scoreBoard.NextRoundButton.text = "To Round " + (currentRound + 1).ToString();
        }
        else
        {
            // final round completed. Let level manager know to start next level.
            nextLevel = true;
            inputManager.enabled = true;
            // hide next round button
            ScoreBoard.NextRoundButton.gameObject.SetActive(false);
            // show main menu button
            ScoreBoard.MainMenuButton.gameObject.SetActive(true);
          //  scoreBoard.NextRoundButton.text = "Next Level";
            currentRound = 0;
        }
    }


    public void NextRound()
    {
        if (nextLevel)
        {
            nextLevel = false;
            LevelManager.Instance.photonView.RPC("LoadNextLevel", RpcTarget.All);
            //  LevelManager.Instance.LoadNextLevel();
        }

        scoreBoard.photonView.RPC("HideScorePanel", RpcTarget.All);
    }

    private void SendBoardState()
    {
        if (!PhotonNetwork.IsMasterClient)  // should never happen. If it did. Logic is wrong somewhere
            return;

        Tile[] tiles;

        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();

        tiles = GameObject.FindObjectsOfType<Tile>();
        tiles = SortTilesByID(tiles);
        // Temporary arrays to store each tile's data
        int length = tiles.Length;                          // # of tiles on board
        int[] IDs = new int[length];                        // each tile's ID
        Vector3[] positions = new Vector3[length];          // each tile's world position as Vector3
        Quaternion[] rotations = new Quaternion[length];    // each tile's rotation as a Quaternion

        // populate arrays
        for (int i = 0; i < tiles.Length; i++)
        {

            IDs[i] = tiles[i].ID;
            positions[i] = tiles[i].transform.position;
            rotations[i] = tiles[i].transform.rotation;
        }

        // Adding our arrays as custom properties to our Photon-Hashtable object.
        // Each string is the hashtable "key". The specified key will point to the assigned value.
        roomProperties[PhotonProperty.DominoCount] = length;
        roomProperties[PhotonProperty.DominoIDS] = IDs as int[];
        roomProperties[PhotonProperty.DominoPoisitions] = positions;
        roomProperties[PhotonProperty.DominoRotations] = rotations;

        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }

    // brute force sort. Definitely a more efficient way
    private Tile[] SortTilesByID(Tile[] tiles)
    {
        Tile[] sortedTiles = new Tile[tiles.Length];
        int currentIndex = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].ID == currentIndex)
            {
                sortedTiles[currentIndex] = tiles[i];
                currentIndex++;
                i = -1; // reset loop counter so we start at beginning of array
            }
        }

        return sortedTiles;
    }

}
