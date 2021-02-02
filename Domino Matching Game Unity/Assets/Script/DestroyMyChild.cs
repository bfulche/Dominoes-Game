using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMyChild : MonoBehaviour
{
    public void DestroyRoomListings()
    {
        Transform roomContainerObject = this.transform;

        foreach (Transform child in roomContainerObject)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnEnable()
    {
        DestroyRoomListings();
    }
}
