using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for holding the string info for all used photon room and player property strings. 
/// </summary>
public static class PhotonProperty
{
    // Room Properties (maybe also use as player properties?)
    public static readonly string DominoCount = "DominoCount";
    public static readonly string DominoIDS = "DominoIDs";
    public static readonly string DominoPoisitions = "DominoPositions";
    public static readonly string DominoRotations = "DominoRotations";

    public static readonly string CurrentRound = "CurrentRound";
    public static readonly string RoundScores = "RoundScores";
    public static readonly string TotalScores = "TotalScores";


    // Player properties
    public static readonly string PlayerScore = "PlayerScore";
}
