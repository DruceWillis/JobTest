using System;
using UnityEngine;
using UnityEngine.AI;

// [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class MonsterController : MeleeBattleEntity
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _monsterModelTransform;

    private BasicAnimatorController _animatorController;
    private AnimationFunctionEventHandler _animationFunctionEventHandler;

    private Rigidbody _rigidBody;
    private Collider _collider;
    private NavMeshAgent _navMeshAgent;
    private Transform _target;

    private Vector3 _currentMovementDirection;
    private bool _initiatedAttack;
    private bool _acquiredTarget;
    private bool _reachedTarget;

    private void Awake()
    {
        // base.Initialize(null);
        
        _animatorController = new BasicAnimatorController(_animator, false);
        _entityType = eBattleEntityType.Monster;
        _health = 2;
        _animationFunctionEventHandler = _monsterModelTransform.GetComponent<AnimationFunctionEventHandler>();

        _rigidBody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        // InitializeWeaponControllers(_data.Weapon, _baseDamage, () =>
        // {
        //     _animationFunctionEventHandler.Initialize(_animatorController, ref _weaponColliders);
        // });
        
        _successfullyInitialized = !HasNullReferences();

        if (!_successfullyInitialized) return;
    }

    public override void Initialize(BattleEntityData data)
    {
        base.Initialize(data);
        
        _animatorController = new BasicAnimatorController(_animator, false);

        _animationFunctionEventHandler = _monsterModelTransform.GetComponent<AnimationFunctionEventHandler>();
        
        _rigidBody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        InitializeWeaponControllers(_data.Weapon, _baseDamage, () =>
        {
            _animationFunctionEventHandler.Initialize(_animatorController, ref _weaponColliders);
        });
        
        _successfullyInitialized = !HasNullReferences();

        if (!_successfullyInitialized) return;
    }
    
    public override void ResetValues()
    {
        base.ResetValues();
        _animatorController.ResetValues();
        _collider.enabled = true;
        
        if (!gameObject.activeSelf) return;
        _animator.Rebind();
        _animator.Update(0f);
    }

    public void AssignTarget(Transform target)
    {
        _target = target;
        _acquiredTarget = true;
    }

    private void Update()
    {
        if (!_successfullyInitialized || _isDead) return;

        if (_acquiredTarget)
        { 
            Move();
            Rotate();
        }
        else
        {
            var player = FindObjectOfType<PlayerController>();
            if (player)
            {
                AssignTarget(player.transform);
            }
        }
        
        HandleAnimation();
    }

    protected override void Move()
    {
        _navMeshAgent.SetDestination(_target.position);

        _reachedTarget = _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance;
    }

    protected override void Rotate()
    {
        _monsterModelTransform.LookAt(_target);
    }

    protected override void Attack()
    {
        // throw new System.NotImplementedException();
    }
    
    protected override void Die()
    {
        base.Die();
        _collider.enabled = false;
    }

    protected override void HandleAnimation()
    {
        Helpers.AnimatorUpdateData animatorUpdateData = new Helpers.AnimatorUpdateData
        {
            IsRunning = !_reachedTarget,
            ReceivedHit = _receivedDamage,
            InitiatedAttack = _initiatedAttack,
            Died = _isDead
        };
        _animatorController.Update(animatorUpdateData);

        _initiatedAttack = false;
        _receivedDamage = false;
    }

    protected override bool HasNullReferences()
    {
        if (_animator == null)
        {
            Debug.LogError("Didn't find main animator");
            return true;
        }
        
        // if (_animationFunctionEventHandler == null)
        // {
        //     Debug.LogError("Didn't find animation function event handler");
        //     return true;
        // }
        //
        // if (_weaponColliders.Count <= 0)
        // {
        //     Debug.LogError("Didn't find weapon colliders");
        //     return true;
        // }
        //
        return false;
    }
}
