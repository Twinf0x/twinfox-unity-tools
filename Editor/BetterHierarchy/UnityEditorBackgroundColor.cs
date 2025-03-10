using UnityEditor;
using UnityEngine;

namespace Twinfox.EditorTools
{
#if UNITY_EDITOR
    public static class UnityEditorBackgroundColor
    {
        private static readonly Color _defaultColor = new Color(0.7843f, 0.7843f, 0.7843f);
        private static readonly Color _defaultColorDark = new Color(0.2196f, 0.2196f, 0.2196f);

        private static readonly Color _selectedColor = new Color(0.22745f, 0.447f, 0.6902f);
        private static readonly Color _selectedColorDark = new Color(0.1725f, 0.3647f, 0.5294f);

        private static readonly Color _selectedUnfocusedColor = new Color(0.68f, 0.68f, 0.68f);
        private static readonly Color _selectedUnfocusedColorDark = new Color(0.3f, 0.3f, 0.3f);

        private static readonly Color _hoveredColor = new Color(0.698f, 0.698f, 0.698f);
        private static readonly Color _hoveredColorDark = new Color(0.2706f, 0.2706f, 0.2706f);

        public static Color Get(bool isSelected, bool isHovered, bool isFocused)
        {
            if (isSelected)
            {
                if (isFocused)
                {
                    return EditorGUIUtility.isProSkin ? _selectedColorDark : _selectedColor;
                }
                else
                {
                    return EditorGUIUtility.isProSkin ? _selectedUnfocusedColorDark : _selectedUnfocusedColor;
                }
            }
            else if (isHovered)
            {
                return EditorGUIUtility.isProSkin ? _hoveredColorDark : _hoveredColor;
            }
            else
            {
                return EditorGUIUtility.isProSkin ? _defaultColorDark : _defaultColor;
            }
        }
    }
#endif
}
