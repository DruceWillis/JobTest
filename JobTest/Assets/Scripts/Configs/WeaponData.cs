using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Configs/Data/WeaponData", order = 0)]
public class WeaponData : ScriptableObject
{
    [SerializeField] private int _damage;
    [SerializeField] private eWeaponType _weaponType;
    [SerializeField] private List<eBattleEntityType> _damageableEntityTypes; 

    public int Damage => _damage;
    public eWeaponType EntityType => _weaponType;
    public List<eBattleEntityType> DamageableEntityTypes => _damageableEntityTypes;

    public bool CanDamageEntityByType(eBattleEntityType entityType)
    {
        return _damageableEntityTypes.Any(det => det == entityType);
    }
}