using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : BaseBattleEntity
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private float _rotationTowardsDirectionSpeed = 40f;
    [SerializeField] private Transform _vikingModel;
    
    private Rigidbody _rigidBody;
    private PlayerInputActions _playerInputActions;

    private Vector3 _currentMovementDirection;
    private Camera _camera;

    private bool _successfullyInitialized;
    
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        
        _camera = Camera.main;

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _successfullyInitialized = !HasNullReferences();
    }

    private void Update()
    {
        if (!_successfullyInitialized) return;
        
        GatherInput();
        Move();
        Rotate();
    }

    private void GatherInput()
    {
        var normalizedDirection = _playerInputActions.Player.Movement.ReadValue<Vector2>().normalized;
        var cameraBasedDirectionCorrection = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0);
        _currentMovementDirection = cameraBasedDirectionCorrection * new Vector3(normalizedDirection.x, 0, normalizedDirection.y);
    }

    protected override void Move()
    {
        Vector3 newVelocity = _currentMovementDirection * _movementSpeed;
        _rigidBody.velocity = new Vector3 (newVelocity.x, _rigidBody.velocity.y, newVelocity.z);
    }

    protected override void Rotate()
    {
        float horizontal = _playerInputActions.Player.LookAround.ReadValue<Vector2>().x * Time.deltaTime * _rotationSpeed;
        
        if (_currentMovementDirection != Vector3.zero)
        {
            _vikingModel.transform.rotation = Quaternion.Slerp(_vikingModel.transform.rotation,
                Quaternion.LookRotation(_currentMovementDirection), Time.deltaTime * _rotationTowardsDirectionSpeed);
        }
        else
        {
            _vikingModel.transform.Rotate(0, -horizontal, 0);
        }
        
        transform.Rotate(0, horizontal, 0);
    }
    
    
    private bool HasNullReferences()
    {
        if (_camera == null)
        {
            Debug.LogError("Didn't find main camera");
            return true;
        }
        
        if (_animator == null)
        {
            Debug.LogError("Didn't find main animator");
            return true;
        }

        return false;
    }
}
