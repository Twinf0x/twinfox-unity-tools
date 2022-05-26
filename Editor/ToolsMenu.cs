using System.IO;
using UnityEditor;
using UnityEngine;

namespace Twinfox
{
    public static class ToolsMenu
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
}
