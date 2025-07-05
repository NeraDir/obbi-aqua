using UnityEngine;

public class PlayerFootstepController : MonoBehaviour
{
    [Header("Footstep Clips")]
    [SerializeField] private AudioClip[] _defaultSteps;
    [SerializeField] private AudioClip[] _sandSteps;

    [Header("Detection")]
    [SerializeField] private float _stepInterval = 0.5f;
    [SerializeField] private float _runInterwal = 0.2f;

    private float _stepTimer;
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _stepTimer = GetCurrentStepInterval();
        MainUpdateController.onUpdate += onUpdate;
    }

    private void OnDestroy()
    {
        MainUpdateController.onUpdate -= onUpdate;
    }

    private void onUpdate()
    {
        if (!_playerController.IsGrounded || _playerController.Velocity.magnitude < 0.2f)
        {
            _stepTimer = GetCurrentStepInterval();
            return;
        }

        _stepTimer -= Time.deltaTime;

        if (_stepTimer <= 0f)
        {
            PlayFootstep();
            _stepTimer = GetCurrentStepInterval();
        }
    }

    private float GetCurrentStepInterval()
    {
        return _playerController != null && _playerController.IsRunning
            ? _runInterwal
            : _stepInterval;
    }

    private void PlayFootstep()
    {
        AudioClip[] clipsToUse = _defaultSteps;

        if (clipsToUse.Length > 0)
        {
            int index = Random.Range(0, clipsToUse.Length);
            Settings.playSound?.Invoke(clipsToUse[index]);
        }
    }
}
