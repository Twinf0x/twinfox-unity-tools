using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Twinfox.EditorTools
{
#if UNITY_EDITOR
    public static class OpenFolderTool
    {
        [OnOpenAsset(1, OnOpenAssetAttributeMode.Execute)]
        public static bool OnOpenAsset(int instanceID, int line, int column)
        {
            Event e = Event.current;
            if (e == null || !e.shift)
            {
                // Let Unity handle it
                return false;
            }

            Object obj = EditorUtility.InstanceIDToObject(instanceID);
            string path = AssetDatabase.GetAssetPath(obj);

            if (AssetDatabase.IsValidFolder(path))
            {
                EditorUtility.RevealInFinder(path);
                // Tell Unity we handled it
                return true;
            }
            return false;
        }
    }
#endif
}
