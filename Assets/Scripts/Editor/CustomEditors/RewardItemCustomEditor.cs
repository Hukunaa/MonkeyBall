using ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace CustomEditors
{
    [UnityEditor.CustomEditor(typeof(RewardItem))]
    public class RewardItemCustomEditor : Editor
    {
        private SerializedProperty _rewardType;
        private SerializedProperty _title;
        private SerializedProperty _description;
        private SerializedProperty _xpRequired;
        private SerializedProperty _rewardSprite;
        private SerializedProperty _coinsReward;
        private SerializedProperty _skinReward;
        private SerializedProperty _isUsed;

        private void OnEnable()
        {
            GetSerializedProperties();
        }
        
        private void GetSerializedProperties()
        {
            _rewardType = serializedObject.FindProperty("_rewardType");
            _coinsReward = serializedObject.FindProperty("_coinsReward");
            _skinReward = serializedObject.FindProperty("_skinReward");
            _title = serializedObject.FindProperty("_title");
            _description = serializedObject.FindProperty("_description");
            _xpRequired = serializedObject.FindProperty("_xpRequired");
            _rewardSprite = serializedObject.FindProperty("_rewardSprite");
            _isUsed = serializedObject.FindProperty("_isUsed");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_rewardType, new GUIContent("Reward Type"));
            EditorGUI.indentLevel++;
            switch (_rewardType.intValue)
            {
                case 0:
                    EditorGUILayout.PropertyField(_coinsReward, new GUIContent("Coins Amount"));
                    break;
                case 1:
                    EditorGUILayout.PropertyField(_skinReward, new GUIContent("Skin Reward"));
                    break;
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.PropertyField(_title, new GUIContent("Title"));
            EditorGUILayout.PropertyField(_description, new GUIContent("Description"));
            
            EditorGUILayout.PropertyField(_xpRequired, new GUIContent("XP Required"));
            EditorGUILayout.PropertyField(_rewardSprite, new GUIContent("Reward Sprite"));
            
            EditorGUILayout.PropertyField(_isUsed, new GUIContent("Is Used"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}