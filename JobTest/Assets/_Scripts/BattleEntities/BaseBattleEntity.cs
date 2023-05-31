using System;
using UnityEngine;

public abstract class BaseBattleEntity : MonoBehaviour
{
    protected BattleEntityData _data;
 
    protected Action OnDie;
    
    protected eBattleEntityType _entityType;
    protected int _maxHealth;
    protected int _health;
    protected int _baseDamage;

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
    }
    
    public virtual void ReceiveDamage(int damage)
    {
        _health -= damage;
        _health = _health < 0 ? 0 : _health;
        
        OnHealthPercentChanged?.Invoke((float)_health / _maxHealth, 0.25f);
        
        if (_health <= 0)
        {
            Die();
            return;
        }
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
