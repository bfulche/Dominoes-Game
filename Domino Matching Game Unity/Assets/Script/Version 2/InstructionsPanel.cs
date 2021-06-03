using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class InstructionsPanel : MonoBehaviour
{
    [SerializeField] RawImage videoPlane;

    [SerializeField] RenderTexture[] videoTextures;

    [SerializeField] string[] videoFileNames; // exclude .mp4 we'll add that in ourselves

    [SerializeField] TMP_Text instructionText;

    [SerializeField] TMP_Text instructionTitle;

    [SerializeField] [Multiline()] string[] instructions;

    [SerializeField] string[] titles;

    [SerializeField] VideoPlayer player;

    int current = 0;
    int max;
    // Start is called before the first frame update
    void Start()
    {
        max = videoTextures.Length - 1;

        player.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileNames[0] + ".mp4");

        UpdateTexture();
    }
    public void Previous()
    {
        if (current > 0)
        {
            current--;
        }
        else
            current = max;
        UpdateTexture();
    }

    public void Next()
    {
        if (current < max)
        {
            current++;
        }
        else
            current = 0;

        UpdateTexture();
    }

    private void ChangeVideo()
    {
        player.Stop();
        player.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileNames[current] + ".mp4");
        player.Play();
    }

    public void Stop()
    {
        player.Stop();
    }

    public void UpdateTexture()
    {
        // videoPlane.texture = videoTextures[current];
        ChangeVideo();

        instructionText.text = instructions[current];
        instructionTitle.text = titles[current];
    }
}
