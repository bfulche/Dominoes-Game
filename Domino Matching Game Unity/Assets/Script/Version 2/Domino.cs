using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 3D replacement of the Tile.cs script.
/// Attached to dominoes - allowing a domino to be selected on Left click
/// </summary>
public class Domino : MonoBehaviour, IPointerDownHandler
{
    private int id = -1;

    [SerializeField] private bool selected = false;

    private bool onTray = true;
    private static Domino selectedDomino = null;

    public static bool HasSelected => selectedDomino ? true : false;

    private Vector3 defaultScale = new Vector3(1.0f, 2.0f, 0.2f);

    private Vector3 selectedScale = new Vector3(1.2f, 2.4f, 0.2f);

    private Vector3 returnPosition; // The domino will return to this position when deselected

    private SelectableLocation currentLocation; // used to make sure the location is valid for future selection

    private Vector3 cachedPosition; // check if position changed since selected.
    private Quaternion cachedRotation; // check if our rotation changed since selected. If changed, send RPC on Deselect - otherwise skip

    public int ID
    {
        get { return id; }
        set
        {
            if (id != -1)   // only allow id edits if value isn't initialized (-1)
                return;
            if (0 > value)  // don't edit if initialization value is negative
                return;

            id = value; // initialize id using specified value
        }
    }

    public void ResetDomino()
    {
        // clears state tracking so it can be placed back on the tray
        selected = false;
        returnPosition = Vector3.zero;
        cachedPosition = Vector3.zero;
        cachedRotation = Quaternion.identity;
        currentLocation = null;
        onTray = true;
      //  transform.localScale = defaultScale;

    }

    private void Select()
    {
        selectedDomino = this;
        selected = true;
        Debug.Log("Domino: " + this.gameObject.name + " selected.");

        //    if (!onTray)
        //    {
        //        returnPosition = this.transform.position;
        //    }
        //    else
        //    {
        //        returnPosition = Vector3.zero;
        //        GameplayManager.Instance.DominoTray.RemoveDomino(this);
        //    }

        if (currentLocation != null)
            currentLocation.MarkValid();

        cachedPosition = transform.position;
        cachedRotation = transform.rotation;

        returnPosition = transform.position;
        GameplayManager.Instance.DominoRotator.transform.position = returnPosition;
        GameplayManager.Instance.DominoRotator.ToggleVisuals(true);
        transform.parent = GameplayManager.Instance.DominoRotator.Parent;
        transform.localPosition = Vector3.zero; // center domino on rotator

        // VFX on
        this.transform.localScale = selectedScale;

    }

    private void DeSelect()
    {
        selectedDomino = null;
        selected = false;
        Debug.Log("Domino: " + this.gameObject.name + " DEselected.");

        transform.parent = null;
        GameplayManager.Instance.DominoRotator.ToggleVisuals(false);
        //  if (returnPosition != Vector3.zero)
        //      transform.position = returnPosition;
        //  else
        //      GameplayManager.Instance.DominoTray.AddNewTile(this);

        transform.position = returnPosition;

        if (currentLocation != null)
        {
            if (currentLocation.transform.position == returnPosition)
                currentLocation.MarkInvalid();
        }
        // VFX off
        this.transform.localScale = defaultScale;

        // position changed since picking domino up. Send message to other players/observers
     //   if (transform.position != cachedPosition)
            GameplayManager.Instance.photonView.RPC(PhotonProperty.UpdateDominoPosition, RpcTarget.All, PhotonNetwork.LocalPlayer, id, transform.position);

        // rotation changed since picking domino up. Send message to other players/observers
   //     if (transform.rotation != cachedRotation)
            GameplayManager.Instance.photonView.RPC(PhotonProperty.UpdateDominoRotation, RpcTarget.All, PhotonNetwork.LocalPlayer, id, transform.rotation);
    }

    public static void SetNewReturnPosition(Vector3 newPosition, SelectableLocation newLocation)
    {
        if (selectedDomino.onTray)
        {
            GameplayManager.Instance.DominoTray.RemoveDomino(selectedDomino);
            selectedDomino.onTray = false;
        }

        selectedDomino.currentLocation = newLocation;
        selectedDomino.returnPosition = newPosition;
        selectedDomino.DeSelect();


    }

    public static void Clear()
    {
        if (selectedDomino != null)
        {
            selectedDomino.currentLocation = null;
            selectedDomino = null;
        }
    }

    public static void DropSelected()
    {
        if (selectedDomino != null)
            selectedDomino.DeSelect();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (!GameplayManager.Instance.InputEnabled || GameplayManager.Instance.IsObserver)
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Left clicked Domino!
            if (!selected)
            {
                selectedDomino?.DeSelect();

                this.Select();
            }
            else
            {
                this.DeSelect();
            }
        }
    }
}
