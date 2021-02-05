using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Credit to https://www.youtube.com/watch?v=IRAeJgGkjHk for UI setup and initial code structure
/// 
/// Date/Timestamp related references
/// https://docs.microsoft.com/en-us/dotnet/api/system.datetime.tostring?view=net-5.0 
/// 
/// Hard-coding 3-chat box implementation. for now
/// Revisit for future projects if need to support more than 3 boxes.
/// </summary>
public class ChatHandler : MonoBehaviourPun
{
    string playerName;

    [SerializeField] GameObject chatPanel, textObject;

    [SerializeField] InputField inputField;

    [SerializeField] GameObject unreadNotificationmarker;

    public int maxMessages = 1000;

    [SerializeField] List<Message> messageList = new List<Message>();

    [SerializeField] string chatBoxName;
    void Start()
    {
        playerName = PhotonNetwork.LocalPlayer.NickName;
    }


    // Time stamp stuff. placing here for now I guess
    CultureInfo culture = new CultureInfo("en-US");

    void StressTest()
    {
        for (int i = 0; i < maxMessages; i++)
        {
            DateTime localDate = DateTime.Now;
            string timeStamp = localDate.ToString("t", culture);
            string[] parameters = new string[3] { playerName, timeStamp, ("This is message # " + (i + 1)) };
            this.photonView.RPC("SendChatMessage", RpcTarget.All, parameters);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (inputField.text != "" && inputField.gameObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                DateTime localDate = DateTime.Now;
                string timeStamp = localDate.ToString("t", culture);
                string[] parameters = new string[3] { playerName, timeStamp, inputField.text };
                this.photonView.RPC("SendChatMessage", RpcTarget.All, parameters);
                //   this.photonView.RPC(SendChatMessage(playerName, timeStamp, inputField.text));
                //    SendChatMessage(playerName, timeStamp, inputField.text);
                inputField.text = "";
                inputField.Select();
            }
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            StressTest();
        }

        //  if (!inputField.isFocused)
        //  {
        //      if (Input.GetKeyDown(KeyCode.Return))
        //      {
        //          if (inputField.text != null)
        //              SendChatMessage(inputField.text);
        //          //   SendChatMessage("You pressed the Enter! key");
        //      }
        //  }

    }

    [PunRPC]
    public void SendChatMessage(string player, string timeStamp, string message)
    {
        Message newMessage = new Message();

        //  newMessage.text = "[" + player + "]" + " " + timeStamp + message;
        newMessage.text = "[" + timeStamp + "]" + "[" + player + "]: " + message;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;

        messageList.Add(newMessage);

        // check if chatbox if chatbox is currently selected. Otherwise leave an unread notification
        if (!chatPanel.activeInHierarchy)
        {
            unreadNotificationmarker.SetActive(true);
        }

        ChatLogger.Instance.Log(chatBoxName, player, timeStamp, message);
    }
}
[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
}