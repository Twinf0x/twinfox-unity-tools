using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Twinfox.EditorTools
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public static class RequiredFieldHierarchyDrawer
    {
        private static Texture2D requiredIcon;
        private static readonly Dictionary<Type, FieldInfo[]> cachedFieldInfo = new Dictionary<Type, FieldInfo[]>();

        static RequiredFieldHierarchyDrawer()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
            LoadIcon();
        }

        private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is not GameObject gameObject)
            {
                return;
            }

            foreach(var component in gameObject.GetComponents<Component>())
            {
                if (component == null)
                {
                    continue;
                }

                var fields = GetRequiredFields(component.GetType());
                if (fields == null)
                {
                    continue;
                }

                if (fields.Any(field => IsFieldUnassigned(field.GetValue(component))))
                {
                    Rect iconRect = new Rect(selectionRect.xMax - 18, selectionRect.y, 16, 16);
                    GUI.Label(iconRect, new GUIContent(requiredIcon, "This object has required fields that are not assigned."), EditorStyles.label);
                    break;
                }
            }

            foreach (var component in gameObject.GetComponentsInChildren<Component>())
            {
                if (component == null)
                {
                    continue;
                }

                var fields = GetRequiredFields(component.GetType());
                if (fields == null)
                {
                    continue;
                }

                if (fields.Any(field => IsFieldUnassigned(field.GetValue(component))))
                {
                    Rect iconRect = new Rect(selectionRect.xMax - 18, selectionRect.y, 16, 16);
                    GUI.Label(iconRect, new GUIContent(requiredIcon, "A child of this object has required fields that are not assigned."), EditorStyles.label);
                    break;
                }
            }
        }

        private static void LoadIcon()
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

        private static FieldInfo[] GetRequiredFields(Type componentType)
        {
            if (!cachedFieldInfo.TryGetValue(componentType, out FieldInfo[] fields))
            {
                fields = componentType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                List<FieldInfo> requiredFields = new List<FieldInfo>();

                foreach(var field in fields)
                {
                    bool isSerialized = field.IsPublic || field.IsDefined(typeof(SerializeField), false);
                    bool isRequired = field.IsDefined(typeof(RequiredFieldAttribute), false);

                    if (isSerialized && isRequired)
                    {
                        requiredFields.Add(field);
                    }
                }

                fields = requiredFields.ToArray();
                cachedFieldInfo[componentType] = fields;
            }

            return fields;
        }

        private static bool IsFieldUnassigned(object fieldValue)
        {
            if (fieldValue == null)
            {
                return true;
            }

            if (fieldValue is string str)
            {
                return string.IsNullOrEmpty(str);
            }

            if (fieldValue is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    if (item == null)
                    {
                        return true;
                    }
                }
            }

            // Add more checks for other types as needed
            return false;
        }
    }
#endif
}


