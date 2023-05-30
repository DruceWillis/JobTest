using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "BattleEntitiesConfig", menuName = "Configs/BattleEntitiesConfig", order = 0)]
public class BattleEntitiesConfig : ScriptableObject
{
    [SerializeField] private List<BattleEntity> _battleEntities;

    public BattleEntity GetBattleEntityByType(eBattleEntityType type)
    {
        return _battleEntities.First(be => be.Data.EntityType == type);
    }
}

[Serializable]
public class BattleEntity
{
    public BaseBattleEntity Prefab;
    public BattleEntityData Data;
}