using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctionEventHandler : MonoBehaviour
{
    private BasicAnimatorController _animatorController;
    private List<Collider> _weaponColliders;

    public Action OnFinishedInPlaceAnimation;
    public Action OnDie;

    public void Initialize(BasicAnimatorController animatorController, ref List<Collider> colliders)
    {
        _animatorController = animatorController;
        _weaponColliders = colliders;
    }

    public void ResetCanAttackPermission()
    {
        _animatorController.SetAttackPermission(true);
    }

    public void EnableWeaponColliders()
    {
        _weaponColliders.ForEach(wc => wc.enabled = true);
    }
    
    public void DisableWeaponColliders()
    {
        _weaponColliders.ForEach(wc => wc.enabled = false);
    }

    public void FinishedInPlaceAnimation()
    {
        OnFinishedInPlaceAnimation?.Invoke();
    }
    
    public void OnFinishedDieAnimation()
    {
        OnDie?.Invoke();
    }
}
