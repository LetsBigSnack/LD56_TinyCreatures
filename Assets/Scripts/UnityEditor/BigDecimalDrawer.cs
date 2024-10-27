namespace UnityEditor
{
    using UnityEngine;
    using UnityEditor;
    using System.Numerics;
    using Data;
    using System.Reflection;

    [CustomPropertyDrawer(typeof(BigDecimal))]
    public class BigDecimalDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Access the serialized string properties for mantissa and exponent
            SerializedProperty mantissaStringProperty = property.FindPropertyRelative("mantissaString");
            SerializedProperty exponentStringProperty = property.FindPropertyRelative("exponentString");

            // Begin drawing the property with label
            EditorGUI.BeginProperty(position, label, property);

            // Draw label and calculate layout for mantissa and exponent fields
            position = EditorGUI.PrefixLabel(position, label);
            float halfWidth = position.width / 2;
            Rect mantissaRect = new Rect(position.x, position.y, halfWidth, position.height);
            Rect exponentRect = new Rect(position.x + halfWidth, position.y, halfWidth, position.height);

            // Get current values from the serialized string properties
            string mantissaInput = mantissaStringProperty.stringValue;
            string exponentInput = exponentStringProperty.stringValue;

            // Draw text fields for mantissa and exponent, allowing editing in the Inspector
            mantissaInput = EditorGUI.TextField(mantissaRect, mantissaInput);
            exponentInput = EditorGUI.TextField(exponentRect, exponentInput);

            // Update the serialized string properties with the current input values
            mantissaStringProperty.stringValue = mantissaInput;
            exponentStringProperty.stringValue = exponentInput;

            // Attempt to parse the string values into BigInteger for validation and internal updating
            if (BigInteger.TryParse(mantissaInput, out BigInteger newMantissa) &&
                BigInteger.TryParse(exponentInput, out BigInteger newExponent))
            {
                // Access the actual BigDecimal instance within the target object
                object targetObject = property.serializedObject.targetObject;
                var targetField = targetObject.GetType().GetField(property.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (targetField != null)
                {
                    // Retrieve the actual BigDecimal instance and update its values directly
                    BigDecimal bigDecimalValue = (BigDecimal)targetField.GetValue(targetObject);
                    bigDecimalValue = new BigDecimal(newMantissa, newExponent); // Update mantissa and exponent

                    // Set the modified BigDecimal back to the field and mark the object dirty
                    targetField.SetValue(targetObject, bigDecimalValue);
                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                }
            }
            else
            {
                Debug.LogWarning("Invalid input for BigDecimal mantissa or exponent.");
            }

            EditorGUI.EndProperty();
        }
    }
}
