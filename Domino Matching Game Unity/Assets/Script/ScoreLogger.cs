using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreLogger : MonoBehaviour
{

    private static string filePath;


    private static ScoreLogger _instance;

    public static ScoreLogger Instance => _instance;


    void Start()
    {
        _instance = this;

        filePath = Application.persistentDataPath + "/GameLog.csv";
        if (!File.Exists(filePath))
        {
            AddHeader();
        }
    }


    private static void AddHeader()
    {
        // create blank csv file with headers so we can append later       
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine("Room Name, Leader, Player, Round, Score");
        }
    }
    public void LogPlayerRoundScore(Player player,int round, int score)
    {
        //   string newLine = targetBox + "," + "[" + timeStamp + "]" + "," + "[" + player + "]" + "," + message;

        string newLine = 
            PhotonNetwork.CurrentRoom.Name + "," +
            PhotonNetwork.MasterClient.NickName + "," +
            player.NickName + "," +
            round + "," +
            score;
        // maybe add a line to show change between player's current score and previous round score

        using (StreamWriter sw = new StreamWriter(filePath, true))
        {
            sw.WriteLine(newLine);
        }
    }
}
