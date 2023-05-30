using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Collider _collider;
    private WeaponData _data;

    public void Initialize(WeaponData data, bool isTrigger = true)
    {
        if (TryGetComponent(out Collider col))
        {
            _collider = col;
            _collider.enabled = false;
            _collider.isTrigger = isTrigger;
        }
        else
        {
            Debug.LogError("Weapon does not have a collider");
        }
        _data = data;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out BaseBattleEntity battleEntity)) return;

        if (_data.CanDamageEntityByType(battleEntity.EntityType))
        {
            battleEntity.ReceiveDamage(_data.Damage);
        }
    }
}
