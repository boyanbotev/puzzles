using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    public static event Action onPuzzlePieceSnapped;
    [SerializeField] public Transform target;
    private bool isDragging = false;
    public bool isSnapped = false;
    private float minSnapDistance = 1f;
    public void OnClick() {
        if (isSnapped) return;
        isDragging = true;
    }

    public void OnDrag()
    {
        if (!isDragging || isSnapped) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
    }

    public void OnRelease()
    {
        isDragging = false;
        if (Vector3.Distance(transform.position, target.position) < minSnapDistance)
        {
            transform.position = target.position;
            isSnapped = true;
            onPuzzlePieceSnapped?.Invoke();
        }
    }
}
