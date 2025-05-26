using System;
using Interactables;
using UnityEngine;

[RequireComponent((typeof(CharacterController)))]
public class SubmarineController : MonoBehaviour
{
    public float speed;
    public float runMultiplier;
    public float gravity = -9.81f;
    public float jumpHeight;
    public float rotationSpeed;
    public bool lockOnStart = false;

    [SerializeField]
    public Transform cameraTransform;

    [SerializeField]
    private float interactionRange = 3f;

    [SerializeField]
    private LayerMask interactableLayer;

    [SerializeField]
    public GlobalUI globalUI;

    [SerializeField]
    public GameObject mainCharacterControl;

    private CharacterController _characterController;
    private Vector3 _velocity;
    private bool _isRunning;
    private bool _isGrounded;
    private InputSystemActions _inputActions;
    private Vector2 _moveInput;
    private float _verticalMovement;
    private Vector2 _lookInput;
    private bool _isActive;

    private float _verticalRotation = 0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _inputActions = new InputSystemActions();
        _inputActions.Submarine.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _inputActions.Submarine.Move.canceled += _ => _moveInput = Vector2.zero;

        _inputActions.Submarine.Up.performed += ctx => _verticalMovement = 1.0f;
        _inputActions.Submarine.Up.canceled += _ => _verticalMovement = 0;

        _inputActions.Submarine.Down.performed += ctx => _verticalMovement = -1.0f;
        _inputActions.Submarine.Down.canceled += _ => _verticalMovement = 0;

        _inputActions.Submarine.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
        _inputActions.Submarine.Look.canceled += _ => _lookInput = new Vector2(0, 0);
        _inputActions.Submarine.Window.performed += _ => ToggleControl(!_isActive);

        _inputActions.Submarine.Exit.performed += _ => ExitMode();
    }

    private void ExitMode()
    {
        var characterCamera = mainCharacterControl.GetComponentInChildren<Camera>();

        GetComponentInChildren<Camera>().enabled = false;
        this.gameObject.SetActive(false);
        
        characterCamera.enabled = true;
        mainCharacterControl.SetActive(true);   
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void Start()
    {
        if (lockOnStart)
            ToggleControl(true);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        _isGrounded = _characterController.isGrounded;
        if (_isGrounded && _velocity.y <= 0)
        {
            _velocity.y = -2.0f;
        }

        var move = new Vector3(_moveInput.x, _verticalMovement, _moveInput.y);
        move = transform.TransformDirection(move);
        var currentSpeed = _isRunning ? speed * runMultiplier : speed;

        _characterController.Move(move * (currentSpeed * Time.deltaTime));
        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);

        Rotate();
    }

    private void Rotate()
    {
        if (_lookInput is { x: 0, y: 0 } || !_isActive) return;

        var yaw = _lookInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * yaw);

        var pitch = -_lookInput.y * rotationSpeed * Time.deltaTime;
        _verticalRotation = Mathf.Clamp(_verticalRotation + pitch, -80f, 80f);
        cameraTransform.localEulerAngles = new Vector3(_verticalRotation, 0f, 0f);
    }

    private void ToggleControl(bool state)
    {
        _isActive = state;
        Cursor.visible = !state;
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
