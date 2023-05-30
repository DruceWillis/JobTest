using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MeleeBattleEntity
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private float _rotationTowardsDirectionSpeed = 40f;
    [SerializeField] private Transform _vikingModelTransform;
    [SerializeField] private Transform _cameraFocusTransform;

    private PlayerInputActions _playerInputActions;
    private BasicAnimatorController _animatorController;
    private AnimationFunctionEventHandler _animationFunctionEventHandler;

    private Rigidbody _rigidBody;
    private Camera _camera;

    private Vector3 _currentMovementDirection;
    private float _horizontalMouseMovement;
    private bool _initiatedAttack;
    private bool _isDead;

    private bool _successfullyInitialized;


    public override void Initialize(BattleEntityData data)
    {
        base.Initialize(data);
        
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _animatorController = new BasicAnimatorController(_animator);

        _animationFunctionEventHandler = _vikingModelTransform.GetComponent<AnimationFunctionEventHandler>();
        
        _rigidBody = GetComponent<Rigidbody>();

        InitializeWeaponControllers(() =>
        {
            _animationFunctionEventHandler.Initialize(_animatorController, ref _weaponColliders);
        });
        
        _successfullyInitialized = !HasNullReferences();

        if (!_successfullyInitialized) return;
        
        if (_camera.TryGetComponent(out MouseAimCamera aimCamera))
        {
            aimCamera.SetTarget(_cameraFocusTransform);
        }
    }

    public override void ResetValues()
    {
        base.ResetValues();
        _isDead = false;
        _animatorController.ResetValues();
    }

    public void SetPlayerCamera(Camera cam)
    {
        _camera = cam;
    }

    private void Update()
    {
        if (!_successfullyInitialized || _isDead) return;

        HandleInput();
        Move();
        Rotate();
        HandleAnimation();
        ResetNecessaryInput();
    }

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

    protected override void Die()
    {
        base.Die();
        _isDead = true;
        _rigidBody.velocity = Vector3.zero;
    }

    protected override void HandleAnimation()
    {
        // FOR DEBUGGING PURPOSES
        bool rh = Input.GetKeyDown(KeyCode.H);
        if (rh)
            ReceiveDamage(10);

        Helpers.AnimatorUpdateData animatorUpdateData = new Helpers.AnimatorUpdateData
        {
            Speed = _currentMovementDirection.magnitude,
            ReceivedHit = rh,
            InitiatedAttack = _initiatedAttack,
            Died = _isDead
        };
        _animatorController.Update(animatorUpdateData);
    }
    
    protected override bool HasNullReferences()
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
        
        if (_animationFunctionEventHandler == null)
        {
            Debug.LogError("Didn't find animation function event handler");
            return true;
        }

        if (_weaponColliders.Count <= 0)
        {
            Debug.LogError("Didn't find weapon colliders");
            return true;
        }
        
        return false;
    }

}
