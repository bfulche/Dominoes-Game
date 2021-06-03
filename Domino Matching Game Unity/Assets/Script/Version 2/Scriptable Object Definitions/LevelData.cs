using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Data object containing all information needed to populate gameplay scene with level specific information.
/// i.e:
///     - # of tiles
///     - Domino prefabs to use ([blank/8] , [2/5], [1/3], etc)
///     - # of "cells" to place tiles in
///     - round duration (timer)
///     - 
/// </summary>
[CreateAssetMenu(fileName ="NewLevel",menuName ="ScriptableObjects/CreateNewLevel")]
public class LevelData : ScriptableObject
{
    [SerializeField] Tile[] tilePrefabs;

    [SerializeField] Domino[] dominoPrefabs;

    [SerializeField] GameObject cellVariant; // holds the "cells layout" prefab used by this level
                                             // i.e. 4 cells in shape of square. 4 cells in + shape. etc

    [SerializeField] GameBoard board;

    [SerializeField] PlayerHand playerHandVariant;  // Starting Tile position prefab to used for this level (hand with 4 slots, vs 6, etc)

    [SerializeField] float roundDuration = 0; // Is there a time where we'd want later rounds to last longer/shorter than previous?
                                          // check if round timer should or shouldn't be consistent each round for a level.

    [SerializeField] string levelName = "Nameless level";  // name appearing in level select dropdown
    [SerializeField] Sprite levelImage = null;  // level preview image appearing in game room setup

    [SerializeField] int randomizedDominoes = 0;  // determines how many dominos will be randomly placed and locked
                                            // for the leader each round

    [Multiline()]
    [SerializeField] string levelDescription = "No Description available";


    public int DominoCount => tilePrefabs.Length;
    public Tile[] TilePrefabs => tilePrefabs;
    public Domino[] DominoPrefabs => dominoPrefabs;
    public GameObject CellVariant => cellVariant;
    public GameBoard gameBoard => board;
    public PlayerHand PlayerHand => playerHandVariant;
    public float RoundDuration => roundDuration;
    public string LevelName => levelName;
    public Sprite LevelImage => levelImage;
    public string LevelDescription => levelDescription;

    public int RandomizedDominoes => randomizedDominoes;
}
