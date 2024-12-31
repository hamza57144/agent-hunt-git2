using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class PlayerSelection :MonoBehaviour
{
    [SerializeField] GameObject GameplayPanel;
    [SerializeField] GameObject PlayerSelectionPanel;
   public   void  SelectPlayer(int index)
    {
        GameManager.instance.RestartGame();
       GameData.SaveSelectedPlayer(index);
       
    }
  public void OnSelectPlayerButtonClick()
  {
      Time.timeScale = 0f;
      GameplayPanel.SetActive(false);
      PlayerSelectionPanel.SetActive(true);
  }

 
}
