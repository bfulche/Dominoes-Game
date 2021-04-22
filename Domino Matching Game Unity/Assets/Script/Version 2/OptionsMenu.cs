using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] GameObject optionsPanel;
    [SerializeField] AudioMixer mixer;

    private void Start()
    {
        InitializePlayerInfoOptions();
    }

    #region Audio Options
    public void SetMasterVolume(float sliderValue)
    {
        mixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
        AudioManager.Instance.Play("RotateDomino");
    }

    public void SetSoundVolume(float sliderValue)
    {
        mixer.SetFloat("Sound", Mathf.Log10(sliderValue) * 20);
        AudioManager.Instance.Play("RotateDomino");
    }

    public void SetMusicVolume(float sliderValue)
    {
        mixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
    }

    #endregion

    #region Photon/Multiplayer options

    [SerializeField] TMP_Text playerNameText;
    [SerializeField] Text changeNameField;

    private void InitializePlayerInfoOptions()
    {
        playerNameText.text = PlayerPrefs.GetString("NickName");
    }

    public void ChangePlayerName()
    {
        SetPlayerNickName(changeNameField.text);
    }

    public void SetPlayerNickName(string newName)
    {
        PlayerPrefs.SetString("NickName", newName);
        PhotonNetwork.NickName = newName;

        playerNameText.text = newName;
    }

    #endregion
}
