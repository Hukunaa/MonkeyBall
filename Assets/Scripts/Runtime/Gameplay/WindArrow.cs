using UnityEngine;

namespace Gameplay
{
    public class WindArrow : MonoBehaviour
    {
        [SerializeField] 
        private WindController _windController;

        private void Awake()
        {
            _windController._onWindChanged += UpdateArrowDirection;
        }
        
        private void UpdateArrowDirection()
        {
            transform.rotation = Quaternion.LookRotation(_windController.WindDirection, Vector3.up);
        }
    }
}