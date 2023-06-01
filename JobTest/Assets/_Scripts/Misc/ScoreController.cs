using System;

public class ScoreController
{
    public Action OnStartedFighting;
    public Action<int> OnScoreChanged;
    
    private int _currentScore;

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