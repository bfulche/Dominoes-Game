using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] GameObject cellVariant; // holds the "cells layout" prefab used by this level
                                             // i.e. 4 cells in shape of square. 4 cells in + shape. etc

    [SerializeField] PlayerHand playerHandVariant;  // Starting Tile position prefab to used for this level (hand with 4 slots, vs 6, etc)

    [SerializeField] float roundDuration; // Is there a time where we'd want later rounds to last longer/shorter than previous?
                                          // check if round timer should or shouldn't be consistent each round for a level.

    public int DominoCount => tilePrefabs.Length;

    public Tile[] TilePrefabs => tilePrefabs;

    public GameObject CellVariant => cellVariant;

    public PlayerHand PlayerHand => playerHandVariant;

    public float RoundDuration => roundDuration;
}
