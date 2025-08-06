using UnityEngine;

public class ObjectDraging : MonoBehaviour
{
    public bool isDragable = true;
    private Vector3 offset;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        if (!isDragable) return;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }

    void OnMouseDrag()
    {
        if (!isDragable) return;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z) + offset;
    }

    public void DisableDrag()
    {
        isDragable = false;
    }

    public void EnableDrag()
    {
        isDragable = true;
    }
}
