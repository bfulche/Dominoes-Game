using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReadyCheck : MonoBehaviour
{
    [SerializeField] Image readyIcon;
    Player assignedPlayer;

    private bool ready = false;

    public bool Ready => ready;

    public void SetPlayer(Player player)
    {
        assignedPlayer = player;
    }

    public void UpdateState(bool newState)
    {
        ready = newState;
        readyIcon.enabled = newState;
    }
}
