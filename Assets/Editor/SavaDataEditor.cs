using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SavedData))] // Attach this editor script to your SavaData class
public class SavaDataEditor : Editor
{
    
    private string[] options = { "All", "Weapons", "Players"}; // Options for the dropdown
    private int selectedIndex = 0; // Index of the selected dropdown item

    public override void OnInspectorGUI()
    {
        // Draw the default Inspector
        DrawDefaultInspector();

        // Add a dropdown to select data type
        selectedIndex = EditorGUILayout.Popup("Select Data to Clear", selectedIndex, options);

        // Add a button to clear the selected data
        if (GUILayout.Button($"Clear {options[selectedIndex]} Data"))
        {
            SavedData savaData = (SavedData)target; // Reference to the SavaData script
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
