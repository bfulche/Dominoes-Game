using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//I'm not entirely sure about this script to be honest. I don't think it is being used in the current version, but I'm not positive.

public class MultiplayerSetting : MonoBehaviour
{

    public static MultiplayerSetting multiplayerSetting;

    public bool delayStart;
    public int maxPlayers;

    public int menuScene;
    public int multiplayerScene;

    private void Awake()
    {
        if(MultiplayerSetting.multiplayerSetting == null)
        {
            MultiplayerSetting.multiplayerSetting = this;
        }
        else
        {
            if(MultiplayerSetting.multiplayerSetting != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
