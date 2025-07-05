using System;
using UnityEngine;
using YG;

public class MobileCamController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 4.0f;

    [SerializeField] private float yMinLimit = -30f;
    [SerializeField] private float yMaxLimit = 70f;

    [SerializeField] private float collisionRadius = 1;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private LayerMask collisionMask;
    private Vector3 currentVelocity;

    private float x = 0.0f;
    private float y = 0.0f;

    private Device _device;
    public static bool activated;

    private void Awake()
    {
        YandexWebBaseFuctions.onSetOrientationByDevice += OnGetDevice;
    }

    private void OnDestroy()
    {
        YandexWebBaseFuctions.onSetOrientationByDevice -= OnGetDevice;
    }

    private void OnGetDevice(Device device)
    {
        _device = device;

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    private void LateUpdate()
    {
        if (_device == Device.Mobile)
        {
            if (activated && CameraTouchInput.IsDragging)
            {
                Vector2 delta = CameraTouchInput.LookInput;
                x += delta.x * YG2.saves.Sensivity / 5;
                y -= delta.y * YG2.saves.Sensivity / 5;
                y = ClampAngle(y, yMinLimit, yMaxLimit);
            }
        }
        else if (_device == Device.PC)
        {
            if (activated)
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                x += mouseX * YG2.saves.Sensivity;
                y -= mouseY * YG2.saves.Sensivity;
                y = ClampAngle(y, yMinLimit, yMaxLimit);
            }
        }

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 desiredPosition = rotation * negDistance + target.position;

        Vector3 direction = (desiredPosition - target.position).normalized;
        if (Physics.SphereCast(target.position, collisionRadius, direction, out RaycastHit hit, distance, collisionMask))
        {
            float adjustedDistance = Mathf.Clamp(hit.distance - 0.1f, minDistance, distance);
            desiredPosition = target.position + direction * adjustedDistance;
        }

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, Time.deltaTime * smoothSpeed);
        transform.rotation = rotation;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
