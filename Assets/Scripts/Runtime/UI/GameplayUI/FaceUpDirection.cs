using UnityEngine;

namespace UI.GameplayUI
{
    public class FaceUpDirection : MonoBehaviour
    {
        [SerializeField] 
        private bool _update;
        
        private void Awake()
        {
            LookUpDirection();
        }

        private void Update()
        {
            if (_update == false) return;
            LookUpDirection();
        }

        private void LookUpDirection()
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }
    }
}