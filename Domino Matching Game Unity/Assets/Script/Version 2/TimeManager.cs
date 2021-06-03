using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviourPun
{

    [SerializeField] float startTime = 5f;
    private float timeRemaining = 2f;
    public bool timerIsRunning = false;
    public TMP_Text timeText;

  //  public TMP_Text senderRules;
  //  public TMP_Text receiverRules;

    // alerts subscribed functions when timer is up
    public delegate void TimerUp();
    public TimerUp timerDone;
 //   [SerializeField] GameObject toScoreboardButton;

    private int timeExtensionsCount = 0;    // tracks how many times the leader extended the timer in a round
    // Start is called before the first frame update

    public void HostSendTimerStart()
    {
        Texture2D newTexture = new Texture2D(240, 160);
        RenderTexture.active = localTexture;
        newTexture.ReadPixels(new Rect(0, 0, localTexture.width, localTexture.height), 0, 0);
        newTexture.Apply();

        this.photonView.RPC("SetMimicTexture", RpcTarget.All, newTexture.EncodeToPNG());
        this.photonView.RPC("StartTimer", RpcTarget.All);
    }

    [SerializeField] RawImage tempTestPicSend;
    [SerializeField] RenderTexture localTexture;
    
    [PunRPC]
    public void SetMimicTexture(byte[] receivedTexture)
    {
        Texture2D newTexture = new Texture2D(1, 1);
        newTexture.LoadImage(receivedTexture);
        tempTestPicSend.texture = newTexture;
       // tempTestPicSend.gameObject.SetActive(true);
    }

    [PunRPC]
    public void RPC_StartTimer()
    {
        GameplayManager.Instance.ShowGamePlayContainer();
        timeRemaining = startTime;
        timerIsRunning = true;
        timeText.gameObject.SetActive(true);

     //   LevelManager.Instance.LoadMimicLevel();
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

             //   GameplayManager.Instance.InputEnabled = false;

                if (GameplayManager.Instance.IsGameLeader)
                {

                    this.photonView.RPC("TimerDone", RpcTarget.All);

                }
                // timerDone?.Invoke();  //let subscribed functions know time is up.

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

    [PunRPC]
    public void TimerDone()
    {
        timeText.gameObject.SetActive(false);
        timerDone?.Invoke();
    }


    [PunRPC]
    public void FinishedEarly()
    {
        timeRemaining = 0;
    }


    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

 //   public void NextScene()
 //   {
 //       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
 //       //SceneManager.LoadScene("RoundThree");
 //   }
 //
 //   public void FirstScene()
 //   {
 //       SceneManager.LoadScene(0);
 //   }
}
