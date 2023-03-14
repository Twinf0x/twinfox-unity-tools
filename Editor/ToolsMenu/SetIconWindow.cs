using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Twinfox.EditorTools
{
#if UNITY_EDITOR
    public class SetIconWindow : EditorWindow
    {
        private const string MENU_PATH = "Assets/Edit/Set Icon";
        private List<Texture2D> _icons = null;
        private int _selectedIcon = 0;
        private Vector2 _scrollPos;

        [MenuItem(MENU_PATH, priority = 0)]
        public static void ShowMenuItem()
        {
            SetIconWindow window = (SetIconWindow)GetWindow(typeof(SetIconWindow));
            window.titleContent = new GUIContent("Set Icon");
            window.Show();
        }

        [MenuItem(MENU_PATH, validate = true)]
        public static bool ShowMenuItemValidation()
        {
            foreach(var asset in Selection.objects)
            {
                if (asset.GetType() != typeof(MonoScript))
                {
                    return false;
                }
            }

            return true;
        }

        private void OnGUI()
        {
            if (_icons == null)
            {
                _icons = new List<Texture2D>();
                string[] assetGuids = AssetDatabase.FindAssets("t:texture2d l:ScriptIcon");

                foreach(var assetGuid in assetGuids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(assetGuid);
                    _icons.Add(AssetDatabase.LoadAssetAtPath<Texture2D>(path));
                }
            }

            if (_icons == null)
            {
                GUILayout.Label("No icons to display. You need to apply the \"ScriptIcon\" tag for icons to show up here");

                if (GUILayout.Button("Close", GUILayout.Width(100)))
                {
                    Close();
                }

                return;
            }

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Cancel", GUILayout.Width(125)))
            {
                Close();
            }

            if (GUILayout.Button("Apply", GUILayout.Width(125)))
            {
                ApplyIcon(_icons[_selectedIcon]);
                Close();
            }

            EditorGUILayout.EndHorizontal();

            _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Height(500));
            _selectedIcon = GUILayout.SelectionGrid(_selectedIcon, _icons.ToArray(), 5);
            GUILayout.EndScrollView();

            // Listen for input
            if (Event.current == null)
            {
                return;
            }

            if (Event.current.isKey)
            {
                switch (Event.current.keyCode)
                {
                    case KeyCode.KeypadEnter:
                    case KeyCode.Return:
                        ApplyIcon(_icons[_selectedIcon]);
                        Close();
                        break;
                    case KeyCode.Escape:
                        Close();
                        break;
                    default:
                        break;
                }
            }
            else if (Event.current.button == 0 && Event.current.clickCount == 2)
            {
                ApplyIcon(_icons[_selectedIcon]);
                Close();
            }
        }

        private void ApplyIcon(Texture2D icon)
        {
            AssetDatabase.StartAssetEditing();

            foreach (var asset in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(asset);

                MonoImporter importer = AssetImporter.GetAtPath(path) as MonoImporter;

                importer.SetIcon(icon);

                AssetDatabase.ImportAsset(path);
            }

            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
        }
    }
#endif
}

