using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for holding the string info for all used photon room and 
/// player property strings. 
/// </summary>
public static class PhotonProperty
{
    // Room Properties (maybe also use as player properties?)
    public static readonly string DominoCount = "DominoCount";
    public static readonly string DominoIDS = "DominoIDs";
    public static readonly string DominoPoisitions = "DominoPositions";
    public static readonly string DominoRotations = "DominoRotations";
    public static readonly string HostScreenShot = "HostScreenShot";

    public static readonly string CurrentRound = "CurrentRound";
    public static readonly string RoundScores = "RoundScores";
    public static readonly string TotalScores = "TotalScores";


    // Player properties
    public static readonly string PlayerScore = "PlayerScore";

    // 4/22: RPC function names. Will later update older RPCs to new naming convention and add to this list
    public static readonly string ChangeLevelPreview = "RPC_ChangeLevelPreview";
    public static readonly string UpdateLeaderText = "RPC_UpdateLeaderText";
    public static readonly string UpdatePlayerList = "RPC_UpdatePlayerList";
    public static readonly string UpdateDominoPosition = "RPC_UpdateDominoPosition";
    public static readonly string UpdateDominoRotation = "RPC_UpdateDominoRotation";
    public static readonly string UpdatePlayerObservingState = "RPC_UpdatePlayerObservingState";
    public static readonly string CreateDominoes = "RPC_CreateDominoes";
    public static readonly string UpdateComparisonImages = "RPC_UpdateComparisonImages";
    public static readonly string StartTimer = "RPC_StartTimer";
    public static readonly string ResetBoards = "RPC_ResetBoards";
    public static readonly string SendReadyCheck = "RPC_SendReadyCheck";
}
