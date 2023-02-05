using UnityEditor;
using UnityEngine;

namespace Twinfox.EditorTools
{
#if UNITY_EDITOR
    public static class CreateMenu
    {
        [MenuItem("Assets/Create/Custom/MonoBehaviour", priority = 40)]
        public static void CreateNewMonoBehaviour()
        {
            string templatePath = "Packages/com.twinfox.tools/ScriptTemplates/NewBehaviourScript.cs.txt";

            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewScript.cs");
        }

        [MenuItem("Assets/Create/Custom/ScriptableObject", priority = 41)]
        public static void CreateNewScriptableObject()
        {
            string templatePath = "Packages/com.twinfox.tools/ScriptTemplates/NewScriptableObject.cs.txt";

            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewScriptableObject.cs");
        }

        [MenuItem("Assets/Create/Custom/Enum", priority = 42)]
        public static void CreateNewEnum()
        {
            string templatePath = "Packages/com.twinfox.tools/ScriptTemplates/NewEnum.cs.txt";

            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewEnum.cs");
        }
    }
#endif
}
