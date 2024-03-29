using UnityEditor;
using UnityEngine;

namespace Twinfox.EditorTools
{
#if UNITY_EDITOR
    public static class CreateMenu
    {
        [MenuItem("Assets/Create/Code/MonoBehaviour", priority = 40)]
        public static void CreateNewMonoBehaviour()
        {
            string templatePath = "Packages/com.twinfox.tools/Editor/ScriptTemplates/NewBehaviourScript.cs.txt";

            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewScript.cs");
        }

        [MenuItem("Assets/Create/Code/ScriptableObject", priority = 41)]
        public static void CreateNewScriptableObject()
        {
            string templatePath = "Packages/com.twinfox.tools/Editor/ScriptTemplates/NewScriptableObject.cs.txt";

            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewScriptableObject.cs");
        }

        [MenuItem("Assets/Create/Code/Enum", priority = 42)]
        public static void CreateNewEnum()
        {
            string templatePath = "Packages/com.twinfox.tools/Editor/ScriptTemplates/NewEnum.cs.txt";

            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewEnum.cs");
        }
    }
#endif
}
