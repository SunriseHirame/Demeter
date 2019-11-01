using UnityEngine;

namespace Hirame.Muses
{
    [CreateAssetMenu (menuName = "Hirame/Demeter/Scene Bundle")]
    public class SceneBundle : ScriptableObject
    {
        [SerializeField] private SceneLoadMode defaultLoadBehavior;
        
        [SerializeField] private SubScene[] scenes;

        internal void LoadScenes ()
        {
            
        }

        internal void UnloadScenes ()
        {
            
        }
    }

}
