using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 2f;
    [SerializeField] private float _runSpeed = 4f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _rotationSmoothTime = 0.1f;
    [SerializeField] private float _groundCheckDistance = 0.2f;
    [SerializeField] private int _maxJumps = 2;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private ParticleSystem _runParticles;
    [SerializeField] private ParticleSystem _dropParticles;
    [SerializeField] private JoystickComponent _joystick;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _landClip;

    private Rigidbody _rb;
    private AnimationController _animator;
    private Transform _cam;
    private Device _device;

    private float _currentSpeed;
    private float _rotationVelocity;
    private Vector2 _inputDirection;
    private bool _isRunning;
    private bool _isGrounded;
    private bool _wasGrounded;
    private bool _kicking;
    private bool _activated;
    
    private int _currentJumps;

    private float _kickTimer;
    private const float KickDuration = 1f;

    public bool IsRunning => _isRunning;
    public bool IsGrounded => _isGrounded;
    public Vector3 Velocity => _rb.linearVelocity;
    public static bool Activated;

    private void Awake()
    {
        YandexWebBaseFuctions.onSetOrientationByDevice += OnGetDevice;
        MainUpdateController.onUpdate += onUpdate;
        MainUpdateController.onFixedUpdate += onFixedUpdate;
    }

    private void OnDestroy()
    {
        YandexWebBaseFuctions.onSetOrientationByDevice -= OnGetDevice;
        MainUpdateController.onUpdate -= onUpdate;
        MainUpdateController.onFixedUpdate -= onFixedUpdate;
    }

    private void OnGetDevice(Device device)
    {
        _device = device;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<AnimationController>();
        _cam = Camera.main.transform;

        _currentJumps = _maxJumps;
    }

    private void onUpdate()
    {
        UserInput();
        GroundCheck();
        Kick();
        Animation();
        Particles();
    }

    private void onFixedUpdate()
    {
        Move();
    }

    public void ToggleCursor()
    {
        if (!Activated)
            return;
        Cursor.visible = !Cursor.visible;
        MobileCamController.activated = !Cursor.visible;
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void UserInput()
    {
        if (_device == Device.PC)
            if (Input.GetKeyDown(KeyCode.Tab))
                ToggleCursor();

        _inputDirection = _device == Device.PC
            ? new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            : new Vector2(_joystick.Direction.x, _joystick.Direction.y);

        _isRunning = _device == Device.PC && Input.GetKey(KeyCode.LeftShift);

        if (_device == Device.PC && Input.GetKeyDown(KeyCode.E))
            LaunchKick();

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    private void GroundCheck()
    {
        _wasGrounded = _isGrounded;
        _isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, _groundCheckDistance, _groundLayer);

        if (_isGrounded && !_wasGrounded)
        {
            _dropParticles?.Play();
            Settings.playSound?.Invoke(_landClip);
            _currentJumps = _maxJumps;
        }
    }

    private void Move()
    {
        if (_kicking)
        {
            _rb.linearVelocity = Vector3.zero;
            return;
        }

        Vector3 forward = _cam.forward;
        Vector3 right = _cam.right;
        forward.y = right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = (forward * _inputDirection.y + right * _inputDirection.x).normalized;

        float targetSpeed = _isRunning ? _runSpeed : _walkSpeed;
        _currentSpeed = move.magnitude > 0.1f ? targetSpeed : 0f;

        Vector3 targetVelocity = move * _currentSpeed;
        Vector3 velocity = new Vector3(targetVelocity.x, _rb.linearVelocity.y, targetVelocity.z);
        _rb.linearVelocity = velocity;

        if (move != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationVelocity, _rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    public void Run()
    {
        _isRunning = !_isRunning;
    }

    public void Jump()
    {
        if (_currentJumps > 0)
        {
            Settings.playSound?.Invoke(_jumpClip);
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _currentJumps--;
            if (!_dropParticles.isPlaying) _dropParticles.Play();
        }
    }

    public void LaunchKick()
    {
        if (Cursor.visible && _device == Device.PC)
            return;
        if (!_isGrounded)
            return;
        if (_kicking) 
            return;
        _kicking = true;
        _kickTimer = KickDuration;
        _animator.SetAnimationState("character_state", 5);
    }

    private void Kick()
    {
        if (!_kicking) return;

        _kickTimer -= Time.deltaTime;
        if (_kickTimer <= 0f)
            _kicking = false;
    }

    private void Animation()
    {
        if (_kicking)
            return;
        if (!_isGrounded)
        {
            _animator.SetAnimationState("character_state", 3);
        }
        else if (_inputDirection == Vector2.zero)
        {
            _animator.SetAnimationState("character_state", 0);
        }
        else
        {
            _animator.SetAnimationState("character_state", _isRunning ? 2 : 1);
        }
    }

    private void Particles()
    {
        if (_currentSpeed > 0 && _isRunning && _isGrounded)
        {
            if (!_runParticles.isPlaying) _runParticles.Play();
        }
        else
        {
            if (_runParticles.isPlaying) _runParticles.Stop();
        }
    }
}

