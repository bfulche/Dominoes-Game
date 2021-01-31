using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ChatLogger : MonoBehaviour
{
    private static string filePath;
    

    private static ChatLogger _instance;

    public static ChatLogger Instance => _instance;

  //  private bool addHeader = false;
    void Start()
    {
        _instance = this;
        
        // Note: Application.persistentDataPath on WebGL is not permanent. So "saving" a file like this won't work for web build.
        // Ted mentioned EnTeam has drop box. so hopefully there's a way to send the "file" to a specified dropbox before the game quits.
          filePath = Application.persistentDataPath + "/ChatLog.csv";
        
     //   filePath = "C:/Users/Alex/Desktop/ChatLog.csv";

       
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
            sw.WriteLine("Chat Box , Time , Player, Message");
        }
    }

    public void Log(string targetBox, string player, string timeStamp, string message)
    {
      //  if (addHeader)
      //  {
      //      AddHeader();
      //      addHeader = false;
      //  }
      //
        targetBox = targetBox.Replace(",", "(comma)");
        player = player.Replace(",", "(comma)");
        message = message.Replace(",", "(comma)");

        //  newMessage.text = "[" + timeStamp + "]" + "[" + player + "]: " + message;
        string newLine = targetBox + "," + "[" + timeStamp + "]" + "," + "[" + player + "]" + "," + message;

        using (StreamWriter sw = new StreamWriter(filePath, true))
        {
            sw.WriteLine(newLine);
        }
    }
}
