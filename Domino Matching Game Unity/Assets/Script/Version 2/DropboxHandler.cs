using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dropbox.Api;

public class DropboxHandler : MonoBehaviour
{
    private static readonly string dbToken = "QQqC7jiei0sAAAAAAAAAAcdn1R13aJThW3GOjdMfHZjJzu0arAG9pFBLUznbmWLA";

    private static DropboxHandler _instance;

    public static DropboxHandler Instance => _instance;

    private static Dictionary<int, GameLogData> gamesDictionary;

    public static Dictionary<int, GameLogData> GameHistory => gamesDictionary;

    [SerializeField] string gameName = "DigitalDominos";

    private static string gameFolderName;

    public static bool Loaded { get; private set; }

    public bool ignoreDropBox = true;
    // Start is called before the first frame update
    void Start()
    {
        if (ignoreDropBox)
            return;

        // Dropbox API isn't supported for Web Builds. Stop Initialization Scripts must check if WebGL build before calling Dropbox functions
        // or an error will be thrown. Returning early to avoid initializing something that can't be used by WebGL
#if UNITY_WEBGL
        return;
#endif
        gameFolderName = gameName;
        if (gamesDictionary == null)
            gamesDictionary = new Dictionary<int, GameLogData>();
        Loaded = false;
        GetGameHistoryList();

        DontDestroyOnLoad(gameObject);
    }

    // revisit implementation after testing if WebGL will still compile
    public static async void GetGameHistoryList()
    {
#if UNITY_WEBGL
        return;
#endif


        gamesDictionary.Clear();
        using (var dbx = new DropboxClient(dbToken))
        {
            string[] delims = new string[4] { ",", "\r\n", "\r", "\n" };
            // var list = await dbx.Files.ListFolderAsync("/Apps/EnTeamDigitalGamesAnalytics/" + gameFolderName,true);

            // find all files in gamelog data and add to temp list (building game data after acquiring both lists
            var gameList = await dbx.Files.ListFolderAsync("/Apps/EnTeamDigitalGamesAnalytics/" + gameFolderName + "/GameLogs");

            var chatList = await dbx.Files.ListFolderAsync("/Apps/EnTeamDigitalGamesAnalytics/" + gameFolderName + "/ChatLogs");

            if (gameList.Entries.Count == chatList.Entries.Count)
            {
                // lists match up as expected. Let's build our data
                for (int i = 0; i < gameList.Entries.Count; i++)
                {
                    string gameDataPath = gameList.Entries[i].PathLower;
                    string chatDataPath = chatList.Entries[i].PathLower;

                    using (var response = await dbx.Files.DownloadAsync(gameDataPath))
                    {
                        var s = response.GetContentAsStringAsync().Result;
                        string[] tokens = s.Split(delims, System.StringSplitOptions.None);




                        int playerCount;
                        string[] playerList;

                        if (!int.TryParse(tokens[12], out playerCount))
                        {
                            Debug.LogError("Invalid Player count at token index 12. Token = " + tokens[12]);
                        }
                        else
                        {
                            playerList = new string[playerCount];

                            for (int k = 0; k < playerCount; k++)
                            {
                                playerList[k] = tokens[13 + k];
                            }
                            // ignore first x tokens as they are the file headers.
                            gamesDictionary.Add(i, new GameLogData(gameDataPath, chatDataPath, tokens[6], tokens[7], tokens[8], tokens[9], tokens[10], tokens[11], playerCount, playerList));

                        }

                    }


                    //   gameDictionary.Add(i, new GameLogData(gameDataPath, chatDataPath));
                    Debug.Log("Added New Game to dictionary: Game Data = " + gameList.Entries[i].PathLower + " and Chat Data = " + chatList.Entries[i].PathLower);
                }
                Loaded = true;
            }

            // should only have 2 folders: GameLogs and ChatLogs
            //  foreach (var item in list.Entries.Where(i => i.IsFolder))
            //  {
            //      
            //      Debug.Log(string.Format("Directory {0}/", item.Name));
            //  }
            //  foreach (var item in list.Entries.Where(i => i.IsFile))
            //  {
            //      Debug.Log(item.PathLower);
            //      Debug.Log(string.Format("File {0,8} {1}", item.AsFile.Size, item.Name));
            //  }
        }
    }
}


[System.Serializable]
public class ChatLogData
{
    public List<ChatEntry> whatHappenedLog;
    public List<ChatEntry> soWhatLog;
    public List<ChatEntry> nowWhatLog;

    public ChatLogData()
    {
        whatHappenedLog = new List<ChatEntry>();
        soWhatLog = new List<ChatEntry>();
        nowWhatLog = new List<ChatEntry>();
    }
}

public class ChatEntry
{
    public string _chatbox;
    public string _timeStamp;
    public string _player;
    public string _message;

    public ChatEntry(string box, string time, string player, string note)
    {
        _chatbox = box;
        _timeStamp = time;
        _player = player;
        _message = note;
    }
}



[System.Serializable]
public class GameLogData
{
    string dataDirectory;
    string chatDirectory;

    string gameName, roomName, roundDuration, r1Score, r2Score, r3Score;

    int playerCount;
    string[] playerList;

    public string GameName => gameName;
    public string RoomName => roomName;
    public string RoundDuration => roundDuration;
    public string Round1Score => r1Score;
    public string Round2Score => r2Score;
    public string Round3Score => r3Score;

    public string DataDirectory => dataDirectory;

    public string ChatDirectory => chatDirectory;

    public GameLogData()
    {
        dataDirectory = null;
        chatDirectory = null;
    }

    public GameLogData(string game, string chat)
    {
        dataDirectory = game;
        chatDirectory = chat;
    }

    public GameLogData(string game, string chat, string gName, string rName, string duration, string r1, string r2, string r3, int count, string[] list)
    {
        dataDirectory = game;
        chatDirectory = chat;
        gameName = gName;
        roomName = rName;
        roundDuration = duration;
        r1Score = r1;
        r2Score = r2;
        r3Score = r3;
        playerCount = count;
        playerList = list;
    }
}
