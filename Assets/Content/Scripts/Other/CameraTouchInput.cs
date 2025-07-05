using UnityEngine;
using UnityEngine.EventSystems;

public class CameraTouchInput : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public static Vector2 LookInput;
    public static bool IsDragging { get; private set; }

    [SerializeField] private float Sensitivity = 0.2f;

    public void OnBeginDrag(PointerEventData eventData)
    {
        LookInput = Vector2.zero;
        IsDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        LookInput = eventData.delta * Sensitivity;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        LookInput = Vector2.zero;
        IsDragging = false;
    }
}
