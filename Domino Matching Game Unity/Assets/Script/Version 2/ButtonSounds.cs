using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSounds : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] private string highlightSound = "Button Highlight";
    [SerializeField] private string clickSound = "Button Click";

    /// <summary>
    /// Button highlighted
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightSound != string.Empty)
            AudioManager.Instance.Play(highlightSound);
    }

    /// <summary>
    /// Button Clicked
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (clickSound != string.Empty)
                AudioManager.Instance.Play(clickSound);
        }
    }


}
