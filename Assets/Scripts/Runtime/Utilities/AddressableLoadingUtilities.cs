using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Utilities
{
    public static class AddressableLoadingUtilities
    {
        public const string SkinsAddressablePath = "SkinsSO/";
        public static async Task<Tuple<T[], AsyncOperationHandle<IList<T>>>> LoadAddressableByKeysAsync<T>(IEnumerable<string> _keys)
        {
            List<T> skins = new List<T>();
            var loadHandle =
                Addressables.LoadAssetsAsync<T>(_keys,
                    _obj =>
                    {
                        skins.Add(_obj);
                    }, Addressables.MergeMode.Union);

            await loadHandle.Task;
            return new Tuple<T[], AsyncOperationHandle<IList<T>>>(skins.ToArray(), loadHandle);
        }

        public static IEnumerator LoadAddressableByKeysCoroutine<T>(IEnumerable<string> _keys, UnityAction<AsyncOperationHandle<IList<T>>, List<T>> callback)
        {
            List<T> skins = new List<T>();
            var loadHandle =
                Addressables.LoadAssetsAsync<T>(_keys,
                    _obj =>
                    {
                        skins.Add(_obj);
                    }, Addressables.MergeMode.Union);

            yield return loadHandle;
            callback.Invoke(loadHandle, skins);
        }
    }
}