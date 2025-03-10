using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using System;
using System.Linq;

namespace Twinfox.EditorTools
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public static class CustomHierarchyDisplay
    {
        private static bool _hierarchyHasFocus;
        private static EditorWindow _hierarchyWindow;

        static CustomHierarchyDisplay()
        {
            EditorApplication.update += OnEditorUpdate;
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemRendering;
        }

        private static void OnEditorUpdate()
        {
            if (_hierarchyWindow == null)
            {
                _hierarchyWindow = EditorWindow.GetWindow(Type.GetType("UnityEditor.SceneHierarchyWindow,UnityEditor"));
            }

            _hierarchyHasFocus = EditorWindow.focusedWindow != null && EditorWindow.focusedWindow == _hierarchyWindow;
        }

        private static void OnHierarchyWindowItemRendering(int instanceID, Rect selectionRect)
        {
            if (!CustomHierarchyDisplaySettingsProvider.UseCustomHierarchyIcons)
            {
                return;
            }

            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (obj == null)
            {
                return;
            }

            if (CustomHierarchyDisplaySettingsProvider.UseSeparateActivationToggle)
            {
                DrawActivationToggle(selectionRect, obj);
            }
            else if (CustomHierarchyDisplaySettingsProvider.UseIconAsActivationToggle)
            {
                UpdateIconAsActivationToggle(selectionRect, obj);
            }

            if (!CustomHierarchyDisplaySettingsProvider.UseCustomIconsForPrefabs
                && PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj) != null)
            {
                // The object is a prefab and the icon should not be replaced
                return;
            }

            Component[] components = obj.GetComponents<Component>();
            if (components == null || components.Length <= 0)
            {
                return;
            }

            // TODO: Maybe create a priority list and fetch the highest priority component?
            Component displayComponent = components.Length > 1 ? components[1] : components[0];
            Type displayType = displayComponent.GetType();

            GUIContent icon = EditorGUIUtility.ObjectContent(displayComponent, displayType);
            icon.text = null;
            icon.tooltip = displayType.Name;

            if (icon.image == null)
            {
                return;
            }

            // Draw over the default icon
            bool isSelected = Selection.instanceIDs.Contains(instanceID);
            bool isHovered = selectionRect.Contains(Event.current.mousePosition);
            Color color = UnityEditorBackgroundColor.Get(isSelected, isHovered, _hierarchyHasFocus);
            Rect backgroundRect = selectionRect;
            backgroundRect.width = 18.5f; // Apparently the default icon width
            EditorGUI.DrawRect(backgroundRect, color);

            // Draw custom icon + label
            EditorGUI.LabelField(selectionRect, icon);
        }

        private static void DrawActivationToggle(Rect selectionRect, GameObject gameObject)
        {
            Rect toggleRect = new Rect(selectionRect);
            toggleRect.x -= 27f;
            toggleRect.width = 13f;

            bool isActive = EditorGUI.Toggle(toggleRect, gameObject.activeSelf);
            if (isActive != gameObject.activeSelf)
            {
                if (!Application.isPlaying)
                {
                    Undo.RecordObject(gameObject, $"Changing active state of {gameObject.name}");
                }
                gameObject.SetActive(isActive);
                EditorSceneManager.MarkSceneDirty(gameObject.scene);
            }
        }

        private static void UpdateIconAsActivationToggle(Rect selectionRect, GameObject gameObject)
        {
            Rect toggleRect = new Rect(selectionRect.x, selectionRect.y, 15f, selectionRect.height);
            if (Event.current.type == EventType.MouseDown
                && Event.current.button == 0
                && toggleRect.Contains(Event.current.mousePosition))
            {
                if (!Application.isPlaying)
                {
                    Undo.RecordObject(gameObject, $"Changing active state of {gameObject.name}");
                }
                gameObject.SetActive(!gameObject.activeSelf);
                if (!Application.isPlaying)
                {
                    EditorSceneManager.MarkSceneDirty(gameObject.scene);
                }
                Event.current.Use();
            }
        }
    }
#endif
}
