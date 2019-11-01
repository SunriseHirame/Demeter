using UnityEditor;
using UnityEngine;
using Hirame.Pantheon.Editor;

namespace Hirame.Muses.Editor
{
    [CustomPropertyDrawer (typeof (SubScene))]
    public class SubSceneDrawer : PropertyDrawer
    {
        private SceneAsset sceneAsset;
        
        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
        {
            var lineRect = position.AsLineHigh ();

            var sceneNameProp = property.FindPropertyRelative ("sceneName");

            GUI.Box (position, GUIContent.none);

            DrawSubSceneHeader (property, sceneNameProp, ref lineRect);

            if (!property.isExpanded)
                return;
            
            DrawSceneAssetSelector (property, ref lineRect);
            DrawSceneLoadMode (property, ref lineRect);
        }

        private void DrawSubSceneHeader (SerializedProperty property, SerializedProperty sceneNameProp, ref Rect lineRect)
        {
            var sceneName = sceneNameProp.stringValue;
            if (string.IsNullOrEmpty (sceneName))
                sceneName = "(None)";

            property.isExpanded = EditorGUI.Foldout (
                lineRect, property.isExpanded, sceneName, true);

            lineRect.NextLine ();
        }

        private void DrawSceneAssetSelector (SerializedProperty property, ref Rect lineRect)
        {
            var changedSceneAsset = EditorGUI.ObjectField (
                lineRect, "Scene Asset", sceneAsset, typeof(SceneAsset), false);
            
            lineRect.NextLine ();
            
            if (changedSceneAsset != false && changedSceneAsset.Equals (sceneAsset))
            {
                
            }
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
    }

}
