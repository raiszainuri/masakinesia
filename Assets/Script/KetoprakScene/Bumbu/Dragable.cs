using System;
using UnityEngine;

public class Dragable : MonoBehaviour
{
    private Collider2D coll;
    private Vector3 startDragPosition;
    private bool isTouchingPlate = false;
    private Collider2D plateCollider;

    void Start()
    {
        coll = GetComponent<Collider2D>();
    }

    void OnMouseDown()
    {
        startDragPosition = transform.position;
    }

    void OnMouseDrag()
    {
        Vector3 targetPos = GetMousePositionInWorldSpace();

        if (isTouchingPlate && plateCollider != null)
        {
            if (IsPointInsideCollider(plateCollider, targetPos))
            {
                transform.position = targetPos;
            }
            else
            {
                Debug.Log("🚫 Tidak boleh keluar dari plate!");
            }
        }
        else
        {
            transform.position = targetPos;
        }
    }

    void OnMouseUp()
    {
        // Cek semua collider di posisi mouse
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        bool droppedSuccessfully = false;

        foreach (var hit in hits)
        {
            if (hit != coll && hit.TryGetComponent(out IDragDropArea dragDropArea))
            {
                dragDropArea.OnDragDrop(this);
                droppedSuccessfully = true;
                break;
            }
        }

        if (!droppedSuccessfully)
        {
            transform.position = startDragPosition;
        }
    }

    public Vector3 GetMousePositionInWorldSpace()
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        p.z = 0f;
        return p;
    }

    private bool IsPointInsideCollider(Collider2D collider, Vector2 point)
    {
        // Lebih aman, bisa diganti dengan Physics2D.OverlapPoint jika perlu
        return collider.OverlapPoint(point);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Plate"))
        {
            isTouchingPlate = true;
            plateCollider = other;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Plate"))
        {
            isTouchingPlate = false;
            plateCollider = null;
        }
    }
}
