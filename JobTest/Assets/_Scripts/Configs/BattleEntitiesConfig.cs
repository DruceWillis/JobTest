using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "BattleEntitiesConfig", menuName = "Configs/BattleEntitiesConfig", order = 0)]
public class BattleEntitiesConfig : ScriptableObject
{
    [SerializeField] private List<BattleEntityData> _battleEntities;

    public BattleEntityData GetBattleEntityDataByType(eBattleEntityType type)
    {
        return _battleEntities.First(be => be.EntityType == type);
    }
}
