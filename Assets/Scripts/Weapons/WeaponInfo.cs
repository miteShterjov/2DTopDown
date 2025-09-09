using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInfo", menuName = "New Weapon Info")]
public class WeaponInfo : ScriptableObject
{
    public int weaponDamage;
    public int weaponRange;
    public GameObject weaponPrefab;
    public float weaponCooldown;
    public int attackCost;
}


