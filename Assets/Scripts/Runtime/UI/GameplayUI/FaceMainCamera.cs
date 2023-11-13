using UnityEngine;

namespace UI.GameplayUI
{
    public class FaceMainCamera : MonoBehaviour
    {
        private Camera _cam;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(-_cam.transform.forward);
        }
    }
}