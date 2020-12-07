using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoardState : MonoBehaviour
{
    Tile[] tiles;

    ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
    // Start is called before the first frame update
    void Start()
    {
        //acquire personal tiles
        tiles = GameObject.FindObjectsOfType<Tile>();

        Timer timer = GameObject.FindObjectOfType<Timer>();

        if (timer != null)
            timer.timerDone += UpdateBoardState;

    }

    private void CalculateScore()
    {

    }

    private void InitializePlayerProperties()
    {
        // if host. Go ahead and initialize room's score related properties
        if (PhotonNetwork.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable initializeProperties = new ExitGames.Client.Photon.Hashtable();

            initializeProperties[PhotonProperty.RoundScores] = new int[] { -1, -1, -1 };   // scores start as -1 by default.
                                                                                           // -1 scores are left blank on score board
            initializeProperties[PhotonProperty.TotalScores] = new int[] { -1, -1, -1 };

            initializeProperties[PhotonProperty.CurrentRound] = 1;

            PhotonNetwork.CurrentRoom.SetCustomProperties(initializeProperties);
        }


        UpdateBoardState();
    }

    private void UpdateBoardState()
    {
        // Temporary arrays to store each tile's data
        int length = tiles.Length;                          // # of tiles on board
        int[] IDs = new int[length];                        // each tile's ID
        Vector3[] positions = new Vector3[length];          // each tile's world position as Vector3
        Quaternion[] rotations = new Quaternion[length];    // each tile's rotation as a Quaternion



        // populate arrays
        for (int i = 0; i < tiles.Length; i++)
        {

            IDs[i] = tiles[i].ID;
            positions[i] = tiles[i].transform.position;
            rotations[i] = tiles[i].transform.rotation;
        }

        // Adding our arrays as custom properties to our Photon-Hashtable object.
        // Each string is the hashtable "key". The specified key will point to the assigned value.
       customProperties[PhotonProperty.DominoCount] = length;
       customProperties[PhotonProperty.DominoIDS] = IDs as int[];
       customProperties[PhotonProperty.DominoPoisitions] = positions;
       customProperties[PhotonProperty.DominoRotations] = rotations;


        // Used in testing: initializing fake score to make sure scoreboard can pull property
        int tempScore = Random.Range(0, 3);
        customProperties[PhotonProperty.PlayerScore] = tempScore;


        // If host. Also set board state as room property so other players can compare to host
        if (PhotonNetwork.IsMasterClient)
        {
            customProperties[PhotonProperty.PlayerScore] = 0;
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);
        }
        // Setting our player property
        PhotonNetwork.SetPlayerCustomProperties(customProperties);
    }
}

/// <summary>
/// Could look into registering custom type with Photon to reduce number of properties.
/// </summary>
public class TileState
{

    int id;
    Vector3 position;
    Quaternion rotation;

    public int ID => id;
    public Vector3 Position => position;
    public Quaternion Rotation => rotation;

    public TileState(int _id, Vector3 _position, Quaternion _rotation)
    {
        id = _id;
        position = _position;
        rotation = _rotation;
    }
}
