using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabToNextField : MonoBehaviour
{

    public void Update()
    {
        // Don't bother checking input if we don't have a selected game object.
        if (EventSystem.current.currentSelectedGameObject == null)
            return;

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable previous = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
            
            if (previous != null)
            { 
                previous = previous.FindSelectableOnUp();

                if (previous != null)
                    previous.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();

            if (next != null)
            {
                next = next.FindSelectableOnDown();
                if (next != null)
                    next.Select();
            }
        }
    }
}
