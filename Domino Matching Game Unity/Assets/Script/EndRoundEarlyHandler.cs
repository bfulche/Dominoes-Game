using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Deals with players that are quick to get their boards set and want to end the round early
/// </summary>
public class EndRoundEarlyHandler : MonoBehaviourPun
{
    [SerializeField] Timer timerScript;

    [SerializeField] Button endRoundButton;

    [SerializeField] Text endRoundButtonText; // if host. says end round. if player, says Ready or Done.

    Dictionary<Player, bool> playerReadyChecks;

    public Button ReadyCheck => endRoundButton;

    // Start is called before the first frame update
    void Start()
    {
        // only bother if we're the host
        if (GameplayManager.Instance.IsGameLeader)
        {
            endRoundButtonText.text = "End Round";
            endRoundButton.interactable = false;

            // setup player dictionary
            playerReadyChecks = new Dictionary<Player, bool>();
            StartNewReadyCheck();
        }
        else
        {
            endRoundButtonText.text = "Submit";
        }

        timerScript.timerDone += ResetReadyButton;
    }

    private void StartNewReadyCheck()
    {
        Player[] playerList = PhotonNetwork.PlayerListOthers; // excludes calling player aka host

        if(playerList.Length == 0)
        {
            // only host in the room. Probably playing in editor. Enable button now and return
            endRoundButton.interactable = true;
            return;
        }

        // initialize each player as "not ready" to end round early
        foreach (Player player in playerList)
        {
            playerReadyChecks.Add(player, false);
        }
    }

    /// <summary>
    /// Only the host should ever receive this rpc (RPCTarget.MasterClient)
    /// </summary>
    /// <param name="readyPlayer"></param>
    [PunRPC]
    public void UpdateReadyCheck(Player readyPlayer)
    {
        playerReadyChecks[readyPlayer] = true;

        if(playerReadyChecks.ContainsValue(false))
        {
            // at least one player isn't ready. Return
            return;
        }

        // all players are ready
        endRoundButton.interactable = true;
    }


    public void ResetReadyButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            endRoundButtonText.text = "End Round";
            endRoundButton.interactable = false;

            // setup player dictionary
            ResetReadyCheck();
        }
        else
        {
            endRoundButton.interactable = true;
            endRoundButtonText.text = "Submit";
        }

        endRoundButton.gameObject.SetActive(false);
    }

    private void ResetReadyCheck()
    {
        playerReadyChecks.Clear();
        StartNewReadyCheck();
    }

    public void SendReadyState()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // host ending round early
            timerScript.photonView.RPC("FinishedEarly", RpcTarget.All);
       //     photonView.RPC("ResetReadyButton", RpcTarget.All);
        }
        else
        {
            // player sending update that they are "ready"
            Player thisPlayer = PhotonNetwork.LocalPlayer;
            this.photonView.RPC("UpdateReadyCheck", RpcTarget.MasterClient, thisPlayer);


            endRoundButton.interactable = false;
            endRoundButtonText.text = "Waiting on Leader";
            timerScript.FinishedEarly();
        }
    }

    [PunRPC]
    public void EnableReadyCheckButton()
    {
        endRoundButton.gameObject.SetActive(true);
    }

    public void HostStartedRound()
    {
        photonView.RPC("EnableReadyCheckButton", RpcTarget.All);
    }
}
