using UnityEngine;

namespace UI.GameplayUI
{
    public class CopyTransformRotation : MonoBehaviour
    {
        [SerializeField] private Transform _transformToCopy;

        private void Update()
        {
            transform.rotation = _transformToCopy.rotation;
        }
    }
}
