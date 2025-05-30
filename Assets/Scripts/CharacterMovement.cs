using System;
using Interactables;
using UnityEngine;

[RequireComponent((typeof(CharacterController)))]
public class CharacterMovement : MonoBehaviour
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

    private CharacterController _characterController;
    private Vector3 _velocity;
    private bool _isRunning;
    private bool _isGrounded;
    private InputSystemActions _inputActions;
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isActive;

    private float _verticalRotation = 0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _inputActions = new InputSystemActions();
        _inputActions.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _inputActions.Player.Move.canceled += _ => _moveInput = Vector2.zero;

        _inputActions.Player.Sprint.performed += _ => _isRunning = true;
        _inputActions.Player.Sprint.canceled += _ => _isRunning = false;

        _inputActions.Player.Jump.performed += _ => Jump();

        _inputActions.Player.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
        _inputActions.Player.Look.canceled += _ => _lookInput = new Vector2(0, 0);

        _inputActions.Player.Window.performed += _ => ToggleControl(!_isActive);

        _inputActions.Player.Interact.performed += _ => TryInteract();

        _inputActions.Player.Exit.performed += _ => ExitGame();
    }

    private static void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void TryInteract()
    {
        if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, interactionRange,
                interactableLayer))
            return;

        if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
        {
            interactable.Interact();
        }
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

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, interactionRange,
                interactableLayer))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.ShowHint();
                if (interactable is CameraChangerItem camChange)
                {
                }
            }
        }
    }

    private void Move()
    {
        _isGrounded = _characterController.isGrounded;
        if (_isGrounded && _velocity.y <= 0)
        {
            _velocity.y = -2.0f;
        }

        var move = new Vector3(_moveInput.x, 0, _moveInput.y);
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

    private void Jump()
    {
        if (_isGrounded)
            _velocity.y = (float)Math.Sqrt(jumpHeight * -2.0f * gravity);
    }

    private void ToggleControl(bool state)
    {
        _isActive = state;
        Cursor.visible = !state;
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
    }
}