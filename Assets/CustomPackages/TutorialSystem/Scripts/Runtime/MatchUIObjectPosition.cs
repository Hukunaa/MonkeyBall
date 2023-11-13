using UnityEngine;

namespace TutorialSystem.Scripts.Runtime
{
    public class MatchUIObjectPosition : MonoBehaviour
    {
        private Transform _objectTransform;
        
        private bool _follow;
        
        public void FindUIObjectWithName(string _objectName)
        {
            _objectTransform = GameObject.Find(_objectName).transform;
            _follow = true;
        }
        
        private void Update()
        {
            if (_follow)
            {
                transform.position = _objectTransform.position;
            }
        }
    }
}