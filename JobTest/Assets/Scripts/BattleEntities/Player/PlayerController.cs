using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    
    private Rigidbody _rigidBody;
    private PlayerInputActions _playerInputActions;

    private Vector3 _currentDisplacement;
    
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
    }

    private void FixedUpdate()
    {
        _rigidBody.MovePosition(_rigidBody.position + _currentDisplacement * Time.fixedDeltaTime);
    }

    private void GatherInput()
    {
        var movementDirection = _playerInputActions.Player.Movement.ReadValue<Vector2>().normalized;
        _currentDisplacement = new Vector3 (movementDirection.x, 0, movementDirection.y) * _speed;
        // Debug.Log();
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
