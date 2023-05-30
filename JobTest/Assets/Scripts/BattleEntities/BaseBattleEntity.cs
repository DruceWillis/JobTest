using System;
using UnityEngine;

public abstract class BaseBattleEntity : MonoBehaviour
{
    protected BattleEntityData _data;
 
    protected Action OnReceiveDamage;
    protected Action OnDie;
    
    protected eBattleEntityType _entityType;
    protected int _health;
    protected int _baseDamage;

    public eBattleEntityType EntityType => _entityType;

    public virtual void Initialize(BattleEntityData data)
    {
        _entityType = data.EntityType;
        _health = data.Health;
        _baseDamage = data.BaseDamage;
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
    
    protected virtual void Move() {}
    protected virtual void Rotate() {}
    protected virtual void Attack() {}
    protected virtual void HandleAnimation() {}

    protected virtual void Die()
    {
        OnDie?.Invoke();
    }

    protected abstract bool HasNullReferences();
}
