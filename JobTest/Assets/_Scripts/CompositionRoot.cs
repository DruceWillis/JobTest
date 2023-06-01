using System.Collections.Generic;
using UnityEngine;

public class CompositionRoot : MonoBehaviour
{
    [SerializeField] private MainCanvas _mainCanvas;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private List<Transform> _monstersSpawnPositions;
    [SerializeField] private BattleEntitiesConfig _battleEntitiesConfig;
    
    public MonsterManager _monstersManager;

    private PlayerController _playerController;
    private ScoreController _scoreController;

    private bool _initializedPlayer;
    
    private void Awake()
    {
        if (HasNullReferences()) return;

        GameStateController.Instance.GameState = eGameState.MainMenu;
        _scoreController = new ScoreController();

        _monstersManager = new MonsterManager(_monstersSpawnPositions,
            _battleEntitiesConfig.GetBattleEntityByType(eBattleEntityType.Monster),
            _scoreController.KilledMonster);
        
        _mainCanvas.Initialize(_scoreController);
        _mainCanvas.OnOpenMainMenu += OnOpenMainMenu;
        _mainCanvas.OnStartFighting += OnStartFighting;
        
        _cameraManager.Initialize(_mainCanvas);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameStateController.Instance.GameState = eGameState.MainMenu;
        }
    }
    

    private void OnOpenMainMenu()
    {
        _playerController.gameObject.SetActive(false);
        _monstersManager.Restart();
        _monstersManager.SetTarget(null);
    }

    private void OnStartFighting()
    {
        HandlePlayerOnStartFighting();
        _monstersManager.Restart();
        _monstersManager.SetTarget(_playerController.transform);
    }

    private void HandlePlayerOnStartFighting()
    {
        if (_initializedPlayer)
        {
            _playerController.transform.position = _playerSpawnPoint.position;
            _playerController.transform.rotation = _playerSpawnPoint.rotation;
            _playerController.ResetValues();
            _playerController.gameObject.SetActive(true);
        }
        else
        {
            var vikingBattleEntity = _battleEntitiesConfig.GetBattleEntityByType(eBattleEntityType.Viking);
            _playerController = Instantiate(vikingBattleEntity.Prefab, _playerSpawnPoint.position, Quaternion.identity)
                .GetComponent<PlayerController>();
            _playerController.SetPlayerCamera(_cameraManager.Camera);
            _playerController.Initialize(vikingBattleEntity.Data);
            _playerController.OnHealthPercentChanged += _mainCanvas.FightingScreen.UpdateHealthBar;
            _initializedPlayer = true;
        }
    }

    private void OnDestroy()
    {
        _mainCanvas.OnOpenMainMenu -= OnOpenMainMenu;
        _mainCanvas.OnStartFighting -= OnStartFighting;
    }
    
    private bool HasNullReferences()
    {
        if (_playerSpawnPoint == null)
        {
            Debug.LogError("PlayerSpawnPoint wasn't assigned");
            return true;
        }
        
        if (_mainCanvas == null)
        {
            Debug.LogError("MainCanvas wasn't assigned");
            return true;
        }
        
        if (_cameraManager == null)
        {
            Debug.LogError("CameraManager wasn't assigned");
            return true;
        }

        if (_battleEntitiesConfig == null)
        {
            Debug.LogError("BattleEntitiesConfig wasn't assigned");
            return true;
        }
        
        return false;
    }
}
