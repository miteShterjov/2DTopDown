using UnityEngine;

namespace TopDown.Weapons
{
    public interface IWeapon
    {
        float AttackCooldown { get; }
        void Attack();
        WeaponInfo GetWeaponInfo();
    }
}
