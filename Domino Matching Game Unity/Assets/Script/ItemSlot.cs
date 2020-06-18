using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler {

    GameManager gm;
    public List<GameObject> testList = new List<GameObject>();

    


    public void OnDrop(PointerEventData eventData) {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null) {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            Debug.Log("Dropped Object was:" + eventData.pointerDrag);
            testList.Add(eventData.pointerDrag);





        
        }
    }
}
