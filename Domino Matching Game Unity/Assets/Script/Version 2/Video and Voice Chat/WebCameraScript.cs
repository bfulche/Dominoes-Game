using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCameraScript : MonoBehaviour
{
    // 5/1 notes
    // works to stream a webcam output to a RawImage. 
    // If a camera is already in use with another program (i.e Zoom), then the game cannot 
    // use that camera as a source. This would cause conflicts with people starting meetings in google/zoom
    // ahead of time... and then wanting to jump into the game.


    static WebCamTexture webCam;

    public RawImage background;

    private Texture defaultBackground;

    // Start is called before the first frame update
    void Start()
    {
        if (webCam == null)
            webCam = new WebCamTexture();

        defaultBackground = background.texture;

        webCam.Play();
        background.texture = webCam;
     //   GetComponent<Renderer>().material.mainTexture = webCam;
     //
     //   if (!webCam.isPlaying)
     //       webCam.Play();


    }
}
