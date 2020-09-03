using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoMan : MonoBehaviour
{
    //References
    public GameObject controller;
    public GameObject movePlate;

    //Positions
    private int xBoard = -1;
    private int yBoard = -1;

    //Sprite references
    public Sprite eightTwo, eightZero, fiveThree, oneFive, oneFour, zeroTwo;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        //take instantiate loaction and adjust the transformation
        setCoords();

        switch (this.name)
        {
            case "eightTwo": this.GetComponent<SpriteRenderer>().sprite = eightTwo; break;
            case "eightZero": this.GetComponent<SpriteRenderer>().sprite = eightZero; break;
            case "fiveThree": this.GetComponent<SpriteRenderer>().sprite = fiveThree; break;
            case "oneFIve": this.GetComponent<SpriteRenderer>().sprite = oneFive; break;
            case "oneFour": this.GetComponent<SpriteRenderer>().sprite = oneFour; break;
            case "zeroTwo": this.GetComponent<SpriteRenderer>().sprite = zeroTwo; break;
        }
    }

    public void setCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

}
