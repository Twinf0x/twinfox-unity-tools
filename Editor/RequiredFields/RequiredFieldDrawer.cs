using UnityEditor;
using UnityEngine;

namespace Twinfox.EditorTools
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RequiredFieldAttribute))]
    public class RequiredFieldDrawer : PropertyDrawer
    {
        private Texture2D requiredIcon;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LoadIcon();
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();

            Rect fieldRect = new Rect(position.x, position.y, position.width - 20, position.height);
            EditorGUI.PropertyField(fieldRect, property, label, true);
            if (IsFieldUnassigned(property))
            {
                Rect iconRect = new Rect(position.xMax - 18, fieldRect.y, 16, 16);
                GUI.Label(iconRect, new GUIContent(requiredIcon, "This field is required and must be assigned."), EditorStyles.label);
            }

            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(property.serializedObject.targetObject);

                EditorApplication.RepaintHierarchyWindow();
            }

            EditorGUI.EndProperty();
        }

        private void LoadIcon()
        {
            if (requiredIcon != null)
            {
                return;
            }

            requiredIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.twinfox.tools/Editor/RequiredFields/required_icon.png");
            if (requiredIcon == null)
            {
                requiredIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/twinfox-unity-tools/Editor/RequiredFields/required_icon.png");
            }
        }

        private bool IsFieldUnassigned(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.ObjectReference:
                    return property.objectReferenceValue == null;
                case SerializedPropertyType.ExposedReference:
                    return property.exposedReferenceValue == null;
                case SerializedPropertyType.String:
                    return string.IsNullOrEmpty(property.stringValue);
                case SerializedPropertyType.AnimationCurve:
                    return property.animationCurveValue.length <= 0;
                default:
                    return true;
            }
        }
    }
#endif
}
