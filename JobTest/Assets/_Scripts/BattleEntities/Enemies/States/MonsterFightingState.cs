using System;

public class MonsterFightingState : MonsterState
{
    public MonsterFightingState(Action logicToExecute, Action onEnteredLogic) : base(logicToExecute, onEnteredLogic)
    {
        _onExecute = logicToExecute;
        _onEnter = onEnteredLogic;
    }
}