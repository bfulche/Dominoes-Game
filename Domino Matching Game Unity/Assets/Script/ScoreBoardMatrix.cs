using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreBoardMatrix : MonoBehaviour
{
    // round scores - only ever 3 total rounds so this is simple enough
    [SerializeField] Text round1Score;
    [SerializeField] Text round2Score;
    [SerializeField] Text round3Score;

    // total scores
    [SerializeField] Text totalScore1;
    [SerializeField] Text totalScore2;
    [SerializeField] Text totalScore3;

    // player's personal scores could go here. no UI element for it yet

    private void Start()
    {
        UpdateScoreMatrix();


    }

    void UpdateScoreMatrix()
    {
        int[] roundScores = (int[])PhotonNetwork.CurrentRoom.CustomProperties[PhotonProperty.RoundScores];
        int[] totalScores = (int[])PhotonNetwork.CurrentRoom.CustomProperties[PhotonProperty.TotalScores];
        int currentRound = (int)PhotonNetwork.CurrentRoom.CustomProperties[PhotonProperty.CurrentRound];

        Player[] players = PhotonNetwork.PlayerList;
        int roundScore = 0;

        // add up all player's scores. Host score should be 0 at all times
        for (int i = 0; i < players.Length; i++)
        {
            roundScore += (int)players[i].CustomProperties[PhotonProperty.PlayerScore];
        }

        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();

        roundScores[currentRound - 1] = roundScore; // CurrentRound initially set to 1

        roomProperties[PhotonProperty.RoundScores] = roundScores;

        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties); // sends 


        // not clean. But basically leaving text field blank if round value isn't used yet ( <= -1)

        if (roundScores[0] > -1)
            round1Score.text = roundScores[0].ToString();
        if (roundScores[1] > -1)
            round2Score.text = roundScores[1].ToString();
        if (roundScores[2] > -1)
            round3Score.text = roundScores[2].ToString();

        if (totalScores[0] > -1)
            totalScore1.text = totalScores[0].ToString();
        if (totalScores[1] > -1)
            totalScore2.text = totalScores[1].ToString();
        if (totalScores[2] > -1)
            totalScore3.text = totalScores[2].ToString();
    }


}
