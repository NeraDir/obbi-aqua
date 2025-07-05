using Unity.VisualScripting;
using UnityEngine;

public class PortalComponent : MonoBehaviour
{
    [SerializeField] private Vector3 m_Direction;
    [SerializeField] private float m_Speed;

    private Transform m_Transform;

    private void Awake()
    {
        m_Transform = transform;
        MainUpdateController.onLateUpdate += onLateUpdate;
    }

    private void OnDestroy()
    {
        MainUpdateController.onLateUpdate -= onLateUpdate;
    }

    private void onLateUpdate()
    {
        if (m_Transform == null)
            return;
        m_Transform.Rotate(m_Direction, m_Speed * Time.deltaTime);
    }
}
