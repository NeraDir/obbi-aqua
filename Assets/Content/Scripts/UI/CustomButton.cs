using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class CustomButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Window _openWindow;
    [SerializeField] private Window _closeWindow;
    [SerializeField] private UnityEvent _action;

    [SerializeField] private AudioClip _clip;

    [SerializeField] private bool _rotate;
    [SerializeField] private bool _scaler;

    [SerializeField] private float _maxRotate = 45;

    public static bool pressed;

    private Vector3 _scale;
    private Quaternion _rotation;

    private void Awake ()
    {
        _scale = transform.localScale;
        _rotation = transform.rotation;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (pressed)
            return;
        pressed = true;
        Settings.playSound?.Invoke(_clip);
        if (_rotate)
        {
            transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, Random.Range(0, _maxRotate)), 0.1f / 2).OnComplete(() =>
            {
                transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, Random.Range(-_maxRotate, 0)), 0.1f * 2).OnComplete(() =>
                {
                    transform.DOLocalRotateQuaternion(_rotation, 0.1f).OnComplete(() =>
                    {
                        StartCoroutine(OnPressed());
                    });
                });
            });
            return;
        }
        if (_scaler)
        {
            transform.DOScale(_scale / 1.2f, 0.1f / 2).OnComplete(() =>
            {
                transform.DOScale(_scale * 1.1f, 0.1f * 2).OnComplete(() =>
                {
                    transform.DOScale(_scale, 0.1f).OnComplete(() =>
                    {
                        StartCoroutine(OnPressed());
                    });
                });
            });
            return;
        }
        StartCoroutine(OnPressed());
    }

    private IEnumerator OnPressed()
    {
        if(_closeWindow != null)
        {
            _closeWindow.Hide();
            yield return !_closeWindow.gameObject.activeInHierarchy;
        }
        if(_openWindow != null)
        {
            _openWindow.Show();
            yield return new WaitForSeconds(0.5f);
        }
        pressed = false;
        _action?.Invoke();
    }

    public void SetAction(UnityAction action)
    {
        _action.AddListener(action);
    }
}
