using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SavedData))] // Attach this editor script to your SavaData class
public class SavaDataEditor : Editor
{
    
    private string[] options = { "All", "Weapons", "Players"}; // Options for the dropdown
    private int selectedIndex = 0; // Index of the selected dropdown item

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
            }

            // Mark the object as dirty to save changes
            EditorUtility.SetDirty(target);
        }
    }
}
