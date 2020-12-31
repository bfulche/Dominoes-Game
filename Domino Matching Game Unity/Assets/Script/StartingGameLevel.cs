using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingGameLevel : MonoBehaviour
{
    public static int startingLevel = 0;


    public void HostSetLevel(int level)
    {
          startingLevel = level;
    }
}
