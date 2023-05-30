using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctionEventHandler : MonoBehaviour
{
    private BasicAnimatorController _animatorController;
    private List<Collider> _weaponColliders;
    
    public void Initialize(BasicAnimatorController animatorController, ref List<Collider> colliders)
    {
        _animatorController = animatorController;
        _weaponColliders = colliders;
    }
    
    public void ResetCanAttackPermission()
    {
        _animatorController.SetAttackPermission(true);
    }

    public void EnableAxeCollider()
    {
        _weaponColliders.ForEach(wc => wc.enabled = true);
    }
    
    public void DisableAxeCollider()
    {
        _weaponColliders.ForEach(wc => wc.enabled = false);
    }
}
