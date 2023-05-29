using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBattleEntity : MonoBehaviour
{
    protected Action OnReceiveDamage;
    protected Action OnDie;
    
    protected eBattleEntityType _entityType;
    protected int _health;
    protected int _damage;

    public virtual void Initialize(BattleEntityData data)
    {
        _entityType = data.EntityType;
        _health = data.Health;
        _damage = data.Damage;
    }
    
    public virtual void ReceiveDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Die();
            return;
        }
        
        OnReceiveDamage?.Invoke();
    }

    protected virtual void Attack() {}

    protected virtual void Move() {}
    
    protected virtual void Rotate() {}

    protected virtual void Die()
    {
        OnDie?.Invoke();
    }
}
