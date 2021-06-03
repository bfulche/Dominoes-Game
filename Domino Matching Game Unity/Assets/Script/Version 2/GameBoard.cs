using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField] SelectableLocation[] locations;

    public void ResetLocations()
    {
        foreach (SelectableLocation location in locations)
        {
            location.MarkValid();
        }
    }
}
