using UnityEngine;

namespace CustomUtilities
{
    public class GuidGenerator : MonoBehaviour
    {
        [SerializeField] private string _guid;

        private void Reset()
        {
            GenerateGuid();
        }

        public void GenerateGuid()
        {
            _guid = System.Guid.NewGuid().ToString();
        }

        public void CopyToClipboard()
        {
            _guid.CopyToClipboard();
        }
    }
}