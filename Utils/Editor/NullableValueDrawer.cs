/*
Copyright 2022 Matti Hiltunen (https://www.mattihiltunen.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;
using UnityEditor;

namespace mtti.Funcs.Editor
{
    [CustomPropertyDrawer(typeof(NullableValue<>))]
    public class NullableValueDrawer : PropertyDrawer
    {
        public override void OnGUI(
            Rect position,
            SerializedProperty property,
            GUIContent label
        )
        {
            var contentRect = EditorGUI.PrefixLabel(
                position,
                label
            );

            var hasValueRect = new Rect(
                contentRect.x,
                contentRect.y,
                10,
                contentRect.height
            );
            var valueRect = new Rect(
                contentRect.x + 15,
                position.y,
                position.width - 15, position.height
            );

            var hasValueProperty = property.FindPropertyRelative("_hasValue");
            EditorGUI.PropertyField(
                hasValueRect,
                hasValueProperty,
                GUIContent.none
            );

            if (hasValueProperty.boolValue)
            {
                var valueProperty = property.FindPropertyRelative("_value");
                EditorGUI.PropertyField(
                    valueRect,
                    valueProperty,
                    GUIContent.none
                );
            }
        }
    }
}
