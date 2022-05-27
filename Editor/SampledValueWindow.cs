using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace mtti.Funcs.Editor
{
    /// <summary>
    /// An editor window for showing sampled values from
    /// <see cref="mtti.Funcs.DebugUtils" />.
    /// </summary>
    public class SampledValueWindow : EditorWindow
    {
        [MenuItem("Window/Analysis/Sampled Values")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(
                typeof(SampledValueWindow),
                false,
                "Sampled Values"
            );
        }

        private List<string> _keys = new List<string>();

        private long _updatedAt = long.MinValue;

        private void DrawSampledValue(string key, string value)
        {
            GUILayout.Label(key);
            EditorGUILayout.SelectableLabel(
                value,
                EditorStyles.textField,
                GUILayout.Height(EditorGUIUtility.singleLineHeight)
            );
            EditorGUILayout.Space();
        }

        private void OnGUI()
        {
            _keys.Clear();
            foreach (var key in DebugUtils.SampledValues.Keys)
            {
                _keys.Add(key);
            }
            _keys.Sort();

            GUILayout.Label(
                $"Total {_keys.Count}",
                EditorStyles.boldLabel
            );
            EditorGUILayout.Space();

            for (int i = 0, count = _keys.Count; i < count; i++)
            {
                DrawSampledValue(
                    _keys[i],
                    DebugUtils.SampledValues.GetLast(_keys[i]).Value.ToString()
                );
            }

            _updatedAt = DateTime.UtcNow.Ticks;
        }

        private void Update()
        {
            if (DebugUtils.SampleTime > _updatedAt) Repaint();
        }
    }
}
