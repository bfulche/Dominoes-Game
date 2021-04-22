using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextRefHolder : MonoBehaviour
{
    [SerializeField] TMP_Text textRef;

    public TMP_Text Text => textRef;
}
