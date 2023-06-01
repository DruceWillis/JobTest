using System;

public class MonsterChaseState : MonsterState
{
    public MonsterChaseState(Action logicToExecute, Action onEnteredLogic) : base(logicToExecute, onEnteredLogic)
    {
        _onExecute = logicToExecute;
        _onEnter = onEnteredLogic;
    }
}