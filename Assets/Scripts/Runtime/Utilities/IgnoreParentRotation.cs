using UnityEngine;

namespace Utilities
{
    public class IgnoreParentRotation : MonoBehaviour
    {
        private Vector3 _offset;
        [SerializeField]
        private Vector3 _selectedOffset = new Vector3(0, -0.5f, 0);

        private void Update()
        {
            var parent = transform.parent;
            var pos = parent.position + _selectedOffset;
            var rotation = parent.rotation;
            var rot = Quaternion.Euler(rotation.x * -1.0f, rotation.x * -1.0f, rotation.x * -1.0f);
            transform.SetPositionAndRotation(pos, rot);
        }
    }
}
