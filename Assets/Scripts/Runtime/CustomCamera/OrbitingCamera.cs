using UnityEngine;

namespace CustomCamera
{
    public class OrbitingCamera : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private void Update()
        {
            RotateAroundTarget();
        }

        void RotateAroundTarget()
        {
            transform.LookAt(_target);
            transform.RotateAround(_target.transform.position, Vector3.up, 20 * Time.deltaTime);
        }
    }
}