using System;

public class ScoreController
{
    private int _currentScore;

    public Action OnStartedFighting;
    public Action<int> OnScoreChanged;

    public ScoreController()
    {
        GameStateController.Instance.OnGameStateChanged += state =>
        {
            if (state == eGameState.Fighting)
            {
                StartFighting();
            }
        };
    }
    
    public int CurrentScore
    {
        get => _currentScore;
        set
        {
            _currentScore = value;
            OnScoreChanged?.Invoke(_currentScore);
        }   
    }

    public void StartFighting()
    {
        CurrentScore = 0;
    }
    
    public void KilledMonster()
    {
        CurrentScore++;
    }
}