using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class MeleeBattleEntity : BaseBattleEntity
{
    protected List<WeaponController> _weaponControllers;
    protected List<Collider> _weaponColliders;

    protected void InitializeWeaponControllers(WeaponData data, int holderBaseDamage,
        Action OnInitializationComplete = null)
    {
        _weaponControllers = GetComponentsInChildren<WeaponController>().ToList();
        _weaponColliders = new List<Collider>();
        
        _weaponControllers.ForEach(wc =>
        {
            wc.Initialize(data, holderBaseDamage);
            
            if (wc.TryGetComponent(out Collider weaponCollider))
            {
                _weaponColliders.Add(weaponCollider);
            }
            else
            {
                Debug.LogError("Weapon collider does not have weapon controller assigned");
            }
        });
        
        OnInitializationComplete?.Invoke();
    }
}
