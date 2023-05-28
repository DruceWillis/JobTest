using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    
    private Rigidbody _rigidBody;
    private PlayerInputActions _playerInputActions;

    private Vector2 _currentMovementDirection;
    
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
       
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Movement.performed += OnInputMovementPerformed;
        _playerInputActions.Player.Fire.performed += OnInputFirePerformed;
    }

    private void Update()
    {
        GatherInput();
        Move();
    }

    private void GatherInput()
    {
        _currentMovementDirection = _playerInputActions.Player.Movement.ReadValue<Vector2>().normalized;
    }

    private void Move()
    {
        var newVelocity = _currentMovementDirection * _speed;
        _rigidBody.velocity = new Vector3 (newVelocity.x, _rigidBody.velocity.y, newVelocity.y);
    }
    
    private void OnInputMovementPerformed(InputAction.CallbackContext context)
    {
        // Debug.Log(context);
    }
    
    private void OnInputFirePerformed(InputAction.CallbackContext context)
    {
        // Debug.Log(context);
    }

    private void OnDestroy()
    {
        _playerInputActions.Player.Movement.performed -= OnInputMovementPerformed;
        _playerInputActions.Player.Fire.performed -= OnInputFirePerformed;
    }
}
