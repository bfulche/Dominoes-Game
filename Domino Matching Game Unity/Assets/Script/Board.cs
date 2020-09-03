using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject mCellPrefab;

    [HideInInspector]
    public Cell[,] mAllCells = new Cell[5, 2];

    //Create
    public void Create()
    {
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                //Create Cell
                GameObject newCell = Instantiate(mCellPrefab, transform);

                //Position
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 100) + 262, (y * 100) + 384);

                //Setup
                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);
            }
        }

        //Color
        for (int x = 0; x < 5; x += 2)
        {
            for (int y = 0; y < 2; y++)
            {
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x + offset;

                mAllCells[finalX, y].GetComponent<Image>().color = new Color32(230, 220, 187, 255);
            }
        }
    }
}
