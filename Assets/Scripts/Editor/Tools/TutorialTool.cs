using TutorialSystem.Scripts.Runtime;
using UnityEditor;
using UnityEngine;

namespace Tools
{
    public class TutorialTool : EditorWindow
    {
        [MenuItem("Tools/TutorialTool")]
        private static void ShowWindow()
        {
            var window = GetWindow<TutorialTool>();
            window.titleContent = new GUIContent("Tutorial Tool");
            window.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Delete tutorial Data"))
            {
                TutorialDataManager.DeleteTutorialData();
            }
            
            
        }
    }
}