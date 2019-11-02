using System;
using UnityEditor;
using UnityEngine;

namespace Hirame.Muses
{
    [Serializable]
    public class SubScene : IComparable<SubScene>
    {
        [SerializeField] private string sceneName;
        [SerializeField] private SceneLoadMode loadBehaviour;

        [SerializeField] private string sceneGuid;

#if UNITY_EDITOR
        public SubScene (SceneAsset sceneAsset)
        {
            sceneName = sceneAsset.name;
            sceneGuid = AssetDatabase.AssetPathToGUID (AssetDatabase.GetAssetPath (sceneAsset));
        }
#endif

        public string SceneName => sceneName;
        public string SceneGuid => sceneGuid;
        
        public int CompareTo (SubScene other)
        {
            return string.Compare (sceneName, other.sceneName);
        }
    }
}