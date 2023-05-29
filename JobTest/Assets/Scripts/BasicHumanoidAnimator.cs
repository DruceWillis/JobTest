using UnityEngine;

public class BasicHumanoidAnimator
{
    private Animator _animator;
    private bool _isRunning;
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");

    public BasicHumanoidAnimator(Animator animator)
    {
        _animator = animator;
    }

    public void Update(float speed)
    {
        _isRunning = speed > 0;
        
        _animator.SetBool(IsRunning, _isRunning);
    }
}
