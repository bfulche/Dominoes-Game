﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Crfedit to https://www.youtube.com/watch?v=lT-SRLKUe5k for taking screenshots from in game camera
/// </summary>
public class ScreenshotHandler : MonoBehaviour
{
    private static ScreenshotHandler instance;

    private Camera myCamera;
    private bool takeScreenshotOnNextFrame;

    private void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
    }

    private void OnPostRender()
    {
        if (takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot.png",byteArray);
            Debug.Log("Saved CameraScreenshot.png");

            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
        }
    }

    private void TakeScreenshot(int width, int height)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
    }

    /// <summary>
    /// When calling function."To take a complete screenshot use Screen.width, Screen.height"
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void TakeScreenShot_Static(int width, int height)
    {
        instance.TakeScreenshot(width, height);
       
    }
}
