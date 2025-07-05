using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] private GameObject _destroyEffect;

    private void OnEnable()
    {
        MainUpdateController.onLateUpdate += onLateUpdate;
    }

    private void OnDestroy()
    {
        MainUpdateController.onLateUpdate -= onLateUpdate;
    }

    public virtual void OnCollision()
    {
        Instantiate(_destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void onLateUpdate()
    {
        transform.Rotate(new Vector3(0,1,0), 90 * Time.deltaTime);
    }
}
