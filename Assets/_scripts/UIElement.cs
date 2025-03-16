using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SelectionHandler selectionHandler;
    void Start()
    {
        selectionHandler = GameObject.Find("Selection").GetComponent<SelectionHandler>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectionHandler.onUI = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        selectionHandler.onUI = false;
    }
}
