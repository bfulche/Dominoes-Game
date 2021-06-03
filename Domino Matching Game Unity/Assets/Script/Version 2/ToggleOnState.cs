using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOnState : MonoBehaviour
{
    [SerializeField] GameObject[] toggledObjects;
    [SerializeField] bool enableWhileActive = false;


    private void ToggleState(bool newState)
    {
        foreach (GameObject item in toggledObjects)
        {
            if (item != null)
                item.SetActive(newState);
        }
    }

    private void OnEnable()
    {
        ToggleState(enableWhileActive);
    }

    private void OnDisable()
    {
        ToggleState(!enableWhileActive);
    }
}
