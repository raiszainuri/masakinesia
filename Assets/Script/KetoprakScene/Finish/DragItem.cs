using UnityEngine;

public class DragItem : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        offset = transform.position - cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseDrag()
    {
        Vector3 newPos = cam.ScreenToWorldPoint(Input.mousePosition) + offset;
        newPos.z = 0f;
        transform.position = newPos;
    }
}