using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitVolumeSlider : MonoBehaviour
{
    bool initialized = false;

    [SerializeField] private string volumeSetting;

    [SerializeField] Slider volumeSlider;
    private void OnEnable()
    {
        if (!initialized)
        {
            volumeSlider.value = PlayerPrefs.GetFloat(volumeSetting, 1f);

            initialized = true;
        }
    }
}
