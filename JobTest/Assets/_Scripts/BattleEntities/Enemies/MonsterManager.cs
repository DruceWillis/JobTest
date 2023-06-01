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
    
    public MonsterManager(List<Transform> spawnPositions, BattleEntity battleEntity, Action onMonsterDeath)
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
    }

    public void Restart()
    {
        _monsters.ForEach(m => ResetMonster(m, true));
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        _monsters.ForEach(m =>
        {
            m.AssignTarget(_target);
        });
    }

    private void ResetMonster(MonsterController monster, bool useOriginalSpawnPosition)
    {
        var newPosition = useOriginalSpawnPosition
            ? _monsterSpawnPointDictionary[monster].position
            : GetRandomPositionInPlayerRadius();
        
        monster.AssignTarget(null);
        monster.gameObject.SetActive(false);
        monster.transform.SetPositionAndRotation(newPosition, Quaternion.identity);
        monster.ResetValues();
        monster.gameObject.SetActive(true);
        monster.AssignTarget(_target);
    }

    private Vector3 GetRandomPositionInPlayerRadius()
    {
        return Vector3.one * 2;
    }
}