using UnityEngine;

public class BasicAnimatorController
{
    private Animator _animator;
   
    private bool _isRunning;
    private bool _canAttack;
    private bool _isDead;
    private bool _usesUpperBodyMask;
    
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Die = Animator.StringToHash("Die");
    
    
    public BasicAnimatorController(Animator animator, bool usesUpperBodyMask)
    {
        _animator = animator;
        _canAttack = true;
        _usesUpperBodyMask = usesUpperBodyMask;
    }

    public void ResetValues()
    {
        _isDead = false;
        SetAttackPermission(true);
    }

    public void SetAttackPermission(bool canAttack)
    {
        _canAttack = canAttack;
    }

    public void Update(Helpers.AnimatorUpdateData data)
    {
        if (data.Died)
        {
            _isDead = true;
            Debug.LogError("dead");
            _animator.SetTrigger(Die);
        }

        if (_isDead) return;
        
        bool prevRunningState = _isRunning;
        _isRunning = data.IsRunning;
        
        if (prevRunningState != _isRunning)
        {
            _animator.SetBool(IsRunning, _isRunning);
        }
        
        if (data.InitiatedAttack && _canAttack)
        {
            _canAttack = false;
            _animator.SetTrigger(Attack);
        }
        
        if (data.ReceivedHit)
        {
            // Using _animator.Play instead of SetTrigger to make it possible to restart
            // animation from the beginning with each hit taken 

            _animator.Play("ReceivedDamage", _usesUpperBodyMask ? 1 : 0, 0.0f);
        }
    }
}
