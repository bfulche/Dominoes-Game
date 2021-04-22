using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectHandler : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadNewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }
}
