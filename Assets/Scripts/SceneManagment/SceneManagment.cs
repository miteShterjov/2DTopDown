using UnityEngine;

namespace TopDown.SceneManagment
{
    public class SceneManagment : Singleton<SceneManagment>
    {
        public string SceneTransitionName { get; private set; } 

        protected override void Awake()
        {
            base.Awake();
        }

        public void SetTransitionName(string sceneTransitionName)
        {
            this.SceneTransitionName = sceneTransitionName;
        }
        
    }
}
