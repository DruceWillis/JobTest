using System;
using System.Collections.Generic;
using UnityEngine;

public class CompositionRoot : MonoBehaviour
{
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private List<Transform> _monstersStartingSpawnPoints;
    
    private BattleEntitiesConfig _battleEntitiesConfig;
    private const string ConfigsPath = "Resources/Configs";
    private const string BattleEntitiesConfigPath = ConfigsPath + "/BattleEntities";

    private void Awake()
    {
        _battleEntitiesConfig = Resources.Load<BattleEntitiesConfig>(BattleEntitiesConfigPath);
    }
}
