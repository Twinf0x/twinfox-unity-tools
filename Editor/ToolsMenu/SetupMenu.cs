using System.IO;
using UnityEditor;
using UnityEngine;

namespace Twinfox.EditorTools
{
#if UNITY_EDITOR
    public static class SetupMenu
    {
        [MenuItem("Tools/Setup/Setup Folder Structure")]
        public static void SetupFolderStructure()
        {
            CreateDirectories(Path.Combine(Application.dataPath, "_Project"), "Art", "Scripts", "Prefabs", "Scenes", "Audio");
            AssetDatabase.Refresh();
        }

        public static void CreateDirectories(string basePath, params string[] directories)
        {
            foreach (var dir in directories)
            {
                Directory.CreateDirectory(Path.Combine(basePath, dir));
            }
        }
    }
#endif
}
