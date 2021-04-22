using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject[] disableForWebGL;

    [SerializeField] GameObject playerNamePromptPanel;

    private bool firstTime = false;
    public bool FirstTimePlayer => firstTime;

    OptionsMenu options;

    private void Start()
    {
        options = GetComponent<OptionsMenu>();

        if (!PlayerPrefs.HasKey("NickName"))
        {
            firstTime = true;
            playerNamePromptPanel.SetActive(true);
            //   string storedName = PlayerPrefs.GetString("NickName");
            //   if (storedName == "")
            //       PhotonNetwork.NickName = "Player " + Random.Range(0, 1000);
            //   else
            //       PhotonNetwork.NickName = storedName;
        }
        else
            PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");

        #region WebGL housekeeping
#if UNITY_WEBGL
        foreach (GameObject item in disableForWebGL)
        {
            item.SetActive(false);
        }
#endif
        #endregion
    }

    [SerializeField] Text newNameField;
    public void InitializePlayerNickName()
    {
        options.SetPlayerNickName(newNameField.text);

        playerNamePromptPanel.SetActive(false);
    }
}
