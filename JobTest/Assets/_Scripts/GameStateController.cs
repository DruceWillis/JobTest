using System;

public class GameStateController
{
    public Action<eGameState> OnGameStateChanged;

    private static GameStateController _instance;

    public static GameStateController Instance => _instance ??= new GameStateController();

    private eGameState _gameState;

    
    public eGameState GameState
    {
        get => _gameState;
        
        set
        {
            _gameState = value;
            OnGameStateChanged?.Invoke(value);
        }
    }
    
    
    public GameStateController()
    {
        if (_instance != null) return; 
        
        _instance = this;
        _gameState = eGameState.InMainMenu;
    }
}

public enum eGameState
{
    InMainMenu,
    Fighting,
    GameOverScreen
}
