using TopDown.Misc;
using TopDown.Player;
using UnityEngine;

namespace TopDown.SceneManagment
{
    public class AreaEntrance : MonoBehaviour
    {
        [SerializeField] private string transitionName;

        private void Start()
        {
            if (transitionName == SceneManagment.Instance.SceneTransitionName)
            {
                PlayerController.Instance.transform.position = this.transform.position;
                CameraController.Instance.SetPlayerCameraFollow();
                UI_Fade.Instance.FadeToClear();
                SpawnManager.Instance.ReLoadSpawnPoints();
            }
        }
    }
}
