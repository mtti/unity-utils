using mtti.Funcs.Components;
using UnityEditor;

namespace mtti.Funcs.Editor
{
    [CustomEditor(typeof(DontDestroyOnLoad))]
    public class DontDestroyOnLoadEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox(
                "This component prevents the GameObject it's attached to from being destroyed when a new scene is loaded.",
                MessageType.Info
            );
        }
    }
}
