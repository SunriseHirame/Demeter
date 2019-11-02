using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Hirame.Pantheon.Editor;

namespace Hirame.Muses.Editor
{
    [CustomPropertyDrawer (typeof (SubScene))]
    public class SubSceneDrawer : PropertyDrawer
    {
        private static Dictionary<SerializedProperty, SceneAsset> sceneAssetLib = new Dictionary<SerializedProperty, SceneAsset> ();
        private SceneAsset sceneAsset;
        
        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
        {
            var lineRect = position.AsLineHigh ();
            var sceneNameProp = property.FindPropertyRelative ("sceneName");

            if (!sceneAssetLib.TryGetValue (property, out sceneAsset))
                FindSceneAsset (property);

            GUI.Box (position, GUIContent.none);

            DrawSubSceneHeader (property, sceneNameProp, ref lineRect);

            if (!property.isExpanded)
                return;

            DrawSceneAssetSelector (property, ref lineRect);
            var guidProp = property.FindPropertyRelative ("sceneGuid");
            EditorGUI.LabelField (lineRect, "Asset Guid", guidProp.stringValue);
            //DrawSceneLoadMode (property, ref lineRect);
        }

        private void DrawSubSceneHeader (SerializedProperty property, SerializedProperty sceneNameProp,
            ref Rect lineRect)
        {
            var sceneName = sceneNameProp?.stringValue ?? string.Empty;
            if (string.IsNullOrEmpty (sceneName))
                sceneName = "(None)";

            property.isExpanded = EditorGUI.Foldout (
                lineRect, property.isExpanded, sceneName, true);

            lineRect.NextLine ();
        }

        private void DrawSceneAssetSelector (SerializedProperty property, ref Rect lineRect)
        {
            var changedSceneAsset = EditorGUI.ObjectField (
                lineRect, "Scene Asset", sceneAsset, typeof (SceneAsset), false);

            lineRect.NextLine ();

            if (changedSceneAsset != false && !changedSceneAsset.Equals (sceneAsset))
                UpdateSceneAsset (property, changedSceneAsset as SceneAsset);
        }

        private void UpdateSceneAsset (SerializedProperty property, SceneAsset newSceneAsset)
        {
            property.FindPropertyRelative ("sceneName").stringValue = newSceneAsset.name;
            sceneAssetLib[property] = newSceneAsset;
            
            var guid = AssetDatabase.AssetPathToGUID (AssetDatabase.GetAssetPath (newSceneAsset));
            property.FindPropertyRelative ("sceneGuid").stringValue = guid;

            property.serializedObject.ApplyModifiedProperties ();
        }

        private void FindSceneAsset (SerializedProperty property)
        {
            var sceneGuid = property.FindPropertyRelative ("sceneGuid").stringValue;
            if (sceneGuid == null)
                return;
            
            sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset> (AssetDatabase.GUIDToAssetPath (sceneGuid));
            sceneAssetLib.Add (property, sceneAsset);
        }

        private void DrawSceneLoadMode (SerializedProperty property, ref Rect lineRect)
        {
            var loadBehaviour = property.FindPropertyRelative ("loadBehaviour");
            EditorGUI.PropertyField (lineRect, loadBehaviour);
            lineRect.NextLine ();
        }

        public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
        {
            var lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            return !property.isExpanded ? lineHeight : lineHeight * 3;
        }

        public override bool CanCacheInspectorGUI (SerializedProperty property)
        {
            return false;
        }
    }
}