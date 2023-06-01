using System;
using UnityEngine;

public abstract class MonsterState
{
    protected Action _onExecute;
    protected Action _onEnter;

    public MonsterState(Action executeLogic, Action onEnteredLogic)
    {
        _onExecute = executeLogic;
        _onEnter = onEnteredLogic;
    }
    
    public virtual void OnEnteredState()
    {
        _onEnter?.Invoke();
    }
    
    public virtual void ExecuteState()
    {
        // Debug.LogError(GetType().Name);
        _onExecute?.Invoke();
    }
}   