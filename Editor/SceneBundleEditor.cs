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
                
                }
            }
        }

        private void ShowMainProperties ()
        {
            DrawBoxedProp ("defaultLoadBehavior");
        }
        

        private void DrawSubScenes ()
        {
            if (DropZone ("Add Scenes", 60, out var droppedAssets))
            {
                Debug.Log (droppedAssets.Length);
            }
            DrawBoxedProp ("scenes");
        }

        private void DrawBoxedProp (string propName)
        {
            var prop = serializedObject.FindProperty (propName);
            
            using (new GUILayout.VerticalScope (GUI.skin.box))
            {
                EditorGUILayout.PropertyField (prop, true);
            }
        }

        private void DrawProp (string propName)
        {
            var prop = serializedObject.FindProperty (propName);
            EditorGUILayout.PropertyField (prop, true);
        }
        
        public static bool DropZone (string title, int height, out Object[] assets)
        {
            GUILayout.Box (title, GUILayout.ExpandWidth(true), GUILayout.MaxHeight (height));
            var dropRect = GUILayoutUtility.GetLastRect ();

            var eventCurrent = Event.current;
            var eventType = Event.current.type;
            var isAccepted = false;
            
            if (dropRect.Contains (eventCurrent.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                
                if (eventType != EventType.DragUpdated || eventType != EventType.DragPerform)
                {
                    assets = null;
                    return false;
                }
                
                if (eventType == EventType.DragPerform) 
                {
                    DragAndDrop.AcceptDrag();
                    isAccepted = true;
                }
                Event.current.Use();
            }

            assets = isAccepted ? DragAndDrop.objectReferences : null;
            return isAccepted ;
        }
    }

}
