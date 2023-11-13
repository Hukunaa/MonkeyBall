using TutorialSystem.Scripts.Runtime;
using UnityEditor;
using UnityEngine;

namespace TutorialSystem.Scripts.Editor
{
    [CustomPropertyDrawer(typeof(TutorialEntry))]
    public class TutorialEntryDrawer : PropertyDrawer
    {
        private SerializedProperty text;
        private SerializedProperty popupPosition;
        private SerializedProperty pauseTime;
        private SerializedProperty fadeBackground;
        private SerializedProperty onShowTutorialEntry;
        private SerializedProperty onHideTutorialEntry;
        private SerializedProperty appearCondition;
        private SerializedProperty showTutorialEventChannel;
        private SerializedProperty showDelay;
        private SerializedProperty delayDuration;
        private SerializedProperty hideCondition;
        private SerializedProperty hideTutorialEventChannel;
        private SerializedProperty hideTutorialButton;
        private SerializedProperty hideDelay;

        private const int fieldHeight = 16;
        private const int marginHeight = 2;

        private void GetSerializedProperties(SerializedProperty property)
        {
            text = property.FindPropertyRelative("_text");
            popupPosition = property.FindPropertyRelative("_tutorialPosition");
            pauseTime = property.FindPropertyRelative("_pauseTime");
            fadeBackground = property.FindPropertyRelative("_useFadeBackground");
            onShowTutorialEntry = property.FindPropertyRelative("_onTutorialEntryStart");
            onHideTutorialEntry = property.FindPropertyRelative("_onTutorialEntryEnd");
            appearCondition = property.FindPropertyRelative("_appearCondition");
            showTutorialEventChannel = property.FindPropertyRelative("_showTutorialEventChannel");
            showDelay = property.FindPropertyRelative("_showDelay");
            delayDuration = property.FindPropertyRelative("_delayDuration");
            hideCondition = property.FindPropertyRelative("_hideCondition");
            hideTutorialEventChannel = property.FindPropertyRelative("_hideTutorialEventChannel");
            hideTutorialButton = property.FindPropertyRelative("_hideTutorialButton");
            hideDelay = property.FindPropertyRelative("_hideDelay");
        }
    
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            GetSerializedProperties(property);

            float height = fieldHeight;

            if (property.isExpanded)
            {
                int fieldCount = 6;

                switch (appearCondition.intValue)
                {
                    case 0:
                        break;
                    case 1:
                        fieldCount++;
                        break;
                    case 2:
                        fieldCount = fieldCount + 2;
                        break;
                }

                switch (hideCondition.intValue)
                {
                    case 0:
                        break;
                    case 1:
                        fieldCount++;
                        break;
                    case 2:
                        fieldCount++;
                        break;
                    case 3:
                        fieldCount = fieldCount + 2;
                        break;
                    case 4:
                        fieldCount++;
                        break;
                }

                if (hideCondition.intValue > 1)
                {
                    fieldCount++;
                }

                int showFaderEventHeight = (int)EditorGUI.GetPropertyHeight(onShowTutorialEntry);
                int hideFaderEventHeight = (int)EditorGUI.GetPropertyHeight(onHideTutorialEntry);
                int faderEventsHeight = showFaderEventHeight + hideFaderEventHeight;
        
                height = height + 64 + (fieldHeight * fieldCount) + faderEventsHeight + (fieldCount + 3) * marginHeight;
            }
        
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
        
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            GetSerializedProperties(property);

            Rect foldoutRect = new Rect(position.x, position.y, position.width, fieldHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label);

            if (property.isExpanded)
            {
                int yOffset = fieldHeight + marginHeight;
                // Draw fields - pass GUIContent.none to each so they are drawn without labels
                var textRect = new Rect(position.x, position.y + yOffset, position.width, 64);
                EditorGUI.PropertyField(textRect, text, new GUIContent("Text: "));
            
                yOffset = yOffset + 64 + marginHeight;
                var positionRect = new Rect(position.x , position.y + yOffset, position.width, 16);
                EditorGUI.PropertyField(positionRect, popupPosition, new GUIContent("Popup position: "));
            
                yOffset = yOffset + fieldHeight + marginHeight;
                var pauseRect = new Rect(position.x, position.y + yOffset, position.width, 16);
                EditorGUI.PropertyField(pauseRect, pauseTime, new GUIContent("Pause time: "));
            
                yOffset = yOffset + fieldHeight + marginHeight;
                var fadeBackgroundRect = new Rect(position.x, position.y + yOffset, position.width, 16);
                EditorGUI.PropertyField(fadeBackgroundRect, fadeBackground, new GUIContent("Use fade background: "));
            
                yOffset = yOffset + fieldHeight + marginHeight;
                var appearConditionRect = new Rect(position.x,  position.y + yOffset, position.width, 16);
                EditorGUI.PropertyField(appearConditionRect, appearCondition, new GUIContent("Appear Condition: "));
                
                switch (appearCondition.intValue)
                {
                    case 0:
                        break;
                    case 1:
                        EditorGUI.indentLevel = 1;
                        yOffset = yOffset + fieldHeight + marginHeight;
                        var delayDurationRect = new Rect(position.x,  position.y + yOffset, position.width, 16);
                        EditorGUI.PropertyField(delayDurationRect, delayDuration, new GUIContent("Delay duration: "));
                        EditorGUI.indentLevel = 0;
                        break;
                    case 2:
                        EditorGUI.indentLevel = 1;
                        yOffset = yOffset + fieldHeight + marginHeight;
                        var showTutorialEventChannelRect = new Rect(position.x,  position.y + yOffset, position.width, 16);
                        EditorGUI.PropertyField(showTutorialEventChannelRect, showTutorialEventChannel, new GUIContent("Show on: "));
                        yOffset = yOffset + fieldHeight + marginHeight;
                        var showDelayRect = new Rect(position.x,  position.y + yOffset, position.width, 16);
                        EditorGUI.PropertyField(showDelayRect, showDelay, new GUIContent("Show Delay: "));
                        EditorGUI.indentLevel = 0;
                        break;
                }
                
                yOffset = yOffset + fieldHeight + marginHeight;
                var hideConditionRect = new Rect(position.x, position.y + yOffset, position.width, 16);
                EditorGUI.PropertyField(hideConditionRect, hideCondition, new GUIContent("Hide condition: "));
                
                Rect hideDelayRect;
                switch (hideCondition.intValue)
                {
                    case 2:
                        EditorGUI.indentLevel = 1;
                        yOffset = yOffset + fieldHeight + marginHeight;
                        var hideTutorialButtonRect = new Rect(position.x,  position.y + yOffset, position.width, 16);
                        EditorGUI.PropertyField(hideTutorialButtonRect, hideTutorialButton, new GUIContent("Button: "));
                        EditorGUI.indentLevel = 0;
                        break;
                    case 3:
                        EditorGUI.indentLevel = 1;
                        yOffset = yOffset + fieldHeight + marginHeight;
                        var hideTutorialEventChannelRect = new Rect(position.x,  position.y + yOffset, position.width, 16);
                        EditorGUI.PropertyField(hideTutorialEventChannelRect, hideTutorialEventChannel, new GUIContent("Event Channel: "));
                        yOffset = yOffset + fieldHeight + marginHeight;
                        hideDelayRect = new Rect(position.x,  position.y + yOffset, position.width, 16);
                        EditorGUI.PropertyField(hideDelayRect, hideDelay, new GUIContent("Hide Delay: "));
                        EditorGUI.indentLevel = 0;
                        break;
                    case 4:
                        yOffset = yOffset + fieldHeight + marginHeight;
                        hideDelayRect = new Rect(position.x,  position.y + yOffset, position.width, 16);
                        EditorGUI.PropertyField(hideDelayRect, hideDelay, new GUIContent("Hide Delay: "));
                        EditorGUI.indentLevel = 0;
                        break;
                    default:
                        break;
                }
            
                yOffset = yOffset + fieldHeight + marginHeight * 2;
                var onShowTutorialRect = new Rect(position.x,  position.y + yOffset, position.width, 16);
                EditorGUI.PropertyField(onShowTutorialRect, onShowTutorialEntry, new GUIContent("On show tutorial entry: "));
                yOffset = yOffset + (int)EditorGUI.GetPropertyHeight(onShowTutorialEntry) + marginHeight;
                var onHideTutorialRect = new Rect(position.x,  position.y + yOffset, position.width, 16);
                EditorGUI.PropertyField(onHideTutorialRect, onHideTutorialEntry, new GUIContent("On hide tutorial entry: "));
            }
        
            EditorGUI.EndProperty();
        }
    }
}