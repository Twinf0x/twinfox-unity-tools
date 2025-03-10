using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Twinfox.EditorTools
{
#if UNITY_EDITOR
    public class CustomHierarchyDisplaySettingsProvider : SettingsProvider
    {
        private const string UseCustomHierarchyIconsKey = "Twinfox_CustomHierarchyIcons";
        private const string UseCustomIconsForPrefabsKey = "Twinfox_CustomIconsForPrefabs";
        private const string UseIconAsActivationToggleKey = "Twinfox_IconsAsActivationToggle";
        private const string UseSeparateActivationToggleKey = "Twinfox_SeparateActivationToggle";

        public static bool UseCustomHierarchyIcons
        {
            get
            {
                return EditorPrefs.GetBool(UseCustomHierarchyIconsKey, true);
            }

            set
            {
                EditorPrefs.SetBool(UseCustomHierarchyIconsKey, value);
            }
        }

        public static bool UseCustomIconsForPrefabs
        {
            get 
            { 
                return EditorPrefs.GetBool(UseCustomIconsForPrefabsKey, true); 
            }

            set
            {
                EditorPrefs.SetBool(UseCustomIconsForPrefabsKey, value);
            }
        }

        public static bool UseIconAsActivationToggle
        {
            get
            {
                return EditorPrefs.GetBool(UseIconAsActivationToggleKey, true);
            }

            set
            {
                EditorPrefs.SetBool(UseIconAsActivationToggleKey, value);
            }
        }

        public static bool UseSeparateActivationToggle
        {
            get
            {
                return EditorPrefs.GetBool(UseSeparateActivationToggleKey, false);
            }

            set
            {
                EditorPrefs.SetBool(UseSeparateActivationToggleKey, value);
            }
        }

        public CustomHierarchyDisplaySettingsProvider(string path, SettingsScope scopes, 
            IEnumerable<string> keywords = null) 
            : base(path, scopes, keywords)
        {

        }

        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);

            GUILayout.Space(5f);
            GUILayout.BeginVertical();
            bool useCustomIcons = UseCustomHierarchyIcons;
            bool value = EditorGUILayout.Toggle("Custom hierarchy icons", useCustomIcons);
            if (useCustomIcons != value)
            {
                UseCustomHierarchyIcons = value;
            }

            bool useCustomIconsForPrefabs = UseCustomIconsForPrefabs;
            value = EditorGUILayout.Toggle("Custom icons for prefabs", useCustomIconsForPrefabs);
            if (useCustomIconsForPrefabs != value)
            {
                UseCustomIconsForPrefabs = value;
            }

            bool useIconAsActivationToggle = UseIconAsActivationToggle;
            value = EditorGUILayout.Toggle("Icons as activation toggle", useIconAsActivationToggle);
            if (useIconAsActivationToggle != value)
            {
                UseIconAsActivationToggle = value;
            }

            bool useSeparateActivationToggle = UseSeparateActivationToggle;
            value = EditorGUILayout.Toggle("Separate activation toggle", useSeparateActivationToggle);
            if (useSeparateActivationToggle != value)
            {
                UseSeparateActivationToggle = value;
            }
            GUILayout.EndVertical();
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new CustomHierarchyDisplaySettingsProvider("Twinfox/Custom Hierarchy", SettingsScope.User);
        }
    }
#endif
}
