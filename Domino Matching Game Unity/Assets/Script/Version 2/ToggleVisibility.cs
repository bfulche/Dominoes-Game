using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Can be used to show, or hide gameobjects on Start(), based on whether or not the user is a host or not.
/// </summary>
public class ToggleVisibility : MonoBehaviour
{
    [SerializeField] GameObject[] DisableNonHost;
    [SerializeField] GameObject[] EnableNonHost;

    [SerializeField] GameObject[] DisableHost;
    [SerializeField] GameObject[] EnableHost;

    void Start()
    {
        if (DisableNonHost != null && !PhotonNetwork.IsMasterClient)
            DisableObjects(DisableNonHost);

        if (EnableNonHost != null && !PhotonNetwork.IsMasterClient)
            EnableObjects(EnableNonHost);

        if (DisableHost != null && PhotonNetwork.IsMasterClient)
            DisableObjects(DisableHost);

        if (EnableHost != null && PhotonNetwork.IsMasterClient)
            EnableObjects(EnableHost);
    }

    private void DisableObjects(GameObject[] objectList)
    {
        foreach (GameObject item in objectList)
            item.SetActive(false);
    }

    private void EnableObjects(GameObject[] objectList)
    {
        foreach (GameObject item in objectList)
            item.SetActive(true);
    }
}
