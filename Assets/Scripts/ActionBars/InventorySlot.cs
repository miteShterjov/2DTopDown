using UnityEngine;

namespace TopDown.ActionBars
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private WeaponInfo weaponInfo;

        public WeaponInfo WeaponInfo
        {
            get => weaponInfo;
            set => weaponInfo = value;
        }          

    }
}
