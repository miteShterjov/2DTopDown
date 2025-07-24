using System.Collections;
using TopDown.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TopDown.SceneManagment
{
    public class AreaExit : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;
        [SerializeField][Tooltip("Here goes the name of the Area_Entrance in the scene we want the player to transition.")] private string sceneTransitionName;
        [SerializeField] private float loadSceneDelay = 1f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
                SceneManagment.Instance.SetTransitionName(sceneTransitionName);

                UI_Fade.Instance.FadeToBlack();
                StartCoroutine(LoadSceneRoutine());
            }
        }

        private IEnumerator LoadSceneRoutine()
        {
            yield return new WaitForSeconds(loadSceneDelay);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
