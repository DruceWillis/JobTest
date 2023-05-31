using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class MonsterController : MeleeBattleEntity
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private Transform _monsterModelTransform;

    private BasicAnimatorController _animatorController;
    private AnimationFunctionEventHandler _animationFunctionEventHandler;

    private Rigidbody _rigidBody;

    private Vector3 _currentMovementDirection;
    private bool _initiatedAttack;
    
    public override void Initialize(BattleEntityData data)
    {
        base.Initialize(data);
        
        _animatorController = new BasicAnimatorController(_animator);

        _animationFunctionEventHandler = _monsterModelTransform.GetComponent<AnimationFunctionEventHandler>();
        
        _rigidBody = GetComponent<Rigidbody>();

        InitializeWeaponControllers(_data.Weapon, () =>
        {
            _animationFunctionEventHandler.Initialize(_animatorController, ref _weaponColliders);
        });
        
        _successfullyInitialized = !HasNullReferences();

        if (!_successfullyInitialized) return;
    }

    private void Update()
    {
        if (!_successfullyInitialized || _isDead) return;
            
        Move();
        Rotate();
        HandleAnimation();
    }

    protected override void Move()
    {
        throw new System.NotImplementedException();
    }

    protected override void Rotate()
    {
        throw new System.NotImplementedException();
    }

    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }

    protected override void HandleAnimation()
    {
        throw new System.NotImplementedException();
    }

    protected override bool HasNullReferences()
    {
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
