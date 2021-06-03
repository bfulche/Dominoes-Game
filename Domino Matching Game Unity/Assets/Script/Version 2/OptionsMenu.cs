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
        float newVolume = Mathf.Log10(sliderValue) * 20;
        mixer.SetFloat("Master", newVolume);
        AudioManager.Instance.Play("RotateDomino");

        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }

    public void SetSoundVolume(float sliderValue)
    {
        float newVolume = Mathf.Log10(sliderValue) * 20;
        mixer.SetFloat("Sound", newVolume);
        AudioManager.Instance.Play("RotateDomino");

        PlayerPrefs.SetFloat("SoundVolume", sliderValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        float newVolume = Mathf.Log10(sliderValue) * 20;
        mixer.SetFloat("Music", newVolume);

        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    #endregion

    #region Photon/Multiplayer options

    [SerializeField] TMP_Text playerNameText;
    [SerializeField] Text changeNameField;

    private void InitializePlayerInfoOptions()
    {
        playerNameText.text = PlayerPrefs.GetString("NickName");

        float m = Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume", 1f)) * 20;
        float s = Mathf.Log10(PlayerPrefs.GetFloat("SoundVolume", 1f)) * 20;
        float u = Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume", 1f)) * 20;

        mixer.SetFloat("Master", m);
        mixer.SetFloat("Sound", s);
        mixer.SetFloat("Music", u);
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
