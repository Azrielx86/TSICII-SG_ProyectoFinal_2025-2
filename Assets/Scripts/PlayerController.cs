using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public float runMultiplier;
    public float gravity = 9.81f;
    public float jumpHeight;
    public float rotationSpeed;
    public bool lockOnStart = false;

    private CharacterController _characterController;
    private Vector3 _direction;
    private Vector3 _velocity;
    private bool _isRunning;
    private bool _isGrounded;
    private InputSystemActions _inputActions;
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isActive;
    

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _inputActions = new InputSystemActions();
        _inputActions.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _inputActions.Player.Move.canceled += _ => _moveInput = Vector2.zero;

        _inputActions.Player.Sprint.performed += _ => _isRunning = true;
        _inputActions.Player.Sprint.canceled += _ => _isRunning = false;
        //
        // _inputActions.Player.Jump.performed += _ => Jump();

        _inputActions.Player.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
        _inputActions.Player.Look.canceled += _ => _lookInput = new Vector2(0, 0);
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void Update()
    {
        Move();

        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, targetAngle, 0.0f);
        
        _characterController.Move(_direction * (speed * Time.deltaTime));
    }

    private void Move()
    {
        _direction = new Vector3(_moveInput.x, 0.0f, _moveInput.y);
    }
}
