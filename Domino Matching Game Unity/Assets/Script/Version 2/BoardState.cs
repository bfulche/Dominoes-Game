using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardState
{
    private DominoState[] dominoes;

    public DominoState[] Dominoes => dominoes;

    public BoardState(Domino[] trackedDominoes)
    {
        dominoes = new DominoState[trackedDominoes.Length];

        for (int i = 0; i < dominoes.Length; i++)
        {
            dominoes[i] = new DominoState(trackedDominoes[i].ID,
                                            trackedDominoes[i].transform.position,
                                            trackedDominoes[i].transform.rotation);
        }
    }

    // Compares two board states and returns an integer for every identical match
    // returns 0 if boards could not be compared
    public static int CompareBoards(BoardState board1, BoardState board2)
    {
        if (board1.dominoes.Length != board2.dominoes.Length)
            return 0;

        int score = 0;
        int length = board1.dominoes.Length;

        // loop through each domino, and check that position matches, then rotation. If yes to all add a point
        for (int i = 0; i < length; i++)
        {
            if (board1.dominoes[i].position == board2.dominoes[i].position)
            {
                // Check rotations are within acceptable range to be considered "equal"
                if (Quaternion.Angle(board1.dominoes[i].rotation, board2.dominoes[i].rotation) < 10f)
                {
                    score++;
                }       
            }     
        }
            


        return score;
    }
}

[System.Serializable]
public class DominoState
{
    public int id;

    public Vector3 position;

    public Quaternion rotation;

    public DominoState(int _id, Vector3 pos, Quaternion rot)
    {
        id = _id;
        position = pos;
        rotation = rot;
    }
}

