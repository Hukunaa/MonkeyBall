using UnityEngine;

namespace TutorialSystem.Scripts.Runtime
{
    public class TutorialVisualElementManager : MonoBehaviour
    {
        [SerializeField] private GameObject _childContainer;
        
        [SerializeField] private GameObject _visualElement;

        [SerializeField] private Vector3 _offset;
        
        public void DisplayElementAboveChild(int _childIndex)
        {
            var child = _childContainer.transform.GetChild(_childIndex);
            if (child == null)
            {
                Debug.LogWarning("No child could be found at this index");
                return;
            }

            _visualElement.transform.position = child.transform.position + _offset;
            _visualElement.gameObject.SetActive(true);
        }

        public void HideElement()
        {
            _visualElement.gameObject.SetActive(false);
        }
    }
}