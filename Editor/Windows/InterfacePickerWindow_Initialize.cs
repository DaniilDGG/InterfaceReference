using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InterfaceReference.Editor
{
    public partial class InterfacePickerWindow
    {
        private void RefreshValidObjects()
        {
            _validObjects = new List<Object>();

            var gameObjectGuids = AssetDatabase.FindAssets("t:GameObject");
            
            foreach (var guid in gameObjectGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                
                if (go && go.GetComponents<Component>().Any(c => _targetType.IsInstanceOfType(c))) _validObjects.Add(go);
            }
            
            var allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            
            foreach (var go in allGameObjects)
            {
                if (EditorUtility.IsPersistent(go) || go.hideFlags != HideFlags.None) continue;
                
                if (go.GetComponents<Component>().Any(c => _targetType.IsInstanceOfType(c))) _validObjects.Add(go);
            }

            var scriptableGuids = AssetDatabase.FindAssets("t:ScriptableObject");
            
            foreach (var guid in scriptableGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                
                if (so && _targetType.IsInstanceOfType(so)) _validObjects.Add(so);
            }
        }
        
        public static void ShowPicker(Type targetType, Action<Object> onSelected)
        {
            var window = CreateInstance<InterfacePickerWindow>();
            
            window._targetType = targetType;
            window._onSelected = onSelected;
            window.titleContent = new GUIContent("Select object implementing " + targetType.Name);
            
            // ReSharper disable PossibleLossOfFraction
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 600, 400);

            window.RefreshValidObjects();
            window.ShowUtility();
        }
    }
}