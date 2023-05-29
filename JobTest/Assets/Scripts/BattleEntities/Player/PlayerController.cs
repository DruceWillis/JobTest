using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : BaseBattleEntity
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private float _rotationTowardsDirectionSpeed = 40f;
    [SerializeField] private Transform _vikingModelTransform;
    [SerializeField] private Transform _cameraFocusTransform;
    
    private PlayerInputActions _playerInputActions;
    private BasicHumanoidAnimatorController _basicAnimatorController;
    
    private Rigidbody _rigidBody;
    private Camera _camera;

    private eHumanoidBattleEntityState _state;
    private Vector3 _currentMovementDirection;
    private float _horizontalMouseMovement;
    private bool _initiatedAttack;

    private bool _successfullyInitialized;
    
    private void Awake()
    {
        _state = eHumanoidBattleEntityState.Idle;
        
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _basicAnimatorController = new BasicHumanoidAnimatorController(_animator);
        
        _rigidBody = GetComponent<Rigidbody>();
        
        _camera = Camera.main;
        _successfullyInitialized = !HasNullReferences();

        if (!_successfullyInitialized) return;
        
        if (_camera.TryGetComponent(out MouseAimCamera aimCamera))
        {
            aimCamera.SetTarget(_cameraFocusTransform);
        }
    }

    private void Update()
    {
        if (!_successfullyInitialized) return;

        HandleInput();
        Move();
        Rotate();
        HandleAnimation();
        ResetNecessaryInput();
        
        
    }

    /*
    private void HandleIdleState()
    {
        GatherInput();
        Move();
        Rotate();
    }
    
    private void HandleRunningState()
    {
        GatherInput();
        Move();
        Rotate();
    }
    
    private void HandleAttackingState()
    {
        
    }
    
    private void HandleReceivingDamageState()
    {
        
    }
    
    private void HandleDieState()
    {
        
    }
    */

    private void HandleInput()
    {
        // Movement
        var normalizedDirection = _playerInputActions.Player.Movement.ReadValue<Vector2>().normalized;
        var cameraBasedDirectionCorrection = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0);
        _currentMovementDirection = cameraBasedDirectionCorrection * new Vector3(normalizedDirection.x, 0, normalizedDirection.y);

        // Look around
        _horizontalMouseMovement = _playerInputActions.Player.LookAround.ReadValue<Vector2>().x;

        if (_playerInputActions.Player.Fire.WasPerformedThisFrame())
        {
            Attack();
        }
    }
    
    private void ResetNecessaryInput()
    {
        _initiatedAttack = false;
    }

    protected override void Move()
    {
        Vector3 newVelocity = _currentMovementDirection * _movementSpeed;
        _rigidBody.velocity = new Vector3 (newVelocity.x, _rigidBody.velocity.y, newVelocity.z);
    }

    protected override void Rotate()
    {
        _horizontalMouseMovement *= Time.deltaTime * _rotationSpeed;
        
        if (_currentMovementDirection != Vector3.zero)
        {
            _vikingModelTransform.transform.rotation = Quaternion.Slerp(_vikingModelTransform.transform.rotation,
                Quaternion.LookRotation(_currentMovementDirection), Time.deltaTime * _rotationTowardsDirectionSpeed);
        }
        else
        {
            _vikingModelTransform.transform.Rotate(0, -_horizontalMouseMovement, 0);
        }
        
        transform.Rotate(0, _horizontalMouseMovement, 0);
    }

    protected override void Attack()
    {
        _initiatedAttack = true;
    }

    protected override void HandleAnimation()
    {
        bool rh = Input.GetKeyDown(KeyCode.H);
        Helpers.AnimatorUpdateData animatorUpdateData = new Helpers.AnimatorUpdateData
        {
            Speed = _currentMovementDirection.magnitude,
            ReceivedHit = rh,
            InitiatedAttack = _initiatedAttack,
            Died = false
        };
        _basicAnimatorController.Update(animatorUpdateData);
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
    
    
    // ANIMATION EVENTS

    public void SetAttackPermission()
    {
        
    }

}
