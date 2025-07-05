using UnityEngine;
using UnityEngine.UI;

public class BackgroundComponent : MonoBehaviour
{
    [SerializeField] private Sprite _landscapeSprite;
    [SerializeField] private Sprite _portraitSprite;

    private Image _image;
    private CanvasScaler _canvas;

    private void Start()
    {
        _image = GetComponent<Image>();
        _canvas = GetComponent<CanvasScaler>();
        MainUpdateController.onLateUpdate += onLateUpdate;
    }

    private void OnDestroy()
    {
        MainUpdateController.onLateUpdate -= onLateUpdate;
    }

    private void onLateUpdate()
    {
        if (_canvas != null)
        {
            if (Screen.width > Screen.height)
            {
                _canvas.referenceResolution = new Vector2(1920, 2400);
            }
            else
            {
                _canvas.referenceResolution = new Vector2(1080, 1920);
            }
        }

        if (_image != null)
        {
            if (Screen.width > Screen.height)
            {
                _image.sprite = _landscapeSprite;
            }
            else
            {
                _image.sprite = _portraitSprite;
            }
        }
    }
}
