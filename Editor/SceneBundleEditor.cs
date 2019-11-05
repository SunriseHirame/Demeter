using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hirame.Muses.Editor
{
    [CustomEditor (typeof (SceneBundle))]
    public class SceneBundleEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI ()
        {
            serializedObject.Update ();
            
            using (var chanceScope = new EditorGUI.ChangeCheckScope ())
            {
                ShowMainControls ();
                ShowMainProperties ();
                DrawSubScenes ();
                
                if (chanceScope.changed)
                    serializedObject.ApplyModifiedProperties ();
            }
        }

        private void ShowMainControls ()
        {
            using (new GUILayout.VerticalScope (GUI.skin.box))
            {
                if (GUILayout.Button ("Load Scenes"))
                {
                    Debug.Log ("LOAD SCENES");
                    (target as SceneBundle)?.LoadScenes ();
                }
            }
        }

        private void ShowMainProperties ()
        {
            //DrawBoxedProp ("defaultLoadBehavior");
        }
        

        private void DrawSubScenes ()
        {
            var scenesProp = serializedObject.FindProperty ("scenes");
            
            if (DropZone<SceneAsset> ("Drop Scenes To Add", 60, out var droppedAssets))
            {
                AddScenes (scenesProp, droppedAssets);
            }
            
            DrawBoxedProperty (scenesProp);
        }

        private void AddScenes (SerializedProperty property, List<SceneAsset> scenes)
        {
            var hashSet = GetIncludedScenes (property);

            for (var i = scenes.Count - 1; i >= 0; i--)
            {
               if (scenes[i] == null || hashSet.Contains (scenes[i]))
                   scenes.RemoveAt (i);
            }
            
            Undo.RecordObject (target, "Add Scenes");
            (target as SceneBundle)?.AddScenes (scenes);
            
            serializedObject.Update ();
            serializedObject.ApplyModifiedProperties ();
            Debug.Log (serializedObject.FindProperty ("scenes").arraySize);
        }

        private HashSet<SceneAsset> GetIncludedScenes (SerializedProperty property)
        {
            var hashSet = new HashSet<SceneAsset> ();
            var sceneCount = property.arraySize;

            for (var i = 0; i < sceneCount; i++)
            {
                var prop = property.GetArrayElementAtIndex (i);
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset> (
                    AssetDatabase.GUIDToAssetPath (prop.FindPropertyRelative ("sceneGuid").stringValue));
                
                if (sceneAsset != null)
                    hashSet.Add (sceneAsset);
            }
            
            return hashSet;
        }

        private void DrawBoxedProperty (string propName)
        {
            var property = serializedObject.FindProperty (propName);
            DrawBoxedProperty (property);
        }

        private void DrawBoxedProperty (SerializedProperty property)
        {
            using (new GUILayout.VerticalScope (GUI.skin.box))
            {
                EditorGUILayout.PropertyField (property, true);
            }
        }

        private void DrawProp (string propName)
        {
            var prop = serializedObject.FindProperty (propName);
            EditorGUILayout.PropertyField (prop, true);
        }
        
        public static bool DropZone<T> (string title, int height, out List<T> assets) where T : Object
        {
            GUILayout.Box (title, GUILayout.ExpandWidth(true), GUILayout.MaxHeight (height));
            var dropRect = GUILayoutUtility.GetLastRect ();

            var eventCurrent = Event.current;
            var eventType = Event.current.type;
            var isAccepted = false;
            
            if (dropRect.Contains (eventCurrent.mousePosition))
            {            
                DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                
                if (eventType == EventType.DragPerform) 
                {
                    DragAndDrop.AcceptDrag();
                    isAccepted = true;
                }
                else if (eventType != EventType.DragUpdated)
                {
                    assets = null;
                    return false;
                }
                
                Event.current.Use();
            }

            if (isAccepted)
            {
                assets = new List<T> ();
                foreach (var objRef in DragAndDrop.objectReferences)
                {
                    if (objRef is T asset)
                        assets.Add (asset);
                }
            }
            else
            {
                assets = null;
            }
            
            return isAccepted;
        }
    }

}
