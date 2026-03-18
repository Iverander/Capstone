using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
namespace Capstone
{
namespace Utility
{

    /// <summary>
    /// a extension class made for addressables
    /// </summary>
    public static class Addressable
    {

    
        #region Load Asset
    
        /// <summary>
        /// Load a addressables asset by its addressable name
        /// </summary>
        /// <typeparam name="T">The type to find</typeparam>
        /// <param name="addressable">The addressable name</param>
        /// <param name="getType">if it should return a script from a GameObject rather than return the gameobject</param>
        /// <returns>Addressables asset</returns>
        public static async Task<T> LoadAsset<T>(string addressable, bool getType) where T : MonoBehaviour
        {
            GameObject item = await LoadAsset<GameObject>(addressable);
    
            if (item == null)
            {
                Debug.LogError("[Addressable] item is null");
            }
            if (getType)
            {
                return item.GetComponent<T>();
            }
    
            return null;
        }
    
        /// <summary>
        /// Load a addressables asset by its addressable name
        /// </summary>
        /// <typeparam name="T">The type to find</typeparam>
        /// <param name="addressable">The addressable name</param>
        /// <returns>Addressables asset</returns>
        public static async Task<T> LoadAsset<T>(string addressable) where T : Object
        {
            if (addressable == null || addressable == string.Empty)
            {
                Debug.LogError("[Addressable] empty string");
                return default;
            }
    
            Debug.Log("[Addressable] loading " + addressable);
    
            AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(addressable);
            await handle.Task;
    
    
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return (T)handle.Result;
            }
    
            Debug.LogError("[Addressable] " + handle.Status + " to load " + addressable);
            return default;
        }
        #endregion
    
        /// <summary>
        /// Load addressables assets by their label
        /// </summary>
        /// <typeparam name="T">The type of assets to load</typeparam>
        /// <param name="label">The label</param>
        /// <returns>List of assets</returns>
        public static async Task<List<T>> LoadAssets<T>(string label) where T : Object
        {
            AsyncOperationHandle handle = Addressables.LoadAssetsAsync<T>(label);
            await handle.Task;
    
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return (List<T>)handle.Result;
            }
    
            Debug.LogError(handle.Status);
            return default;
        }
    }
}

}
