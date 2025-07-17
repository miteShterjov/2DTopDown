using TopDown.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TopDown.SceneManagment
{
    public class AreaExit : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;
        [SerializeField] private string sceneTransitionName;
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerController>())
            {
                SceneManager.LoadScene(sceneToLoad); 
                SceneManagment.Instance.SetTransitionName(sceneTransitionName);
            }
        }
    }
}
