using TopDown.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace TopDown.ActionBars
{
    public class ActiveInventory : MonoBehaviour
    {
        private int activeSlotIndexNum = 0;

        private InputSystem_Actions inputActions;

        void Awake()
        {
            inputActions = new InputSystem_Actions();
        }

        void Start()
        {
            inputActions.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
            ToggleActiveHighlight(0); // Start with the first slot aka sword as weapon highlighted by deafault
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void ToggleActiveSlot(int numValue)
        {
            ToggleActiveHighlight(numValue);
        }

        private void ToggleActiveHighlight(int numValue)
        {
            activeSlotIndexNum = numValue;

            foreach (Transform actionBarSlot in transform)
            {
                actionBarSlot.GetChild(0).gameObject.SetActive(false);
            }

            transform.GetChild(activeSlotIndexNum).GetChild(0).gameObject.SetActive(true);

            ChangeActiveWeapon();
        }

        private void ChangeActiveWeapon()
        {
            if (ActiveWeapon.Instance.CurrentActiveWeapon != null) Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);

            Transform childTransform = transform.GetChild(activeSlotIndexNum);
            InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
            WeaponInfo weaponInfo = inventorySlot?.WeaponInfo;
            GameObject weaponToSpawn = weaponInfo?.weaponPrefab;

            if (weaponInfo == null)
            {
                ActiveWeapon.Instance.WeaponNull();
                return;
            }


            GameObject newWeapon = Instantiate(
                            weaponToSpawn,
                            ActiveWeapon.Instance.transform.position,
                            Quaternion.identity
                            );

            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
            newWeapon.transform.SetParent(ActiveWeapon.Instance.transform);

            ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
        }
    }
}
