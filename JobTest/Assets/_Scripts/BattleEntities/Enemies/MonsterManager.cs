using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager
{
    private List<MonsterController> _monsters;
    private Dictionary<MonsterController, Transform> _monsterSpawnPointDictionary;

    private Action _onMonsterDie;
    public MonsterManager(List<Transform> spawnPositions, BattleEntity battleEntity, Action onMonsterDeath)
    {
        _monsters = new List<MonsterController>();
        _monsterSpawnPointDictionary = new Dictionary<MonsterController, Transform>();
        
        spawnPositions.ForEach(sp =>
        {
            var monster = GameObject.Instantiate(battleEntity.Prefab, sp.position, Quaternion.identity).GetComponent<MonsterController>();
            monster.Initialize(battleEntity.Data);
            monster.OnDie += onMonsterDeath;
            _monsters.Add(monster.GetComponent<MonsterController>());
            _monsterSpawnPointDictionary.Add(monster, sp);
        });
    }

    public void Restart()
    {
        _monsters.ForEach(m =>
        {
            m.gameObject.SetActive(true);
            m.ResetValues();
            m.transform.SetPositionAndRotation(_monsterSpawnPointDictionary[m].position, Quaternion.identity);
        });
    }

    public void SetTarget(Transform target)
    {
        _monsters.ForEach(m =>
        {
            m.AssignTarget(target);
        });
    }
}