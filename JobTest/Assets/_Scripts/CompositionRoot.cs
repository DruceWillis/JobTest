using System;
using System.Collections.Generic;
using UnityEngine;

public class CompositionRoot : MonoBehaviour
{
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private BattleEntitiesConfig _battleEntitiesConfig;

    private PlayerController _playerController;

    private void Awake()
    {
        if (_playerSpawnPoint == null)
        {
            Debug.LogError("Player spawn point wasn't assigned");
        }

        // return;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            var vikingBattleEntity = _battleEntitiesConfig.GetBattleEntityByType(eBattleEntityType.Viking);
            GameStateController.Instance.GameState = eGameState.Fighting;
            _playerController = Instantiate(vikingBattleEntity.Prefab, _playerSpawnPoint.position, Quaternion.identity).GetComponent<PlayerController>();
            _playerController.SetPlayerCamera(_cameraManager.Camera);
            _playerController.Initialize(vikingBattleEntity.Data);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameStateController.Instance.GameState = eGameState.InMainMenu;
        }
    }
}
