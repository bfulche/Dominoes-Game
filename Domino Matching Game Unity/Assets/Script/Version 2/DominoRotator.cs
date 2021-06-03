using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoRotator : MonoBehaviour
{
    [SerializeField] Transform anchorPoint;
    [SerializeField] float rotationAmount = 45f;
    [SerializeField] GameObject visuals;
    public Transform Parent => anchorPoint;

    private void Awake()
    {
        ToggleVisuals(false);
    }
    public void RotateRight()
    {
        anchorPoint.Rotate(0f, 0f, -rotationAmount, Space.Self);
     //   AudioManager.Instance.Play("RotateDomino");
    }


    public void RotateLeft()
    {
        anchorPoint.Rotate(0f, 0f, rotationAmount, Space.Self);
     //   AudioManager.Instance.Play("RotateDomino");
    }

    public void ResetRotation()
    {
        anchorPoint.rotation = Quaternion.identity;
    }

    public void ToggleVisuals(bool newState)
    {
        ResetRotation();
        visuals.SetActive(newState);
    }
}
