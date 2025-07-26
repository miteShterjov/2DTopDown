using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDown.Misc
{
    public class MouseFollow : MonoBehaviour
    {
        void Update()
        {
            FaceMouse();
        }

        private void FaceMouse()
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector2 direction = (transform.position - mousePosition).normalized;
            transform.right = direction;
        }
    }
}
