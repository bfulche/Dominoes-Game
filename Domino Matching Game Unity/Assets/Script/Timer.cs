using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

//This is the timer for the 2 minutes for each round. It's also used in the Debrief scenes although it doesn't need to be
public class Timer : MonoBehaviourPun
{
    [SerializeField] float startTime = 5f;
    private float timeRemaining = 2f;
    public bool timerIsRunning = false;
    public Text timeText;

    // alerts subscribed functions when timer is up
    public delegate void TimerUp();
    public TimerUp timerDone;

    [SerializeField] GameObject toScoreboardButton;

    private void Start()
    {
       // timerIsRunning = true;
        toScoreboardButton.SetActive(false);
    }

    [PunRPC]
    public void StartTimer()
    {
        timeRemaining = startTime;
        timerIsRunning = true;
    }

    public void SetRoundTimer(float duration)
    {
        startTime = duration;
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

                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

                timerDone?.Invoke();  //let subscribed functions know time is up.

                // only allows host to host (master client) to view/press button to go to scoreboard
             //   if (PhotonNetwork.IsMasterClient)
             //   {
             //       toScoreboardButton.SetActive(true);
             //   }

                // need some way of confirming data has been updated, and players finished uploading data to server before switching scenes
                // I think score calculating should be at the start of the next scene? Is there a benefit to calculating scores before
                // moving to next scene? Personal preference?

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
