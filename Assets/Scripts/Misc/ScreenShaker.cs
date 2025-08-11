using TopDown.SceneManagment;
using Unity.Cinemachine;

namespace TopDown.Misc
{
    public class ScreenShaker : Singleton<ScreenShaker>
    {
        private CinemachineImpulseSource source;

        protected override void Awake()
        {
            base.Awake();
            source = GetComponent<CinemachineImpulseSource>();
        }

        public void ShakeScreen() => source.GenerateImpulse();
    }
}
