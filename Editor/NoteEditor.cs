using UnityEngine;
using UnityEditor;

namespace mtti.Funcs.Editor
{
    [CustomEditor(typeof(Note))]
    public class NoteEditor : UnityEditor.Editor
    {
        private Note _target;

        private bool _editing = false;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Separator();

            if (_editing)
            {
                EditorGUI.BeginChangeCheck();

                var newType = (MessageType)EditorGUILayout.EnumPopup(
                    "Type",
                    _target.MessageType
                );

                var newText = GUILayout.TextArea(_target.Text);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_target, "Changed note");
                    _target.MessageType = newType;
                    _target.Text = newText;
                }

                EditorGUILayout.Separator();

                if (GUILayout.Button("Done", GUILayout.Width(100)))
                {
                    _editing = false;
                }
            }
            else
            {
                EditorGUILayout.HelpBox(
                    $"\n{_target.Text}\n",
                    _target.MessageType
                );

                EditorGUILayout.Separator();

                if (GUILayout.Button("Edit", GUILayout.Width(100))) _editing = true;
            }

            EditorGUILayout.Separator();
        }

        private void OnEnable()
        {
            _editing = false;
            _target = (Note)target;
        }
    }
}
