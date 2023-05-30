using UnityEngine;


[CreateAssetMenu(fileName = "BattleEntityData", menuName = "Configs/Data/BattleEntityData", order = 0)]
public class BattleEntityData : ScriptableObject
{
    [SerializeField] private int _health;
    [SerializeField] private int _baseDamage;
    [SerializeField] private WeaponData _weapon;
    [SerializeField] private eBattleEntityType _entityType;

    public int Health => _health;
    public int BaseDamage => _baseDamage;
    public WeaponData Weapon => _weapon;
    public eBattleEntityType EntityType => _entityType;
}