using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingGameLevel : MonoBehaviour
{
    public static int startingLevel = -1;


    public void SetStartingLevel(int level)
    {
        startingLevel = level;
    }
}
