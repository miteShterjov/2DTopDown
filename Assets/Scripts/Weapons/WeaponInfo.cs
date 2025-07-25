using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInfo", menuName = "New Weapon Info")]
public class WeaponInfo : ScriptableObject
{
    public GameObject weaponPrefab;
    public float weaponCooldown;
}


