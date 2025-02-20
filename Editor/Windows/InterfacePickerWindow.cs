using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace InterfaceReference.Editor
{
    public partial class InterfacePickerWindow : EditorWindow
    {
        #region Fields

        private Vector2 _scrollPos;
        private List<UnityEngine.Object> _validObjects;

        private Type _targetType;
        private Action<UnityEngine.Object> _onSelected;

        private int _selectedTab;

        #endregion

        private void OnGUI()
        {
            if (_targetType == null)
            {
                CloseWindow();
                
                return;
            }

            GUILayout.Space(3);
    
            var style = new GUIStyle(EditorStyles.toolbarButton)
            {
                fontSize = 12,
                fixedHeight = 25,
                fixedWidth = 60
            };

            var selectedStyle = new GUIStyle(style)
            {
                normal = { background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn.png") as Texture2D, textColor = Color.white }
            };

            var tabNames = new[] { "Assets", "Scene" };

            GUILayout.BeginHorizontal();
            
            for (var index = 0; index < tabNames.Length; index++)
            {
                var currentStyle = index == _selectedTab ? selectedStyle : style;
                
                if (GUILayout.Button(tabNames[index], currentStyle)) _selectedTab = index;
                
                GUILayout.Space(1);
            }
            
            GUILayout.EndHorizontal();

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            var objectsToDisplay = _selectedTab == 1 ?
                _validObjects.Where(o => !EditorUtility.IsPersistent(o)).ToList() :
                _validObjects.Where(EditorUtility.IsPersistent).ToList();

            var nullContent = EditorGUIUtility.ObjectContent(null, _targetType);

            if (GUILayout.Button(nullContent, EditorStyles.toolbarButton))
            {
                _onSelected?.Invoke(null);
                Close();

                return;
            }
            
            foreach (var obj in objectsToDisplay)
            {
                var content = EditorGUIUtility.ObjectContent(obj, _targetType);

                if (!GUILayout.Button(content, EditorStyles.toolbarButton)) continue;
                
                _onSelected?.Invoke(obj);
                Close();
            }

            EditorGUILayout.EndScrollView();
        }

        private async void CloseWindow()
        {
            await Task.Delay(10);

            if (_targetType != null) return;

            Close();
        }
    }
}