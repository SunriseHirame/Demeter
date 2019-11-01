using UnityEngine;

namespace Hirame.Muses
{
    [System.Serializable]
    public class SubScene
    {
        [SerializeField] private string sceneName;
        [SerializeField] private SceneLoadMode loadBehaviour;
    }

}