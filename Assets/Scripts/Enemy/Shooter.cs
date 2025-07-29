using TopDown.Player;
using UnityEngine;

namespace TopDown.Enemy
{
    public class Shooter : MonoBehaviour, IEnemy
    {
        [SerializeField] private GameObject bulletPrefab;
        public void Attack()
        {
            Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;

            GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            newBullet.transform.right = targetDirection;
        }
    }
}
