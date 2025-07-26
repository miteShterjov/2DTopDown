using System;
using System.Collections;
using TopDown.SceneManagment;
using UnityEngine;

namespace TopDown.Weapons
{
    public class ActiveWeapon : Singleton<ActiveWeapon>
    {
        public MonoBehaviour CurrentActiveWeapon { get; private set; }

        private InputSystem_Actions inputActions;

        private bool isAttacking;

        protected override void Awake()
        {
            base.Awake();

            inputActions = new InputSystem_Actions();
        }

        private void Start()
        {
            inputActions.Player.Attack.started += _ => Attack();
        }

        private void OnEnable() => inputActions.Enable();

        public void NewWeapon(MonoBehaviour newWeapon) => CurrentActiveWeapon = newWeapon;

        public void WeaponNull() => CurrentActiveWeapon = null;

        private void Attack()
        {
            if (isAttacking && CurrentActiveWeapon as IWeapon == null) return;

            (CurrentActiveWeapon as IWeapon)?.Attack();
            isAttacking = true;

            StartCoroutine(WaitForAttackCooldownRoutine());
        }

        private IEnumerator WaitForAttackCooldownRoutine()
        {
            if (CurrentActiveWeapon as IWeapon == null) yield break;

            yield return new WaitForSeconds((CurrentActiveWeapon as IWeapon).AttackCooldown);
            isAttacking = false;
        }
    }
}
