using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of currently selected domino, and enables/disables indicators of valid move 
/// positions (domino tray and the board "cells")
/// </summary>
public class GameControlsManager : MonoBehaviour
{
    private static GameControlsManager _instance;

    public static GameControlsManager Instance => _instance;

    Domino selectedDomino = null;

    public void SelectDomino(Domino newDomino)
    {
      //  selectedDomino?.DeSelected();

        selectedDomino = newDomino;
    //    newDomino.Selected();
    }
}
