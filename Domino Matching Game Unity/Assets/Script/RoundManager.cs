﻿using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Throwing a quick setup together to test comparison timing and scoring.
/// Attached to Manager object in hierarchy
/// </summary>
[RequireComponent(typeof(Timer))]
public class RoundManager : MonoBehaviourPunCallbacks
{
    Timer timer;
    [SerializeField] Button startRoundButton;
    int currentRoundScore = 0;
    [SerializeField] ScoreBoardMatrix scoreBoard;
    Dictionary<Player, int> playerScores;   // attempt to prevent duplicate scores being sent

    // Start is called before the first frame update
    void Start()
    {
        playerScores = new Dictionary<Player, int>();
        timer = GetComponent<Timer>();
        timer.timerDone += DisplayScore;
        if (!PhotonNetwork.IsMasterClient)
        {
            // only show button for host
            startRoundButton.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// not sure why but players are sending score multiple times currently
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="changedProps"></param>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        // We're the host, and we received our own property update. Don't count our score

        if (targetPlayer == PhotonNetwork.MasterClient) // don't count host score
            return;

        if (!playerScores.ContainsKey(targetPlayer))
        {
            // we don't have this player as a key yet. So this is the first time score being sent
            int playerScore = (int)changedProps[PhotonProperty.PlayerScore];
            currentRoundScore += playerScore;
            playerScores[targetPlayer] = playerScore;   // add player to our dictionary
            Debug.Log("Round score for player:" + targetPlayer.NickName + playerScore + ".Round Score after update: " + currentRoundScore);
        }
    }

    public void OnClick()
    {
        // only host can click this.

        // prevent host from being able to move tiles.
        GetComponent<InputManager>().enabled = false;

        //  Send host's current board state.
        SendBoardState();

        // Send message to other clients that round is starting.
        timer.photonView.RPC("StartTimer", RpcTarget.All);
        //start round timer
        
        timer.StartTimer();

        playerScores.Clear();   // ready for new round
    }

    private void DisplayScore()
    {
        StartCoroutine(DisplayScoreAfterSeconds());
    }

    IEnumerator DisplayScoreAfterSeconds()
    {
        yield return new WaitForSeconds(5);

        scoreBoard.UpdateLocalScoreBoard(currentRoundScore);
        scoreBoard.ShowScorePanel();
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