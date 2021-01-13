using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Based on LevelData object currently reference - populates the gameplay scene with level data, and keeps track of rounds.
/// </summary>
public class LevelManager : MonoBehaviourPun
{
    [SerializeField] LevelData[] levelList;
    /* [SerializeField] */
    LevelData data;

    int currentLevel = 0;
    [SerializeField] Transform gameplayContainer;   // destroy all children of this object during scene clean up/ level switch
    RoundManager roundManager;

    private static LevelManager _instance;

    public static LevelManager Instance => _instance;
    private void Awake()
    {
        if (_instance != null)
            Destroy(this);
        else
            _instance = this;

        if (PhotonNetwork.IsMasterClient)
        {
            this.photonView.RPC("SetStartingLevel", RpcTarget.All, StartingGameLevel.startingLevel);
        }
    }
    [PunRPC]
    void SetStartingLevel(int level)
    {
        data = levelList[level];
        currentLevel = level;

        roundManager = GetComponent<RoundManager>();

        LoadNewLevel();
    }

    /// <summary>
    /// Populates gameplay scene based on currently set level data
    /// </summary>
    private void LoadNewLevel()
    {
        CleanCurrentLevel();
        Instantiate(data.CellVariant, gameplayContainer);

        PlayerHand playerHand = Instantiate(data.PlayerHand, gameplayContainer);

        int count = data.DominoCount;
        Timer t = FindObjectOfType<Timer>();

        t.SetRoundTimer(data.RoundDuration);

        roundManager.ScoreBoard.NewLevel(); // sets total level score to 0

        List<Transform> possiblePositions = playerHand.Slots.ToList();

        float[] possibleRotations = new float[] { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, possiblePositions.Count);
            int randomRotation = Random.Range(0, 8);

            Tile tile = Instantiate(data.TilePrefabs[i], possiblePositions[randomIndex].position, possiblePositions[randomIndex].rotation);
            tile.ID = i;

            tile.transform.Rotate(0f, 0f, possibleRotations[randomRotation]);
            tile.SetInitialPositionAndRotation(tile.transform.position, tile.transform.rotation);

            // remove used starting position from list so it can't be picked again by another tile
            possiblePositions.RemoveAt(randomIndex);
        }
    }
    [PunRPC]
    public void LoadNextLevel()
    {
        currentLevel++;


        if (currentLevel < levelList.Length)
        {
            data = levelList[currentLevel];
            LoadNewLevel();
        }
    }

    /// <summary>
    /// Used to skip to a specific level in the list
    /// </summary>
    /// <param name="newData"></param>
    public void SetLevel(int levelIndex)
    {
        if (levelIndex < levelList.Length)
        {
            data = levelList[levelIndex];
            LoadNewLevel();
        }
        else
            Debug.LogError("Unable to load level at index: " + levelIndex + ". Level does not exist.");

    }

    private void CleanCurrentLevel()
    {
        Tile.ClearTileList();   // clear tile list before destroying tiles/cells

        // remove irrelevant level objects
        int childrenCount = gameplayContainer.childCount;

        for (int i = 0; i < childrenCount; i++)
        {
            Destroy(gameplayContainer.GetChild(i).gameObject);
        }
    }
}
