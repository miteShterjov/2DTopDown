using TopDown.Player;
using UnityEngine;

namespace TopDown.SceneManagment
{
    public class AreaEntrance : MonoBehaviour
    {
        [SerializeField] private string transitionName;

        void Start()
        {
            if (transitionName == SceneManagment.Instance.SceneTransitionName)
            {
                PlayerController.Instance.transform.position = transform.position;
            }
        }
    }
}
