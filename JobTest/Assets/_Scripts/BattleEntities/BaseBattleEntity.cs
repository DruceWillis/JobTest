using System;
using UnityEngine;

public abstract class BaseBattleEntity : MonoBehaviour
{
    protected BattleEntityData _data;

    protected eBattleEntityType _entityType;
    protected int _maxHealth;
    protected int _health;
    protected int _baseDamage;
    protected bool _isDead;
    protected bool _successfullyInitialized;
    protected bool _receivedDamage;
    
    public Action<float, float> OnHealthPercentChanged;
    public eBattleEntityType EntityType => _entityType;

    public virtual void Initialize(BattleEntityData data)
    {
        _data = data;
        _entityType = _data.EntityType;
        _maxHealth = _data.Health;
        _health = _data.Health;
        _baseDamage = _data.BaseDamage;
    }

    public virtual void ResetValues()
    {
        _health = _maxHealth;
        OnHealthPercentChanged?.Invoke((float)_health / _maxHealth, 0f);
        _isDead = false;
    }
    
    public virtual void ReceiveDamage(int damage)
    {
        if (_isDead) return;
        
        _health -= damage;
        _health = _health < 0 ? 0 : _health;
        
        OnHealthPercentChanged?.Invoke((float)_health / _maxHealth, 0.25f);
        
        if (_health <= 0)
        {
            Die();
            return;
        }

        _receivedDamage = true;
    }
    
    public virtual void ReceiveHeal(int healAmount)
    {
        if (_isDead) return;
        
        _health = _health + healAmount > _maxHealth ? _maxHealth : _health + healAmount;
        
        OnHealthPercentChanged?.Invoke((float)_health / _maxHealth, 0.25f);
    }

    protected abstract void Move();
    protected abstract void Rotate();
    protected abstract void Attack();
    protected abstract void HandleAnimation();

    protected virtual void Die()
    {
        _isDead = true;
        HandleAnimation();
    }

    protected abstract bool HasNullReferences();
}
