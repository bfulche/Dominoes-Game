using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Automatically force mutes the music mixer. When leaving the scene, the original values are reinstated
/// </summary>
public class AutoMuteMusic : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.ForceMuteMusic();
    }

    private void OnDisable()
    {
        AudioManager.Instance.RestoreMusic();
    }
}
