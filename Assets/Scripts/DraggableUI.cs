using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour
{
    private RectTransform dragRectTransform;
    private Canvas canvas;

    private void Awake()
    {
        dragRectTransform = GetComponent<RectTransform>();
        canvas = dragRectTransform.GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            while (canvas == null)
            {
                Transform parent = dragRectTransform.parent;
                if (parent == null)
                {
                    break;
                }
                canvas = parent.GetComponent<Canvas>();
            }
        }
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        dragRectTransform.anchoredPosition = mousePos / canvas.scaleFactor;
    }
}
