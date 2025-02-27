using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class PlayerSelection :MonoBehaviour
{
    [SerializeField] GameObject GameplayPanel;
    [SerializeField] GameObject PlayerSelectionPanel;
   public   void  SelectPlayer(int index)
   {
        GameData.SaveSelectedPlayer(index);
        Debug.Log($"Selected Player index is {GameData.SelectedPlayerIndex}");
       
     
       
   }
  public void OnSelectPlayerButtonClick()
  {
      Time.timeScale = 0f;
      GameplayPanel.SetActive(false);
      PlayerSelectionPanel.SetActive(true);
  }

 
}
