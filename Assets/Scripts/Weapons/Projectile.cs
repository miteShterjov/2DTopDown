using System;
using TopDown.Enemy;
using TopDown.Misc;
using UnityEngine;

namespace TopDown.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifetime = 5f;
        [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] private float destroyDelay = 0.8f;

        private WeaponInfo weaponInfo;
        private Vector3 startPosition;

        private void Start()
        {
            Destroy(gameObject, lifetime);
            startPosition = transform.position;
        }

        private void Update()
        {
            MovePojectile();
            DetectFiredDistance();
        }

        public void UpdateWeaponInfo(WeaponInfo info)
        {
            this.weaponInfo = info;
        }

        private void MovePojectile()
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();

            if (enemy != null) RunDestroySequence();
            if (collision.gameObject.GetComponent<Indestructuble>() != null && !collision.isTrigger) RunDestroySequence();
        }

        private void RunDestroySequence()
        {
            GameObject hitEffectVFX = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            hitEffectVFX.GetComponent<DestroyThis>().DestroySelf();
            Destroy(gameObject);
        }

        private void DetectFiredDistance()
        {
            float distanceTraveled = Vector3.Distance(transform.position, startPosition);
            if (distanceTraveled > weaponInfo.weaponRange) Destroy(gameObject);
        }
    }
}
