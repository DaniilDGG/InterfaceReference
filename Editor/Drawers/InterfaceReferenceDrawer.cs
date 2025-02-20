using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace InterfaceReference.Editor
{
    [CustomPropertyDrawer(typeof(InterfaceReference<>), true)]
    public class InterfaceReferenceDrawer : PropertyDrawer
    {
        #region Fields
        
        private bool _isObjectPinged;
        
        #endregion
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var fieldType = fieldInfo.FieldType;
            Type targetType;
            
            while (true)
            {
                if (fieldType.GetGenericTypeDefinition() != typeof(InterfaceReference<>))
                {
                    if (!fieldType.IsGenericType)
                    {
                        Debug.LogError("Invalid type!");
                        
                        return;
                    }
                    
                    fieldType = fieldType.GenericTypeArguments[0];
                    
                    continue;
                }
                
                targetType = fieldType.GenericTypeArguments[0];
                
                break;
            }

            var targetProperty = property.FindPropertyRelative("_target");

            EditorGUI.BeginProperty(position, label, property);

            var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelRect, label);

            var fieldRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width - EditorGUIUtility.labelWidth - 20, EditorGUIUtility.singleLineHeight);
            var buttonRect = new Rect(position.x + position.width - 20, position.y, 20, EditorGUIUtility.singleLineHeight);

            var currentName = targetProperty.objectReferenceValue ? targetProperty.objectReferenceValue.name : "None";

            if (GUI.Button(fieldRect, new GUIContent(currentName), EditorStyles.objectField))
            {
                if (!targetProperty.objectReferenceValue) 
                {
                    ShowPicker(targetType, targetProperty, property);
                    _isObjectPinged = false;
                }
                else 
                {
                    if (!_isObjectPinged)
                    {
                        EditorGUIUtility.PingObject(targetProperty.objectReferenceValue);
                        _isObjectPinged = true;
                    }
                    else
                    {
                        Selection.activeObject = targetProperty.objectReferenceValue;
                        _isObjectPinged = false;
                    }
                }
            }

            if (GUI.Button(buttonRect, EditorGUIUtility.IconContent("DotSelection"), EditorStyles.miniButton)) ShowPicker(targetType, targetProperty, property);

            EditorGUI.EndProperty();
        }

        private void ShowPicker(Type targetType, SerializedProperty targetProperty, SerializedProperty property)
        {
            InterfacePickerWindow.ShowPicker(targetType, (selectedObject) =>
            {
                if (selectedObject)
                {
                    if (selectedObject is GameObject go)
                    {
                        var component = go.GetComponents<Component>().FirstOrDefault(targetType.IsInstanceOfType);
                            
                        if (component) targetProperty.objectReferenceValue = component;
                        else
                        {
                            Debug.LogError($"GameObject '{go.name}' does not have a component implementing {targetType.Name}.", go);
                                
                            targetProperty.objectReferenceValue = null;
                        }
                    }
                    else if (targetType.IsInstanceOfType(selectedObject)) targetProperty.objectReferenceValue = selectedObject;
                    else
                    {
                        Debug.LogError($"Selected object does not implement {targetType.Name}.", selectedObject);
                        
                        targetProperty.objectReferenceValue = null;
                    }
                }
                else targetProperty.objectReferenceValue = null;

                property.serializedObject.ApplyModifiedProperties();
            });
        }
    }
}