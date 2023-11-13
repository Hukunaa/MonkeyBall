using ScriptableObjects.DataContainer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Utilities
{
    public class PriceTableInitializer : MonoBehaviour
    {
        [SerializeField] 
        private AssetReferenceT<PriceTableSO>  _priceTableAssetRef;

        private AsyncOperationHandle<PriceTableSO> _priceTableLoadHandle;

        private void Awake()
        {
            _priceTableLoadHandle = _priceTableAssetRef.LoadAssetAsync<PriceTableSO>();
            _priceTableLoadHandle.Completed += _handle =>
            {
                _handle.Result.LoadPriceTable();
            };
        }

        private void OnDestroy()
        {
            Addressables.Release(_priceTableLoadHandle);
        }
    }
}
