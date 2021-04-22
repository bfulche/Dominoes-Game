using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Credit to https://youtu.be/HXFoUGw7eKk
/// </summary>
public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;

    public Tooltip toolTip;
    private void Awake()
    {
        current = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public static void Show(string content, string header = "")
    {
        current.toolTip.SetText(content, header);
        current.toolTip.gameObject.SetActive(true);
        Cursor.visible = false;
    }

    public static void Hide()
    {
        current.toolTip.gameObject.SetActive(false);
        Cursor.visible = true;
    }
}
