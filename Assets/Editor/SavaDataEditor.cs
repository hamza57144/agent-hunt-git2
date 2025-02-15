using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SavedData))] // Attach this editor script to your SavedData class
public class SavaDataEditor : Editor
{
    private string[] options = { "All", "Weapons", "Players", "Levels" }; // Options for the dropdown
    private int selectedIndex = 0; // Index of the selected dropdown item
    private int levelIndex = 1; // Default value set to 1

    private void OnEnable()
    {
        levelIndex = 1; 
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        selectedIndex = EditorGUILayout.Popup("Select Data to Clear", selectedIndex, options);

        // Create a custom GUIStyle for the button
        GUIStyle dangerButtonStyle = new GUIStyle(GUI.skin.button)
        {
            normal = { textColor = Color.white },
            fontStyle = FontStyle.Bold,
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter
        };

        // Set the button background color
        Color dangerColor = new Color(0.8f, 0.1f, 0.1f);
        GUI.backgroundColor = dangerColor;

        if (GUILayout.Button($"Clear {options[selectedIndex]} Data", dangerButtonStyle))
        {
            SavedData savaData = (SavedData)target;
            switch (selectedIndex)
            {
                case 0:
                    savaData.ClearAllData();
                    Debug.Log("All data cleared!");
                    break;
                case 1:
                    savaData.ClearWeaponsData();
                    Debug.Log("Weapons data cleared!");
                    break;
                case 2:
                    savaData.ClearPlayersData();
                    Debug.Log("Players data cleared!");
                    break;
                case 3:
                    savaData.ClearLevelsData();
                    Debug.Log("Levels data cleared!");
                    break;
            }

            // Mark the object as dirty to save changes
            EditorUtility.SetDirty(target);
        }

        // Reset background color
        GUI.backgroundColor = Color.white;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Play Level", EditorStyles.boldLabel);

        levelIndex = EditorGUILayout.IntField("Level No", levelIndex);

        if (GUILayout.Button($"Play Level {levelIndex}"))
        {
            SavedData savaData = (SavedData)target;

            // Save level progress
            savaData.OpenLevel(levelIndex-1);
            Debug.Log($"Opened level {levelIndex}");

            // Start play mode if not already playing
            if (!EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = true;
            }

            EditorUtility.SetDirty(target);
        }
    }
}
