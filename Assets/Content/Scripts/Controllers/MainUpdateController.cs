using System;
using UnityEngine;

public class MainUpdateController : MonoBehaviour
{
    public static Action onUpdate;
    public static Action onLateUpdate;
    public static Action onFixedUpdate;

    private void Update()
    {
        onUpdate?.Invoke();
    }

    private void LateUpdate()
    {
        onLateUpdate?.Invoke();
    }

    private void FixedUpdate()
    {
        onFixedUpdate?.Invoke();
    }
}
