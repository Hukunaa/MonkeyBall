using UI.GameplayUI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Gameplay.Character
{
    public class PlayerInstantiationManager : MonoBehaviour
    {
        [SerializeField] 
        private AssetReferenceT<GameObject> _pointsEarnedAssetRef;
    
        private AsyncOperationHandle<GameObject> _pointsEarnedLoadHandle;
    
        private void Awake()
        {
            _pointsEarnedLoadHandle = _pointsEarnedAssetRef.LoadAssetAsync<GameObject>();
            _pointsEarnedLoadHandle.Completed += PointsEarnedLoadHandleOnCompleted;

      
        }
    
        private void OnDestroy()
        {
            Addressables.Release(_pointsEarnedLoadHandle);
        }

        private void PointsEarnedLoadHandleOnCompleted(AsyncOperationHandle<GameObject> obj)
        {
            var instance = Instantiate(obj.Result);
            instance.GetComponent<PointsEarnUIFeedback>().SetPlayerInfo(transform.GetComponent<Player>());
        }
    }
}
