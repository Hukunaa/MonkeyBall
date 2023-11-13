using PlayerNameInput;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace Tools
{
    public class DataManagementTool : EditorWindow
    {
        private Vector2 scrollPos;
    
        [MenuItem("Tools/DataManager")]
        private static void ShowWindow()
        {
            var window = GetWindow<DataManagementTool>();
            window.titleContent = new GUIContent("Data Manager");
            window.Show();
        }

        private void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            if (GUILayout.Button("Reset all data"))
            {
                DataLoader.ResetAllData();
            }

            EditorGUILayout.Space();
            GUILayout.Label("Player Data", EditorStyles.boldLabel);
        
            if (GUILayout.Button("Reset Player name"))
            {
                PlayerNameDataManager.DeletePlayerNameData();
            }
        
            if (GUILayout.Button("Reset Player currencies data"))
            {
                DataLoader.ResetPlayerCurrenciesData();
            }

            if (GUILayout.Button("Reset Player BattlePass data"))
            {
                DataLoader.ResetPlayerBattlePassData();
            }
        
            if (GUILayout.Button("Reset Player Score data"))
            {
                DataLoader.ResetPlayerScoreData();
            }
        
            if (GUILayout.Button("Reset Player Skins Inventory data"))
            {
                DataLoader.ResetPlayerSkinsInventoryData();
            }
            
            if (GUILayout.Button("Reset Player current Skins data"))
            {
                DataLoader.ResetPlayerCurrentSkinsData();
            }
            
            if (GUILayout.Button("Reset Store Skins data"))
            {
                DataLoader.ResetStoreSkinsData();
            }
        
            EditorGUILayout.EndScrollView();
        }
    }
}
