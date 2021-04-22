using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameData : MonoBehaviour
{
    #region Editor setup stuff
    [SerializeField] LevelData[] availableLevels;

    #endregion
    // Start is called before the first frame update
    #region Singleton stuff
    private static GlobalGameData _instance;

    public static GlobalGameData Instance => _instance;

    #endregion

    #region private global data available at run-time

    private Dictionary<int, LevelData> _levels;

    #endregion


    #region global data available at run-time accessors

    public Dictionary<int, LevelData> Levels => _levels; // might have explicit function
                                    // to request by key rather than just returning dictionary object


    #endregion

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            Init();
        }
        else
            Destroy(this);
    }

    private void Init()
    {

        // build level data dictionary
        _levels = new Dictionary<int, LevelData>();

        for (int i = 0; i < availableLevels.Length; i++)
        {
            _levels.Add(i, availableLevels[i]);
        }
    }

    #region Get() functions

    public LevelData GetLevelData(int index)
    {
        // level indexes are sequential if it's less than count then we're good.
        if (index < _levels.Count)
            return _levels[index];
        else
        {
            Debug.LogError("Invalid index: " + index + " requested. Current level count: " + _levels.Count);
            return null;
        }
    }

    #endregion
}
