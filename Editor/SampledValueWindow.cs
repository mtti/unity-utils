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

        private void DrawSampledValue(string key, SampledValue value)
        {
            GUILayout.Label(key);
            DrawField("Value", value.Value.ToString());
            DrawField(
                "Updated",
                new DateTime(value.Time)
                    .ToLocalTime()
                    .ToString("HH:mm:ss.fffffff")
            );
            DrawField(
                "Frame",
                value.Frame.ToString()
            );
            EditorGUILayout.Space();
        }

        private void DrawField(string label, string value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(
                label,
                GUILayout.Width(EditorGUIUtility.labelWidth - 4)
            );
            EditorGUILayout.SelectableLabel(
                value,
                EditorStyles.textField,
                GUILayout.Height(EditorGUIUtility.singleLineHeight)
            );
            EditorGUILayout.EndHorizontal();
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

            EditorGUILayout.HelpBox(
                "Call mtti.Funcs.DebugUtils.Sample(...) to output values here.",
                MessageType.Info
            );
            EditorGUILayout.Space();

            for (int i = 0, count = _keys.Count; i < count; i++)
            {
                DrawSampledValue(
                    _keys[i],
                    DebugUtils.SampledValues.GetLast(_keys[i])
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
