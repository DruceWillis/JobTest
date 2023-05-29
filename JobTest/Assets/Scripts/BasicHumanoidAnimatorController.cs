using UnityEngine;

public class BasicHumanoidAnimatorController
{
    private Animator _animator;
   
    private bool _isRunning;
    private bool _canAttack;
    
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int CanAttack = Animator.StringToHash("CanAttack");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int ReceivedDamage = Animator.StringToHash("ReceivedDamage");

    public BasicHumanoidAnimatorController(Animator animator)
    {
        _animator = animator;
        _canAttack = true;
        _animator.SetBool(CanAttack, _canAttack);
    }

    public void Update(Helpers.AnimatorUpdateData data)
    {
        bool prevRunningState = _isRunning;
        _isRunning = data.Speed > 0.05f;
        
        if (prevRunningState != _isRunning)
        {
            _animator.SetBool(IsRunning, _isRunning);
        }
        
        if (data.InitiatedAttack)
        {
            _animator.SetTrigger(Attack);
        }

        if (data.ReceivedHit)
        {
            _animator.SetTrigger(ReceivedDamage);
        }
    }
}
