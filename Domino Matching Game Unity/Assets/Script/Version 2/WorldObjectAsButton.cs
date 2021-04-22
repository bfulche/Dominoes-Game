using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldObjectAsButton : MonoBehaviour
{
    GameObject definedButton;
    public UnityEvent OnClick = new UnityEvent();

    [SerializeField] Color startColor = Color.white;
    [SerializeField] Color highlightColor = Color.yellow;

    MeshRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        definedButton = this.gameObject;
        renderer = GetComponent<MeshRenderer>();

        startColor = renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        if (Input.GetMouseButtonDown(0))
        { 

            if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
            {
                Debug.Log("Button Clicked");
                OnClick.Invoke();
            }
        }
    }

    private void OnMouseEnter()
    {
        renderer.material.color = highlightColor;
    }

    private void OnMouseExit()
    {
        renderer.material.color = startColor;
    }
}
