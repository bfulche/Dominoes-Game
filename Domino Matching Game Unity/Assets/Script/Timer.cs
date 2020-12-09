using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//This is the timer for the 2 minutes for each round. It's also used in the Debrief scenes although it doesn't need to be
public class Timer : MonoBehaviour
{
    public float timeRemaining = 2;
    public bool timerIsRunning = false;
    public Text timeText;

    // alerts subscribed functions when tmier is up
    public delegate void TimerUp();
    public TimerUp timerDone;

    private void Start()
    {
        timerIsRunning = true;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                Debug.Log("Time's Up!");

                timerDone?.Invoke();  //let subscribed functions know time is up.

                // need some way of confirming data has been updated, and players finished uploading data to server before switching scenes
                // I think score calculating should be at the start of the next scene? Is there a benefit to calculating scores before
                // moving to next scene? Personal preference?

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }


    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadScene("RoundThree");
    }

    public void FirstScene()
    {
        SceneManager.LoadScene(0);
    }
}
