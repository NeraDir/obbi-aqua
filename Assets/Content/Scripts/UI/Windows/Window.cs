using UnityEngine;

public abstract class Window : MonoBehaviour
{
    [SerializeField] private bool _activeMotion;
    [SerializeField] private bool _dontSetCurrent;

    private Animator _animator;

    public virtual void OnEnable()
    {
        PlayerController.Activated = _activeMotion;
        if(!_dontSetCurrent)
            GameController.currentOpenWindow = this;
        CustomButton.pressed = false;
        if(_animator == null) _animator = GetComponent<Animator>();
    }

    public virtual void Init()
    {

    }

    public virtual void Hide()
    {
        if (_animator != null)
        {
            _animator.SetBool("WINDOW_STATE", true);
            Invoke(nameof(DisActivate), 0.5f);
        }
        else
        {
            DisActivate();
        }
    }

    private void DisActivate()
    {
        gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
}
