using UnityEngine;
using UnityEngine.EventSystems;

public class ClickContinueWindow : Window, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        base.Hide();
        GameController.currentOpenWindow.Show();
        if(!Cursor.visible)
            MobileCamController.activated = true;
    }
}
