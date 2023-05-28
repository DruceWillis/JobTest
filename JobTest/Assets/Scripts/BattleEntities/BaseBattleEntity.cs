using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBattleEntity : MonoBehaviour
{
    [SerializeField]
    protected eBattleEntityType _entityType;
    protected int _health;
    protected int _damage;

    public void Initialize(BattleEntityData data)
    {
        
    }
    
    public void TakeHit()
    {
        
    }

    public void Attack()
    {
        
    }
}
