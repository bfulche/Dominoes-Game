using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1/13. Was going to use this to "fix" inability to slot into a cell during future rounds, but found alternative fix.
/// This script could be attached to each cell though and used as part of a debug button to "reset" all cells (?)
/// not needed at this time I think
/// </summary>
public class Cell : MonoBehaviour
{
    private static List<Cell> cells;
    BoxCollider cellWalls;
    private void Start()
    {
        cellWalls = GetComponent<BoxCollider>();
        cells.Add(this);
    }
    public static void EnableCells()
    {
        foreach (Cell cell in cells)
        {
            cell.cellWalls.enabled = true;
        }
    }

    private void OnDisable()
    {
        cells.Remove(this);
    }
}
