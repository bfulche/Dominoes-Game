using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Objects with this script will accept a Digital Domino.
/// </summary>
public class SelectableLocation : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private bool valid = true; // purely for viewing/testing in the inspector

    private Collider boxCollider; // what enables/disables selecting the location

    private void Start()
    {
        boxCollider = GetComponent<Collider>();
    }

    public void MarkValid()
    {
        boxCollider.enabled = true;
        valid = true;
    }

    public void MarkInvalid()
    {
        boxCollider.enabled = false;
        valid = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!GameplayManager.Instance.InputEnabled || GameplayManager.Instance.IsObserver)
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Domino.HasSelected)
            {
                MarkInvalid();
                Domino.SetNewReturnPosition(transform.position, this);
            }
        }
    }
}
