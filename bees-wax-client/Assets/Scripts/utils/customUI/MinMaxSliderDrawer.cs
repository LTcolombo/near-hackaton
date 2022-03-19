using System;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

#endif

namespace utils.customUI
{
    [Serializable]
    public class MinMaxRange
    {
        public float RangeEnd;
        public float RangeStart;
    }

    public class MinMaxRangeAttribute : PropertyAttribute
    {
        public readonly float MaxLimit;

        public readonly float MinLimit;

        public MinMaxRangeAttribute(float min, float max)
        {
            MinLimit = min;
            MaxLimit = max;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
    internal class MinMaxSliderDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + 16;
        }

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Now draw the property as a Slider or an IntSlider based on whether it’s a float or integer.
            if (property.type != "MinMaxRange")
            {
                Debug.LogWarning("Use only with MinMaxRange type");
            }
            else
            {
                var range = attribute as MinMaxRangeAttribute;
                var minValue = property.FindPropertyRelative("RangeStart");
                var maxValue = property.FindPropertyRelative("RangeEnd");
                var newMin = minValue.floatValue;
                var newMax = maxValue.floatValue;

                var xDivision = position.width * 0.33f;
                var yDivision = position.height * 0.5f;
                EditorGUI.LabelField(new Rect(position.x, position.y, xDivision, yDivision), label);

                EditorGUI.LabelField(new Rect(position.x, position.y + yDivision, position.width, yDivision),
                    range.MinLimit.ToString("0.##"));
                EditorGUI.LabelField(
                    new Rect(position.x + position.width - 20f, position.y + yDivision, position.width, yDivision),
                    range.MaxLimit.ToString("0.##"));
                EditorGUI.MinMaxSlider(
                    new Rect(position.x + 24f, position.y + yDivision, position.width + -48f, yDivision), ref newMin,
                    ref newMax, range.MinLimit, range.MaxLimit);

                EditorGUI.LabelField(new Rect(position.x + xDivision, position.y, xDivision, yDivision), "From: ");
                newMin = Mathf.Clamp(
                    EditorGUI.FloatField(
                        new Rect(position.x + xDivision + 34f, position.y, xDivision + -30f, yDivision), newMin),
                    range.MinLimit, newMax);
                EditorGUI.LabelField(new Rect(position.x + xDivision * 2f + 4f, position.y, xDivision, yDivision),
                    "To: ");
                newMax = Mathf.Clamp(
                    EditorGUI.FloatField(
                        new Rect(position.x + xDivision * 2f + 24, position.y, xDivision + -24f, yDivision), newMax),
                    newMin, range.MaxLimit);

                minValue.floatValue = newMin;
                maxValue.floatValue = newMax;

                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
#endif
}