using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Twinfox.EditorTools
{
    /// <summary>
    /// Code is take from this Unity forum thread:
    /// https://forum.unity.com/threads/editor-tool-better-scriptableobject-inspector-editing.484393/
    /// 
    /// Removed some options I didn't want to use
    /// </summary>
    [CustomPropertyDrawer(typeof(EmbeddedScriptableObjectAttribute))]
    public class EmbeddedScriptableObjectDrawer : PropertyDrawer
    {
        #region Style Setup
        /// <summary>
        /// The spacing on the inside of the background rect.
        /// </summary>
        private static float INNER_SPACING = 6.0f;

        /// <summary>
        /// The spacing on the outside of the background rect.
        /// </summary>
        private static float OUTER_SPACING = 4.0f;
        #endregion

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = 0.0f;

            totalHeight += EditorGUIUtility.singleLineHeight;

            if (property.objectReferenceValue == null)
                return totalHeight;

            if (!property.isExpanded)
                return totalHeight;

            SerializedObject targetObject = new SerializedObject(property.objectReferenceValue);

            if (targetObject == null)
                return totalHeight;

            SerializedProperty field = targetObject.GetIterator();

            field.NextVisible(true);

            while (field.NextVisible(false))
            {
                totalHeight += EditorGUI.GetPropertyHeight(field, true) + EditorGUIUtility.standardVerticalSpacing;
            }

            totalHeight += INNER_SPACING * 2;
            totalHeight += OUTER_SPACING * 2;

            return totalHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect fieldRect = new Rect(position);
            fieldRect.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(fieldRect, property, label, true);

            if (property.objectReferenceValue == null)
            {
                return;
            }

            property.isExpanded = EditorGUI.Foldout(fieldRect, property.isExpanded, GUIContent.none, true);

            if (!property.isExpanded)
            {
                return;
            }

            SerializedObject targetObject = new SerializedObject(property.objectReferenceValue);

            if (targetObject == null)
            {
                return;
            }

            #region Format Field Rects
            List<Rect> propertyRects = new List<Rect>();
            Rect marchingRect = new Rect(fieldRect);

            Rect bodyRect = new Rect(fieldRect);
            bodyRect.xMin += EditorGUI.indentLevel * 14;
            bodyRect.yMin += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + OUTER_SPACING;

            SerializedProperty field = targetObject.GetIterator();
            field.NextVisible(true);

            marchingRect.y += INNER_SPACING + OUTER_SPACING;

            while (field.NextVisible(false))
            {
                marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing;
                marchingRect.height = EditorGUI.GetPropertyHeight(field, true);
                propertyRects.Add(marchingRect);
            }

            marchingRect.y += INNER_SPACING;

            bodyRect.yMax = marchingRect.yMax;
            #endregion

            DrawBackground(bodyRect);

            #region Draw Fields
            EditorGUI.indentLevel++;

            int index = 0;
            field = targetObject.GetIterator();
            field.NextVisible(true);

            //Replacement for "editor.OnInspectorGUI ();" so we have more control on how we draw the editor
            while (field.NextVisible(false))
            {
                try
                {
                    EditorGUI.PropertyField(propertyRects[index], field, true);
                }
                catch (StackOverflowException)
                {
                    field.objectReferenceValue = null;
                    Debug.LogError("Detected self-nesting cauisng a StackOverflowException, avoid using the same " +
                        "object iside a nested structure.");
                }

                index++;
            }

            targetObject.ApplyModifiedProperties();

            EditorGUI.indentLevel--;
            #endregion
        }

        /// <summary>
        /// Draws the Background
        /// </summary>
        /// <param name="rect">The Rect where the background is drawn.</param>
        private void DrawBackground(Rect rect)
        {
            EditorGUI.HelpBox(rect, "", MessageType.None);
        }
    }
}

