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
            if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
            {
                Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
            }

            GameObject weaponToSpawn = transform.GetChild(activeSlotIndexNum).GetComponent<InventorySlot>().WeaponInfo.weaponPrefab;

            GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);
            newWeapon.transform.SetParent(ActiveWeapon.Instance.transform);

            ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
        }
    }
}
