using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponsConfig", menuName = "Configs/WeaponsConfig", order = 0)]
public class WeaponsConfig : ScriptableObject
{
    [SerializeField] private List<WeaponData> _weapons;

    public WeaponData GetWeaponDataByType(eWeaponType type)
    {
        return _weapons.First(w => w.EntityType == type);
    }
}
