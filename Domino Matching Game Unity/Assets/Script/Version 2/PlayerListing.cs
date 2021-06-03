using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour
{
    [SerializeField] Image observerIcon;
    Player thisPlayer;

    public void SetPlayer(Player trackedPlayer)
    {
        thisPlayer = trackedPlayer;

        thisPlayer.OnTagChanged += Observing;

        if (trackedPlayer.TagObject != null)
            Observing(trackedPlayer.TagObject);
    }

    /// <summary>
    /// For this project, player's tagObject will always indicate whether or not the player 
    /// is an observer. Always treating the object as a boolean.
    /// </summary>
    /// <param name="newState"></param>
    public void Observing(object newState)
    {
        observerIcon.enabled = (bool)newState;
    }

    private void OnDisable()
    {
        thisPlayer.OnTagChanged -= Observing;
    }
}
