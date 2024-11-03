using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private bool isDragging = false;
    public void OnClick() {
        Debug.Log("Clicked on " + gameObject.name);

        isDragging = true;
    }

    public void OnDrag()
    {
        if (!isDragging) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
    }

    public void OnRelease()
    {
        isDragging = false;
    }
}
