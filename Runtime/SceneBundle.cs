using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.SceneManagement;

#endif

namespace Hirame.Muses
{
    [CreateAssetMenu (menuName = "Hirame/Demeter/Scene Bundle")]
    public class SceneBundle : ScriptableObject
    {
        //[SerializeField] private SceneLoadMode defaultLoadBehavior;

        [SerializeField] private SubScene[] scenes;


        internal void LoadScenes ()
        {
            
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                var openMode = OpenSceneMode.Single;

                foreach (var scene in scenes)
                {
                    EditorSceneManager.OpenScene (AssetDatabase.GUIDToAssetPath (scene.SceneGuid), openMode);
                    openMode = OpenSceneMode.Additive;
                }

                return;
            }
#endif

            var loadMode = LoadSceneMode.Single;

            foreach (var scene in scenes)
            {
                SceneManager.LoadScene (scene.SceneName, loadMode);
                loadMode = LoadSceneMode.Additive;
            }
        }

        internal void UnloadScenes ()
        {
        }

#if UNITY_EDITOR
        public void AddScene (SceneAsset sceneAsset)
        {
            if (scenes == null)
            {
                scenes = new[] {new SubScene (sceneAsset)};
                return;
            }
            
            Array.Resize (ref scenes, scenes.Length + 1);
            scenes[scenes.Length - 1] = new SubScene (sceneAsset);
            Array.Sort (scenes);
        }

        public void AddScenes (List<SceneAsset> sceneAssets)
        {
            var count = scenes?.Length ?? 0;
            if (scenes == null)
            {
                scenes = new SubScene[sceneAssets.Count];
            }
            else
            {
                Array.Resize (ref scenes, count + sceneAssets.Count);
            }
            
            var j = 0;
            for (var i = count; i < scenes.Length; i++)
            {
                scenes[i] = new SubScene (sceneAssets[j++]);
            }

            Array.Sort (scenes);
        }
#endif
    }
}