using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterManager
{
    private List<MonsterController> _monsters;
    private Dictionary<MonsterController, Transform> _monsterSpawnPointDictionary;

    private Action _onMonsterDie;

    private Transform _target;
    private float _minRespawnRadius;
    private float _maxRespawnRadius;
    private float AllowedRadiusMargin => _maxRespawnRadius - _minRespawnRadius;
    
    public MonsterManager(List<Transform> spawnPositions, BattleEntity battleEntity, Action onMonsterDeath,
        float minRespawnRadius, float maxRespawnRadius)
    {
        _monsters = new List<MonsterController>();
        _monsterSpawnPointDictionary = new Dictionary<MonsterController, Transform>();
        
        spawnPositions.ForEach(sp =>
        {
            var monster = GameObject.Instantiate(battleEntity.Prefab, sp.position, Quaternion.identity).GetComponent<MonsterController>();
            monster.Initialize(battleEntity.Data);
            monster.OnDie += onMonsterDeath;
            monster.OnFinishedDieAnimation += () => ResetMonster(monster, true);
            _monsters.Add(monster.GetComponent<MonsterController>());
            _monsterSpawnPointDictionary.Add(monster, sp);
        });
        
        _minRespawnRadius = minRespawnRadius;
        _maxRespawnRadius = maxRespawnRadius;
    }

    public void Restart()
    {
        _monsters.ForEach(m => ResetMonster(m, false));
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        _monsters.ForEach(m =>
        {
            m.AssignTarget(_target);
        });
    }

    private void ResetMonster(MonsterController monster, bool revivingDuringFight)
    {
        if (revivingDuringFight && _target == null) return;
        
        var newPosition = revivingDuringFight
            ? GetRandomPositionInPlayerRadius()
            : _monsterSpawnPointDictionary[monster].position;
   
        monster.AssignTarget(null);
        monster.gameObject.SetActive(false);
        monster.transform.SetPositionAndRotation(newPosition, Quaternion.identity);
        (revivingDuringFight ? (Action)monster.IncreaseMaxHealth : monster.ResetMaxHealth)();
        monster.ResetValues();
        monster.gameObject.SetActive(true);
        monster.AssignTarget(_target);
    }

    private Vector3 GetRandomPositionInPlayerRadius()
    {
        var randDirection = Random.insideUnitCircle.normalized;
        var randomizedCenterPoint = randDirection * _minRespawnRadius 
                              + AllowedRadiusMargin * 0.5f * randDirection;
        var randomPosition = new Vector2(_target.position.x, _target.position.z) 
                             + randomizedCenterPoint + Random.insideUnitCircle * AllowedRadiusMargin;
            
        var rayStartPosition = new Vector3(randomPosition.x, 1000, randomPosition.y);
        int rayCastDistance = (int)rayStartPosition.y + 500;
        
        RaycastHit hit;
        if (!Physics.Raycast(rayStartPosition, Vector3.down, out hit, rayCastDistance, 1 << 8))
        {
            Debug.LogError("Something went wrong with raycast for new spawn position");
        }
        
        return hit.point;
    }
}