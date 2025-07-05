using UnityEngine;

public class CanvasAdaptive : MonoBehaviour
{
    private RectTransform _panel;

    void Awake()
    {
        _panel = GetComponent<RectTransform>();
        Apply();
    }

    void Apply()
    {
        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        _panel.anchorMin = anchorMin;
        _panel.anchorMax = anchorMax;
    }
}
