using System;

public class GameStateController
{
    private static GameStateController _instance;
    private eGameState _gameState;

    public Action<eGameState> OnGameStateChanged;

    public static GameStateController Instance => _instance ??= new GameStateController();

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
        _gameState = eGameState.MainMenu;
    }
}
