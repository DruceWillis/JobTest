using System;

public class MonsterIdleState : MonsterState
{
    public MonsterIdleState(Action logicToExecute, Action onEnteredLogic) : base(logicToExecute, onEnteredLogic)
    {
        _onExecute = logicToExecute;
        _onEnter = onEnteredLogic;
    }
}