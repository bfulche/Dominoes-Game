using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviourPun
{
    [SerializeField] List<ChatHandler> chatBoxes;


    public void SendMessage(ChatHandler targetChatBox)
    {
        int i = chatBoxes.IndexOf(targetChatBox);


    }
}
