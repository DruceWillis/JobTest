using System;
using UnityEngine;

public class CompositionRoot : MonoBehaviour
{
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private BattleEntitiesConfig _battleEntitiesConfig;

    private PlayerController _playerController;

    private bool _initializedPlayer;
    
    private void Awake()
    {
        if (_playerSpawnPoint == null)
        {
            Debug.LogError("Player spawn point wasn't assigned");
        }

        GameStateController.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameStateController.Instance.GameState = eGameState.Fighting;

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameStateController.Instance.GameState = eGameState.MainMenu;
        }
    }
    
    private void OnGameStateChanged(eGameState state)
    {
        switch (state)
        {
            case eGameState.MainMenu:
                OnOpenMainMenu();
                break;
            case eGameState.Fighting:
                OnStartFighting();
                break;
        }
    }
    
    private void OnOpenMainMenu()
    {
        _playerController.gameObject.SetActive(false);
    }
    
        
    private void OnStartFighting()
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
            _initializedPlayer = true;
        }
    }

    private void OnDestroy()
    {
        GameStateController.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
}
