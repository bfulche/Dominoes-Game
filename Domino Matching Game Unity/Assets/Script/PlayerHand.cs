using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// holds reference to children transforms (possible tile starting positions). Simplifies gettign random starting tile positions
/// </summary>
public class PlayerHand : MonoBehaviour
{
    [SerializeField] Transform[] slots;

    public Transform[] Slots => slots;
}
