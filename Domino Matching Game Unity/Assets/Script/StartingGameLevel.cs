using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingGameLevel : MonoBehaviour
{
    public static int startingLevel = 0;

    public static Player designatedLeader;

    public static List<Player> observingPlayers;

    public static List<Player> activePlayers;

    public void HostSetLevel(int level)
    {
          startingLevel = level;
    }
}
