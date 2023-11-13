using TutorialSystem.Scripts.Runtime;
using UnityEditor;
using UnityEngine;

namespace TutorialSystem.Scripts.Editor
{
    [CustomEditor(typeof(TutorialManager))]
    public class TutorialManagerCustomEditor : UnityEditor.Editor
    {
        private SerializedProperty _tutorialName;
        private SerializedProperty _clickBlocker;
        private SerializedProperty _textPopup;
        private SerializedProperty _fader;
        private SerializedProperty _timeEventChannel;
        private SerializedProperty _tutorialStartCondition;
        private SerializedProperty _tutorialStartEventChannel;
        private SerializedProperty _tutorialStartButton;
        private SerializedProperty _requireOtherTutorialComplete;
        private SerializedProperty _otherTutorialName;
    
        private SerializedProperty _onTutorialStartEvent;
        private SerializedProperty _onTutorialEndEvent;
        private SerializedProperty _tutorialEntries;
    
        private void OnEnable()
        {
            GetSerializedProperties();
        }

        private void GetSerializedProperties()
        {
            _tutorialName = serializedObject.FindProperty("_tutorialName");
            _clickBlocker = serializedObject.FindProperty("_clickBlocker");
            _textPopup = serializedObject.FindProperty("_tutorialPopup");
            _fader = serializedObject.FindProperty("_tutorialFader");
            _timeEventChannel = serializedObject.FindProperty("_timeEventChannel");
            _tutorialStartCondition = serializedObject.FindProperty("_tutorialStartCondition");
            _tutorialStartEventChannel = serializedObject.FindProperty("_startTutorialEventChannel");
            _tutorialStartButton = serializedObject.FindProperty("_startTutorialButton");
            _requireOtherTutorialComplete = serializedObject.FindProperty("_requireOtherTutorialCompleted");
            _otherTutorialName = serializedObject.FindProperty("_requiredTutorialName");
        
            _onTutorialStartEvent = serializedObject.FindProperty("_onTutorialStart");
            _onTutorialEndEvent = serializedObject.FindProperty("_onTutorialEnd");
            _tutorialEntries = serializedObject.FindProperty("_tutorialEntries");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
        
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_tutorialName, new GUIContent("Tutorial name"));
            EditorGUILayout.PropertyField(_tutorialStartCondition, new GUIContent("Tutorial start condition"));
            switch (_tutorialStartCondition.intValue)
            {
                case 0:
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_tutorialStartEventChannel, new GUIContent("Tutorial start event channel"));
                    EditorGUI.indentLevel--;
                    break;
                case 2:
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_tutorialStartButton, new GUIContent("Tutorial start button"));
                    EditorGUI.indentLevel--;
                    break;
            }
        
            EditorGUILayout.PropertyField(_requireOtherTutorialComplete, new GUIContent("Require other tutorial complete"));
            if (_requireOtherTutorialComplete.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_otherTutorialName, new GUIContent("Other tutorial name"));
                EditorGUI.indentLevel--;
            }
        
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("References", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_clickBlocker, new GUIContent("Click blocker"));
            EditorGUILayout.PropertyField(_textPopup, new GUIContent("Text Popup"));
            EditorGUILayout.PropertyField(_fader, new GUIContent("Fader"));
            EditorGUILayout.PropertyField(_timeEventChannel, new GUIContent("Time event channel"));
        
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_onTutorialStartEvent, new GUIContent("On tutorial start"));
            EditorGUILayout.PropertyField(_onTutorialEndEvent, new GUIContent("On tutorial end"));
        
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Entries", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_tutorialEntries, new GUIContent("Tutorial entries"));
        
            serializedObject.ApplyModifiedProperties();
        }
    }
}
