using mtti.Funcs.Types;
using System;
using UnityEngine;
using UnityEditor;

namespace mtti.Funcs.Editor
{
	/// <summary>
	/// A property drawer for the <see cref="Uuid"/> struct, allowing its value
	/// to be set manually in the editor or randomized by clicking a button in
	/// the inspector.
	/// </summary>
	[CustomPropertyDrawer(typeof(Uuid))]
	public class UuidDrawer : UnityEditor.PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var name = property.displayName;
			
			var mostSignificantProp = property.FindPropertyRelative("_mostSignificantBits");
			var leastSignificantProp = property.FindPropertyRelative("_leastSignificantBits");

			var oldValue = new Uuid(
				mostSignificantProp.longValue,
				leastSignificantProp.longValue
			);

			Rect contentPosition = EditorGUI.PrefixLabel(position, new GUIContent(name));
			Rect randomizeButtonPos = new Rect(contentPosition);
			
			contentPosition.width -= 65;
			randomizeButtonPos.x += contentPosition.width;
			randomizeButtonPos.width = 65;
			
			EditorGUI.BeginProperty(contentPosition, label, property);
			EditorGUI.BeginChangeCheck();

			var randomize = GUI.Button(randomizeButtonPos, "Random");

			string newRawVal;
			if (randomize)
			{
				newRawVal = Uuid.NewV4().ToHexString();
			}
			else
			{
				newRawVal = EditorGUI.TextField(
					contentPosition,
					"",
					oldValue.ToHexString()
				);
			}
			
			var updated = EditorGUI.EndChangeCheck();
			if (updated)
			{
				if (newRawVal.Length == 24 || newRawVal.Length == 32 ||
				    newRawVal.Length == 36)
				{
					try
					{
						var newUuid = new Uuid(newRawVal);
						mostSignificantProp.longValue = newUuid.MostSignificantBits;
						leastSignificantProp.longValue =
							newUuid.LeastSignificantBits;
					}
					catch (FormatException e)
					{
						Debug.LogException(e);
					}
					catch (ArgumentException e)
					{
						Debug.LogException(e);
					}
				} else if (newRawVal.Length == 0)
				{
					mostSignificantProp.longValue = 0L;
					leastSignificantProp.longValue = 0L;
				}
			}
			EditorGUI.EndProperty();
		}
	}
}
