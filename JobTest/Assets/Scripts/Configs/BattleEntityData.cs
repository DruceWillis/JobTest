using UnityEngine;


[CreateAssetMenu(fileName = "BattleEntityData", menuName = "Configs/BattleEntityData", order = 0)]
public class BattleEntityData : ScriptableObject
{
    [SerializeField] private int _health;
    [SerializeField] private int _damage;
    [SerializeField] private eBattleEntityType _entityType;

    public int Health => _health;
    public int Damage => _damage;
    public eBattleEntityType EntityType => _entityType;
}