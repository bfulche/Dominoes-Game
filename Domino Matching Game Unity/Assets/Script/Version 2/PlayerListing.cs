using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour
{
    [SerializeField] Image playerReadyCheck;
    private bool ready = false;

    public void Ready()
    {
        ready = true;
        playerReadyCheck.enabled = true;
    }

    public void NotReady()
    {
        ready = false;
        playerReadyCheck.enabled = false;
    }
    

}
