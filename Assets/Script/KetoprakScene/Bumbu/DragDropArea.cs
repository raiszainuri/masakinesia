using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class DragDropArea : MonoBehaviour, IDragDropArea
{
    public void OnDragDrop(Dragable drag)
    {
        drag.transform.position = transform.position;
    }
}

