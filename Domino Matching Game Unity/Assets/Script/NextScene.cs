using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextScene : MonoBehaviour
{

    public GameObject StartMenuCanvas;
    public GameObject P1Canvas;
    public GameObject BetweenCanvas;
    public GameObject P2Canvas;
    public GameObject ScoreCanvas;

   /* private void Start()
    {
        StartMenuCanvas.GetComponent<Image>().enabled = true;
        P1Canvas.GetComponent<Image>().enabled = false;
        BetweenCanvas.GetComponent<Image>().enabled = false;
        P2Canvas.GetComponent<Image>().enabled = false;
        ScoreCanvas.GetComponent<Image>().enabled = false;
    }


     public void PlayGame()
     {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
     }

     public void Back2()
     {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
     }
     public void Back()
     {
         SceneManager.LoadScene("Start");
     }*/
    public void ToP1()
    {
        StartMenuCanvas.SetActive(false);
        P1Canvas.SetActive(true);
    }

    public void ToBetween()
    {
        P1Canvas.SetActive(false);
        BetweenCanvas.SetActive(true);
    }

    public void ToP2()
    {
        BetweenCanvas.SetActive(false);
        P2Canvas.SetActive(true);
    }

    public void ToScore()
    {
        P2Canvas.SetActive(false);
        ScoreCanvas.SetActive(true);
    }

    public void ToMenu()
    {
        ScoreCanvas.SetActive(false);
        StartMenuCanvas.SetActive(true);
    }
}
