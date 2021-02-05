using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreBoardMatrix : MonoBehaviourPun
{
    int totalGameScoreTally = 0;

    [SerializeField] GameObject scorePanel;

    [SerializeField] Text[] roundScores;
    [SerializeField] Text[] totalScores;
    [SerializeField] Button buttonToNextRound;
    Text buttonToNextRoundText;

    [SerializeField] Button mainMenuButton;

  //  [SerializeField] Image hostImage, localImage;

    public Text NextRoundText => buttonToNextRoundText;
    // player's personal scores could go here. no UI element for it yet

    public Button NextRoundButton => buttonToNextRound;
    public Button MainMenuButton => mainMenuButton;

    private void Start()
    {
        buttonToNextRoundText = buttonToNextRound.GetComponentInChildren<Text>();
        HideScorePanel();
    }

    [PunRPC]
    public void ShowScorePanel()
    {
        scorePanel.SetActive(true);

        // only host should see button to next round
        if (PhotonNetwork.IsMasterClient)
            buttonToNextRound.gameObject.SetActive(true);
        else
            buttonToNextRound.gameObject.SetActive(false);
        
    }

    [PunRPC]
    public void HideScorePanel()
    {
        // clean up board state to redo round.
        Tile.ResetTiles();
        LevelManager.Instance.CleanMimic();
        scorePanel.SetActive(false);
        InputManager.Instance.enabled = true;
    }

    public void NewLevel()
    {
        totalGameScoreTally = 0;
    }

    public void UpdateLocalScoreBoard(int roundScore, int currentRound)
    {
        Debug.Log("Received Round Score: " + roundScore +". Received Current Round Integer: " + currentRound);

        roundScores[currentRound].text = roundScore.ToString();

        totalGameScoreTally += roundScore;
        totalScores[currentRound].text = totalGameScoreTally.ToString();

      //  hostImage.sprite = hostSprite;
      //  localImage.sprite = playerSprite;
       // currentRound++;
    }
    /*
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

    */
}
