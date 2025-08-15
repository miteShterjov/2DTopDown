using UnityEngine;

namespace TopDown.PickUps
{
    public class PickUpSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] pickUps;

        public void DropLoot()
        {
            int randomIndex = Random.Range(0, pickUps.Length);

            Instantiate(pickUps[randomIndex], transform.position, Quaternion.identity);
        }
    }
}
