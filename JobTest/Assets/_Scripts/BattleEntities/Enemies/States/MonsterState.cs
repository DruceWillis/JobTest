﻿using System;

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
        _onExecute?.Invoke();
    }
}   