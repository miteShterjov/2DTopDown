using TopDown.Player;
using Unity.Cinemachine;
using UnityEngine;

namespace TopDown.SceneManagment
{
    public class CameraController : Singleton<CameraController>
    {
        private CinemachineCamera cinemachineCamera;

        public void SetPlayerCameraFollow()
        {
            cinemachineCamera = FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None)[0];
            if (cinemachineCamera != null)
            {
                cinemachineCamera.Follow = PlayerController.Instance.transform;
                cinemachineCamera.LookAt = PlayerController.Instance.transform;
            }
            else
            {
                Debug.LogError("Cinemachine Camera not found in the scene.");
            }
        }
    }
}
