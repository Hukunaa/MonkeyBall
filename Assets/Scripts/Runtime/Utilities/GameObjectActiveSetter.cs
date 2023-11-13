using System;
using GeneralScriptableObjects.EventChannels;
using UnityEngine;

namespace Utilities
{
    public class GameObjectActiveSetter : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _gameObject;

        [SerializeField] 
        private BoolEventChannel _onChangeGameObjectActiveEventChannel;

        private void Awake()
        {
            _onChangeGameObjectActiveEventChannel.onEventRaised += ChangeVisibility;
        }

        private void OnDestroy()
        {
            _onChangeGameObjectActiveEventChannel.onEventRaised -= ChangeVisibility;
        }

        private void ChangeVisibility(bool _visible)
        {
            _gameObject.SetActive(_visible);
        }
    }
}