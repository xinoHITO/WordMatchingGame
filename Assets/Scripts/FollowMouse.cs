using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private RectTransform dragRectTransform;

    private void Awake()
    {
        dragRectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        dragRectTransform.position = mousePos;
    }
}
