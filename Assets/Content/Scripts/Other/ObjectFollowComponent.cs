using UnityEngine;

public class ObjectFollowComponent : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _speed;

    private void Awake()
    {
        MainUpdateController.onLateUpdate += onLateUpdate;
    }

    private void OnDestroy()
    {
        MainUpdateController.onLateUpdate -= onLateUpdate;
    }

    private void onLateUpdate()
    {
        if (_target != null)
            transform.position = Vector3.Lerp(transform.position, _target.position + _offset, _speed * Time.deltaTime);
    }
}
