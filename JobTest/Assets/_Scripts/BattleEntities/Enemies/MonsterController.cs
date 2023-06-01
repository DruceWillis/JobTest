using System;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MeleeBattleEntity
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _monsterModelTransform;
    [SerializeField] private MonsterHealthBarController _healthBarController;
    [SerializeField] private Collider _collider;

    private BasicAnimatorController _animatorController;
    private AnimationFunctionEventHandler _animationFunctionEventHandler;

    private NavMeshAgent _navMeshAgent;
    private Transform _target;
    private Camera _camera;

    private MonsterState _currentState;
    private MonsterIdleState _idleState;
    private MonsterChaseState _chaseState;
    private MonsterFightingState _fightingState;

    private float _rotationTowardsTargetSpeed = 3f;
    
    private bool _initiatedAttack;
    private bool _acquiredTarget;
    private bool _reachedTarget;
    private bool _isTargetInFieldOfView;
    private bool _canChase = true;

    public Action OnFinishedDieAnimation;
    
    public MonsterState CurrentState
    {
        get => _currentState;
        set 
        {
            _currentState = value;
            _currentState.OnEnteredState();
        }
    }
    
    public override void Initialize(BattleEntityData data)
    {
        base.Initialize(data);
        
        _animatorController = new BasicAnimatorController(_animator, false);

        _animationFunctionEventHandler = _monsterModelTransform.GetComponent<AnimationFunctionEventHandler>();
        _animationFunctionEventHandler.OnDie = () => OnFinishedDieAnimation?.Invoke();
        _animationFunctionEventHandler.OnFinishedInPlaceAnimation = () => _canChase = true;

        InitializeWeaponControllers(_data.Weapon, _baseDamage, () =>
        {
            _animationFunctionEventHandler.Initialize(_animatorController, ref _weaponColliders);
        });
        
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        _idleState = new MonsterIdleState(IdleStateLogic, OnEnteredIdleState);
        _chaseState = new MonsterChaseState(ChaseStateLogic, OnEnteredChaseState);
        _fightingState = new MonsterFightingState(FightingStateLogic, OnEnteredFightingState);
        CurrentState = _idleState;
        
        OnHealthPercentChanged += _healthBarController.UpdateHealthBar;

        _successfullyInitialized = !HasNullReferences();

        if (!_successfullyInitialized) return;
    }
        
    private void Update()
    {
        if (!_successfullyInitialized || _isDead) return;
        
        CurrentState.ExecuteState();

        _healthBarController.LookAtPlayerCamera(_camera.transform.position);
        
        HandleAnimation();
    }

    public override void ResetValues()
    {
        base.ResetValues();

        _animatorController.ResetValues();
        _collider.enabled = true;

        if (!gameObject.activeSelf) return;
        
        _navMeshAgent.isStopped = false;
        
        _animator.Rebind();
        _animator.Update(0f);
    }

    public override void ReceiveDamage(int damage)
    {
        base.ReceiveDamage(damage);
        
        _canChase = false;
    }
    
    public void IncreaseMaxHealth()
    {
        _maxHealth++;
    }
    public void ResetMaxHealth()
    {
        _maxHealth = _data.Health;
    }

    public void AssignTarget(Transform target)
    {
        _target = target;
        
        if (_target != null)
        {
            _acquiredTarget = true;
            _canChase = true;
            CurrentState = _chaseState;
        }
        else
        {
            _acquiredTarget = false;
            _canChase = false;
            CurrentState = _idleState;
        }
    }
    
    public void AssignCamera(Camera cam)
    {
        _camera = cam;
    }

    protected override void Move()
    {
        if (!_acquiredTarget) return;
        
        _isTargetInFieldOfView = Vector3.Dot(_monsterModelTransform.transform.forward,
            (_target.position - _monsterModelTransform.transform.position).normalized) > 0.2f;

        _navMeshAgent.isStopped = !_isTargetInFieldOfView;
        _navMeshAgent.SetDestination(_target.position);
        
        UpdateReachedTarget();
        
        if (_reachedTarget)
        {
            CurrentState = _fightingState;
        }
    }

    protected override void Rotate()
    {
        _monsterModelTransform.transform.rotation = Quaternion.Slerp(_monsterModelTransform.transform.rotation,
            Quaternion.LookRotation((_target.position - transform.position).normalized), Time.deltaTime * _rotationTowardsTargetSpeed);
    }

    protected override void Attack()
    {
        _initiatedAttack = true;
        _canChase = false;
    }

    protected override void Die()
    {
        base.Die();
        
        _collider.enabled = false;
        _navMeshAgent.isStopped = true;
    }

    protected override void HandleAnimation()
    {
        Helpers.AnimatorUpdateData animatorUpdateData = new Helpers.AnimatorUpdateData
        {
            IsRunning = !_reachedTarget && _acquiredTarget && _isTargetInFieldOfView,
            ReceivedHit = _receivedDamage,
            InitiatedAttack = _initiatedAttack,
            Died = _isDead
        };
        
        _animatorController.Update(animatorUpdateData);

        _initiatedAttack = false;
        _receivedDamage = false;
    }

    private void OnEnteredIdleState()
    {
        _navMeshAgent.isStopped = true;
    }
    
    private void IdleStateLogic() { }

    private void OnEnteredChaseState() { }
    
    private void ChaseStateLogic()
    {
        Rotate();
        Move();
    }
    
    private void OnEnteredFightingState()
    {
        _navMeshAgent.isStopped = true;
    }

    private void FightingStateLogic()
    {
        _navMeshAgent.SetDestination(_target.position);
        
        UpdateReachedTarget();
        
        if (_reachedTarget)
        {
            UpdateTargetInFieldOfView();
            if (_isTargetInFieldOfView)
            {
                Attack();
            }
            else if (_animatorController.CanAttack)
            {
                Rotate();
            }
        }
        else if (_canChase)
        {
            CurrentState = _chaseState;
        }
    }

    private void UpdateReachedTarget()
    {
        if (_navMeshAgent.pathPending) return;
        
        _reachedTarget = _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance;
    }
    
    private void UpdateTargetInFieldOfView()
    {
        _isTargetInFieldOfView = Vector3.Dot(_monsterModelTransform.transform.forward,
            (_target.position - _monsterModelTransform.transform.position).normalized) > 0.02f;
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
