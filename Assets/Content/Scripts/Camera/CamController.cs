using UnityEngine;

public class CamController : MonoBehaviour
{
    public GameObject CinemachineCameraTarget;

    public GameObject cinemachine;

    public bool LockCameraPosition = false;

    public float TopClamp = 70.0f;

    public float BottomClamp = -30.0f;

    public float CameraAngleOverride = 0.0f;
    public float minDistance = 1f;
    public float smoothSpeed = 10f;
    public LayerMask collisionMask;

    private const float _threshold = 0.01f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private Vector3 _currentVelocity;
    public static bool activated;

    private Device _device;

    public bool _isWork;

    private void Awake()
    {
        YandexWebBaseFuctions.onSetOrientationByDevice += OnGetDevice;
        MainUpdateController.onLateUpdate += onLateUpdate;
    }

    private void OnDestroy()
    {
        YandexWebBaseFuctions.onSetOrientationByDevice -= OnGetDevice;
        MainUpdateController.onLateUpdate -= onLateUpdate;
    }

    private void OnGetDevice(Device device)
    {
        _device = device;
        if (_device != Device.Mobile)
            _isWork = true;
        else
            return;
        cinemachine.SetActive(true);
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void onLateUpdate()
    {
        if (!_isWork)
            return;
        if (!activated)
            return;

        if (_device == Device.Mobile)
        {
            if (Input.GetMouseButton(1))
            {
                CameraRotation();
            }
        }
        else
        {
            CameraRotation();
        }
    }

    private void CameraRotation()
    {
        Vector2 input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (input.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            float deltaTimeMultiplier = 1;

            _cinemachineTargetYaw += (input.x) * deltaTimeMultiplier;
            _cinemachineTargetPitch += (-input.y) * deltaTimeMultiplier;
        }
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
           _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
